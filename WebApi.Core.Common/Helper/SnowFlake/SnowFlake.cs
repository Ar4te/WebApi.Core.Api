namespace WebApi.Core.Common.Helper.SnowFlake
{
    public class SnowFlake
    {
        // 上次计算Id的时间戳
        private long _lastTimestamp = -1L;
        // WorkId 的位数
        private readonly static int _workerIdBits = 4;
        // 数据中心 ID 的位数
        private readonly static int _datacenterIdBits = 4;
        // 最大工作者Id 31
        private readonly long _maxWorkerId = -1L * (-1L << 4);
        // 最大数据中心Id 31
        private readonly long _maxDatacenterId = -1L * (-1L << 4);
        // 序列号的位数
        private readonly static int _sequenceBits = 20 - _workerIdBits - _datacenterIdBits - 1;
        // worker ID 在时间戳中的位移量
        private readonly int _workerIdShift = 8;
        // 数据中心 ID 在时间戳中的位移量
        private readonly int _datacenterIdShift = 14;
        // 时间戳的位移量
        private readonly int _timestampLeftShift = 18;
        // 序号掩码 4095
        private readonly long _sequenceMask = -1L * (-1L << _sequenceBits);
        // worker ID
        private long _workerId;
        // 数据中心 ID
        private long _datacenterId;
        // 序号
        private long _sequence = 0L;
        // Id 总长度
        // _totalBits = _timestampBits + _workerIdBits + _datacenterIdBits + _sequenceBits

        private readonly object _lock = new object();

        public SnowFlake(long workerId, long datacenterId)
        {
            if (workerId > _maxWorkerId || workerId < 0) throw new ArgumentException($"Worker ID cannot be greater than {_maxWorkerId} or less than 0");

            if (datacenterId > _maxDatacenterId || datacenterId < 0)
                throw new ArgumentException($"Datacenter ID cannot be greater than {_maxDatacenterId} or less than 0");

            _workerId = workerId;
            _datacenterId = datacenterId;
        }

        public long NextId()
        {
            lock (_lock)
            {
                var timestamp = GetTimestamp();
                if (timestamp < _lastTimestamp)
                    throw new Exception($"Invalid timestamp. The last timestamp is {_lastTimestamp} and the current timestamp is {timestamp}");

                if (_lastTimestamp == timestamp)
                {
                    _sequence = (_sequence + 1) & _sequenceMask;
                    if (_sequence == 0)
                    {
                        timestamp = WaitForNextTick(_lastTimestamp);
                    }
                }
                else
                {
                    _sequence = 0;
                }

                var id = ((timestamp - 1288834974657L) << _timestampLeftShift) | (_datacenterId << _datacenterIdShift) | (_workerId << _workerIdShift) | _sequence;
                return id;
            }
        }

        private static long GetTimestamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        private static long WaitForNextTick(long lastTimestamp)
        {
            var timestamp = GetTimestamp();
            while (timestamp <= lastTimestamp)
            {
                timestamp = GetTimestamp();
            }
            return timestamp;
        }
    }
}
