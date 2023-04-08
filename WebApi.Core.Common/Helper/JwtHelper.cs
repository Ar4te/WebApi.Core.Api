using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Core.Model;

namespace WebApi.Core.Common.Helper
{
    public class JwtHelper
    {
        private static string iss = AppSettings.app("JWTSettings", "Issuer");
        private static string aud = AppSettings.app("JWTSettings", "Audience");
        private static string secret = AppSettings.app("JWTSettings", "SecretKey");
        private static double expires = Convert.ToDouble(AppSettings.app("JWTSettings", "Expires"));

        public static string IssueJwt(string id, string name)
        {
            var claims = new[]
            {
                new Claim("Id",id),
                new Claim("Name",name),
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            var alogrithm = SecurityAlgorithms.HmacSha256;

            var signingCredentials = new SigningCredentials(secretKey, alogrithm);

            var tokenStr = new JwtSecurityToken(iss, aud, claims, DateTime.Now, DateTime.Now.AddDays(expires), signingCredentials);

            var jwtStr = new JwtSecurityTokenHandler().WriteToken(tokenStr);

            return jwtStr;
        }
    }
}
