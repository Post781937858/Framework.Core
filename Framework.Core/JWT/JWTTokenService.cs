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
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);
            object role;
            object name;
            try
            {
                jwtToken.Payload.TryGetValue(ClaimTypes.Role, out role);
                jwtToken.Payload.TryGetValue(ClaimTypes.Name, out name);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            var tm = new TokenModelJwt
            {
                Uid = (jwtToken.Id).ObjToInt(),
                Name= name.ObjToString(),
                Role = role != null ? role.ObjToString() : "",
            };
            return tm;
        }
    }
}