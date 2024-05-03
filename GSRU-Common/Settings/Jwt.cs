namespace GSRU_APICommon.Settings
{
    public class Jwt
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public int ExpireMinutes { get; set; } = default!;
        public int RefreshExpireMinutes { get; set; } = default!;
    }
}
