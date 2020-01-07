
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Infra.Data.Auth
{
    /// <summary>
    /// SigningConfigurations handler
    /// </summary>
    public class SigningConfigurations
    {
        /// <summary>
        /// SecurityKey Key used
        /// </summary>
        public SecurityKey Key { get; }

        /// <summary>
        /// SigningCredentials reference
        /// </summary>
        public SigningCredentials SigningCredentials { get; }

        /// <summary>
        /// constructor
        /// </summary>
        public SigningConfigurations()
        {
            Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("^S3@#Ra80xx!S3rv3r!M4dD3!N-9102"));

            SigningCredentials = new SigningCredentials(
                Key, SecurityAlgorithms.HmacSha256);
        }

        /// <summary>
        /// constructor with specific secret
        /// </summary>
        public SigningConfigurations(string secret)
        {
            Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            SigningCredentials = new SigningCredentials(
                Key, SecurityAlgorithms.HmacSha256);
        }

        public SigningCredentials GetSigningCredentialsForKey(string secret)
        {
            return new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)), SecurityAlgorithms.HmacSha256);
        }
    }
}
