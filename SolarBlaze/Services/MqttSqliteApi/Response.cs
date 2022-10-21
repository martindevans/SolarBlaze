using System.Net;

namespace SolarBlaze.Services.MqttSqliteApi
{
    public class Response<T>
        where T : class

    {
    public bool IsError { get; }
    public string? Message { get; }
    public HttpStatusCode Status { get; }

    private readonly T? _data;

    public T Data
    {
        get
        {
            if (IsError)
                throw new InvalidOperationException("Cannot get `Topics` from error Response");
            return _data!;
        }
    }

    private Response(HttpStatusCode status, string? message)
    {
        IsError = true;
        Status = status;
        Message = message;
        _data = null;
    }

    private Response(HttpStatusCode status, T data)
    {
        IsError = false;
        Status = status;
        _data = data;
    }

    public static Response<T> Error(HttpStatusCode status, string? message)
    {
        return new Response<T>(status, message);
    }

    public static Response<T> Ok(HttpStatusCode status, T data)
    {
        return new Response<T>(status, data);
    }
    }
}
