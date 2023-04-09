namespace WebApi.Core.Common.Helper.SnowFlake
{
    public class YittHelper
    {
        public static SnowFlake _snowFlake { get; set; }
        public YittHelper()
        {
            var _workerId = Convert.ToInt64(AppSettings.app("SnowFlake", "WorkerId"));
            var _datacenterId = Convert.ToInt64(AppSettings.app("SnowFlake", "DatacenterId"));
            _snowFlake = new SnowFlake(_workerId, _datacenterId);
        }

        public static long NextId()
        {
            return _snowFlake.NextId();
        }
    }
}
