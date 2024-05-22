using GSRU_APICommon.Settings;

namespace GSRU_API.Common.Settings
{
    public class AppSettings
    {
        public Jwt JWT { get; set; } = default!;
        public IEnumerable<string> CorsOrigins { get; set; } = Enumerable.Empty<string>();
    }
}
