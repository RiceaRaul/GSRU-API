using System.Net;

namespace GSRU_API.Common.Models
{
    public class GenericError<T>
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public ApiError<T>? ApiError { get; set; }
    }

    public class ApiError<T>
    {
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
    public static class GenerateGenericError
    {
        public static T Generate<T,U>(HttpStatusCode statusCode, string message, U? data = null) where T : GenericError<U>, new() where U : class
        {
            return new T
            {
                StatusCode = statusCode,
                ApiError = new ApiError<U>
                {
                    Message = message,
                    Data = data
                }
            };
        }
        public static T Generate<T>(HttpStatusCode statusCode, string message, string? data = "") where T : GenericError<string>, new()
        {
            return new T
            {
                StatusCode = statusCode,
                ApiError = new ApiError<string>
                {
                    Message = message,
                    Data = data
                }
            };
        }

        public static T GenerateInternalError<T>(string message) where T : GenericError<string>, new()
        {
            return new T
            {
                StatusCode = HttpStatusCode.InternalServerError,
                ApiError = new ApiError<string>
                {
                    Message = message
                }
            };
        }
    }
}
