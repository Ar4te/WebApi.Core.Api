namespace WebApi.Core.Repository.Cache;

public interface ICache
{
    /// <summary>
    /// 添加缓存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="expireTime"></param>
    /// <returns></returns>
    bool SetCache<T>(string key, T value, DateTime? expireTime = null);

    /// <summary>
    /// 获取缓存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    T GetCache<T>(string key);

    /// <summary>
    /// 根据键精准删除
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    bool RemoveCache(string key);

    /// <summary>
    /// 左侧匹配，右侧模糊
    /// </summary>
    /// <param name="keywords"></param>
    /// <returns></returns>
    Task RemoveKeysLeftLike(string keywords);

    /// <summary>
    /// 获取自增Id
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    long GetIncr(string key);

    /// <summary>
    /// 获取自增Id
    /// </summary>
    /// <param name="key"></param>
    /// <param name="expiresTime"></param>
    /// <returns></returns>
    long GetIncr(string key, TimeSpan expiresTime);

    int SetHashFieldCache<T>(string key, string fieldKey, T fieldValue);
    int SetHashFieldCache<T>(string key, Dictionary<string, T> dict);
    T GetHashFieldCache<T>(string key, string fieldKey);
    Dictionary<string, T> GetHashFieldCache<T>(string key, Dictionary<string, T> dict);
    Dictionary<string, T> GetHashCache<T>(string key);
    List<T> GetHashToListCache<T>(string key);
    bool RemoveHashFieldCache(string key, string fieldKey);
    Dictionary<string, bool> RemoveHashFieldCache(string key, Dictionary<string, bool> dict);
}
