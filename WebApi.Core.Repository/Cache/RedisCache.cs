using Newtonsoft.Json;
using StackExchange.Redis;
using WebApi.Core.Common.Helper;

namespace WebApi.Core.Repository.Cache;

public class RedisCache : ICache
{
    private IDatabase cache;
    private ConnectionMultiplexer conn;

    public RedisCache()
    {
        string redishost = AppSettings.app("Redis", "host");
        int redisport = int.Parse(AppSettings.app("Redis", "port"));
        var configurationOptions = new ConfigurationOptions
        {
            EndPoints = { { redishost, redisport } },
            KeepAlive = 180,
            Password = AppSettings.app("Redis", "password"),
            AllowAdmin = true,
        };
        conn = ConnectionMultiplexer.Connect(configurationOptions);
        cache = conn.GetDatabase();
    }

    private string InitKey(string key) => $"{AppSettings.app("Redis", "preName")}{key}";

    public void Dispose()
    {
        if (conn != null)
        {
            conn.Close();
        }
        GC.SuppressFinalize(this);
    }

    public T GetCache<T>(string key)
    {
        var t = default(T);
        try
        {
            var val = cache.StringGet(InitKey(key));
            if (string.IsNullOrEmpty(val)) return t;

            t = JsonConvert.DeserializeObject<T>(val);
        }
        catch (Exception)
        {
        }
        return t;
    }

    public Dictionary<string, T> GetHashCache<T>(string key)
    {
        Dictionary<string, T> dict = new();
        var hashFields = cache.HashGetAll(InitKey(key));
        foreach (var field in hashFields)
        {
            dict[field.Name] = JsonConvert.DeserializeObject<T>(field.Value);
        }
        return dict;
    }

    public T GetHashFieldCache<T>(string key, string fieldKey)
    {
        var dict = GetHashFieldCache<T>(InitKey(key), new Dictionary<string, T> { { fieldKey, default(T) } });
        return dict[fieldKey];
    }

    public Dictionary<string, T> GetHashFieldCache<T>(string key, Dictionary<string, T> dict)
    {
        foreach (string fieldKey in dict.Keys)
        {
            string fieldVal = cache.HashGet(InitKey(key), fieldKey);
            dict[fieldKey] = JsonConvert.DeserializeObject<T>(fieldVal);
        }
        return dict;
    }

    public List<T> GetHashToListCache<T>(string key)
    {
        var list = new List<T>();
        var hashFields = cache.HashGetAll(InitKey(key));
        foreach (var field in hashFields)
        {
            list.Add(JsonConvert.DeserializeObject<T>(field.Value));
        }
        return list;
    }

    public long GetIncr(string key)
    {
        try
        {
            return cache.StringIncrement(InitKey(key));
        }
        catch (Exception)
        {
            return 0;
        }
    }

    public long GetIncr(string key, TimeSpan expiresTime)
    {
        try
        {
            var qty = cache.StringIncrement(InitKey(key));
            if (qty == 1) cache.KeyExpire(key, expiresTime);
            return qty;
        }
        catch (Exception)
        {
            return 0;
        }
    }

    public bool RemoveCache(string key) => cache.KeyDelete(InitKey(key));

    public bool RemoveHashFieldCache(string key, string fieldKey)
    {
        var dict = new Dictionary<string, bool> { { fieldKey, false } };
        dict = RemoveHashFieldCache(InitKey(key), dict);
        return dict[fieldKey];
    }

    public Dictionary<string, bool> RemoveHashFieldCache(string key, Dictionary<string, bool> dict)
    {
        foreach (var fieldKey in dict.Keys)
        {
            dict[fieldKey] = cache.HashDelete(InitKey(key), fieldKey);
        }
        return dict;
    }

    public async Task RemoveKeysLeftLike(string keywords)
    {
        var redisRes = await cache.ScriptEvaluateAsync(LuaScript.Prepare(
            " local res = redis.call('KEYS', @keywords) " +
            " return res "), new { keywords = $"{InitKey(keywords)}*" });
        if (!redisRes.IsNull)
            cache.KeyDelete((RedisKey[])redisRes);
    }

    public bool SetCache<T>(string key, T value, DateTime? expireTime = null)
    {
        try
        {
            var jsonOpt = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            string strVal = JsonConvert.SerializeObject(value, jsonOpt);
            if (string.IsNullOrEmpty(strVal)) return false;

            if (expireTime == null) return cache.StringSet(InitKey(key), strVal);
            else return cache.StringSet(InitKey(key), strVal, expireTime.Value - DateTime.Now);
        }
        catch (Exception)
        {
        }
        return false;
    }

    public int SetHashFieldCache<T>(string key, string fieldKey, T fieldValue) => SetHashFieldCache<T>(InitKey(key), new Dictionary<string, T> { { fieldKey, fieldValue } });

    public int SetHashFieldCache<T>(string key, Dictionary<string, T> dict)
    {
        int count = 0;
        var jsonOpt = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        foreach (var fieldKey in dict.Keys)
        {
            string fieldVal = JsonConvert.SerializeObject(dict[fieldKey], jsonOpt);
            count += cache.HashSet(InitKey(key), fieldKey, fieldVal) ? 1 : 0;
        }
        return count;
    }
}
