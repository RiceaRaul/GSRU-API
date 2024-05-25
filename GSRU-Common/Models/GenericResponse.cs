using GSRU_API.Common.Models;

namespace GSRU_Common.Models
{
    public class GenericResponse<T> : GenericError<string>
    {
        public T? Data { get; set; }
    }
}
