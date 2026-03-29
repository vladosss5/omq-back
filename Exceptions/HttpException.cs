namespace AskAgainApi.Exceptions
{
    public class HttpException : Exception
    {
        public int Code { get; }

        public HttpException(string message, int code = 400)
        : base(message)
        {
            Code = code;
        }
    }
}
