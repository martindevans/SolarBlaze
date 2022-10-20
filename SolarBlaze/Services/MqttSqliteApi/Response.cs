using System.Net;

namespace SolarBlaze.Services.MqttSqliteApi
{
    public class Response
    {
        public bool IsError { get; }
        public string? Message { get; }
        public HttpStatusCode Status { get; }

        private readonly TopicsContainer? _topics;
        public TopicsContainer Data
        {
            get
            {
                if (IsError)
                    throw new InvalidOperationException("Cannot get `Topics` from error Response");
                return _topics!;
            }
        }

        private Response(HttpStatusCode status, string? message)
        {
            IsError = true;
            Status = status;
            Message = message;
            _topics = null;
        }

        private Response(HttpStatusCode status, TopicsContainer data)
        {
            IsError = false;
            Status = status;
            _topics = data;
        }

        public static Response Error(HttpStatusCode status, string? message)
        {
            return new Response(status, message);
        }

        public static Response Ok(HttpStatusCode status, TopicsContainer data)
        {
            return new Response(status, data);
        }
    }
}
