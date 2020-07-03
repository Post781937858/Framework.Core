using Framework.Core.Common;
using Framework.Core.Models;
using IdentityModel;
using SqlSugar;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Framework.Core
{
    public class JWTTokenService
    {
        public static string GetToken(TokenModelJwt user)
        {
            var _jwtSetting = ServerJwtSetting.GetJwtSetting();
            // 创建用户身份标识
            var claims = new Claim[]
            {
                new Claim(JwtClaimTypes.JwtId, Guid.NewGuid().ToString()),
                new Claim(JwtClaimTypes.Id, user.Uid.ToString(), ClaimValueTypes.Integer32),
                new Claim(JwtClaimTypes.Name, user.Name, ClaimValueTypes.String),
                new Claim(JwtClaimTypes.Role, user.Role?.ToString(),ClaimValueTypes.Integer32)
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

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static TokenModelJwt SerializeJwt(string jwtStr)
        {
            TokenModelJwt tm = new TokenModelJwt();
            try
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);
                object role;
                object name;
                object Id;
                jwtToken.Payload.TryGetValue(JwtClaimTypes.Id, out Id);
                jwtToken.Payload.TryGetValue(JwtClaimTypes.Role, out role);
                jwtToken.Payload.TryGetValue(JwtClaimTypes.Name, out name);
                tm.Uid = Id.ToInt();
                tm.Name = name.ObjToString();
                tm.Role = role != null ? role.ObjToString() : "";
            }
            catch (Exception) { }
            return tm;
        }
    }
}