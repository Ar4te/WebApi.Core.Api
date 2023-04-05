namespace WebApi.Core.Common.Global
{
    public class MessageModel<T>
    {
        public int code { get; set; }
        public string msg { get; set; }
        public T response { get; set; }
        public bool success { get; set; }

        public static MessageModel<T> Success(string msg, T response = default)
        {
            return new MessageModel<T> { code = 200, msg = msg, response = response, success = true };
        }

        public static MessageModel<T> Fail(int code, string msg, T response = default)
        {
            return new MessageModel<T> { code = code, msg = msg, response = response, success = false };
        }

        public static MessageModel<T> Fail(string msg, T response = default)
        {
            return new MessageModel<T> { code = 200, msg = msg, response = response, success = false };
        }
    }
}
