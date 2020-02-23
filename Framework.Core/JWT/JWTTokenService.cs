using Framework.Core.Models;
using IdentityModel;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Framework.Core
{
    public interface IJWTTokenService
    {
        string GetToken(Userinfo user);
    }

    public class JWTTokenService : IJWTTokenService
    {
        private readonly ServerJwtSetting _jwtSetting;

        public JWTTokenService()
        {
            _jwtSetting = new ServerJwtSetting
            {
                SecurityKey = "d0ecd23c-dvdb-5005-a2ka-0fea210c858a", // 密钥
                Issuer = "jwtIssuertest", // 颁发者
                Audience = "jwtAudiencetest", // 接收者
                ExpireSeconds = 60 * 60 * 24 * 7 // 7t 过期时间
            };
        }

        public string GetToken(Userinfo user)
        {
            // 创建用户身份标识
            var claims = new Claim[]
            {
                new Claim(JwtClaimTypes.JwtId, Guid.NewGuid().ToString()),
                new Claim(JwtClaimTypes.Id, user.id.ToString(), ClaimValueTypes.Integer32),
                new Claim(JwtClaimTypes.Name, user.Name, ClaimValueTypes.String),
                new Claim(JwtClaimTypes.Scope, user.Power_ID.ToString(),ClaimValueTypes.Integer32)
            };

            // 创建令牌
            var token = new JwtSecurityToken(
                    issuer: _jwtSetting.Issuer,
                    audience: _jwtSetting.Audience,
                    signingCredentials: _jwtSetting.Credentials,
                    claims: claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddSeconds(_jwtSetting.ExpireSeconds)
                );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
    }
}