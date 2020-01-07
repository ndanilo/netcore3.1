namespace Infra.Data.Auth
{
    /// <summary>
    /// TokenConfigurations handler
    /// </summary>
    public class TokenConfigurations
    {
        /// <summary>
        /// Audience
        /// </summary>
        public string Audience { get; set; } = "SearaBoxAudience";

        /// <summary>
        /// Issuer
        /// </summary>
        public string Issuer { get; set; } = "SearaBoxIssuer";

        /// <summary>
        /// Seconds
        /// </summary>
        public int Seconds { get; set; } = 172800;
    }
}
