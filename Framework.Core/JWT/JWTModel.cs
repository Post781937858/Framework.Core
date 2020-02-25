using Framework.Core.Common;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Framework.Core
{

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

        public static ServerJwtSetting GetJwtSetting()
        {
            return new ServerJwtSetting
            {
                SecurityKey = Appsettings.app("JwtSetting:SecurityKey"), // 密钥
                Issuer = Appsettings.app("JwtSetting:Issuer"), // 颁发者
                Audience = Appsettings.app("JwtSetting:Audience"), // 接收者
                ExpireSeconds = 60 * 60 * 24 * (Appsettings.app("JwtSetting:ExpireSeconds")).ToInt() // 7t 过期时间
            };
        }
    }

    /// <summary>
    /// 令牌
    /// </summary>
    public class TokenModelJwt
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Uid { get; set; }


        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 角色
        /// </summary>
        public string Role { get; set; } = string.Empty;

    }
}