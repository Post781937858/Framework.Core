using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Framework.Core
{
    public class JWTModel
    {
    }

    public class JwtSetting
    {
        /// <summary>
        /// 颁发者
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 接收者
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 令牌密码
        /// </summary>
        public string SecurityKey { get; set; }
    }

    public class ServerJwtSetting : JwtSetting
    {
        /// <summary>
        ///  过期时间
        /// </summary>
        public long ExpireSeconds { get; set; }

        /// <summary>
        /// 签名 Microsoft.AspNetCore.Authentication.JwtBearer
        /// </summary>
        public SigningCredentials Credentials
        {
            get
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey));
                return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            }
        }
    }
}