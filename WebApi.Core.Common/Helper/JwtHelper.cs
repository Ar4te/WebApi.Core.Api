using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Core.Model;

namespace WebApi.Core.Common.Helper
{
    public class JwtHelper
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public JwtHelper(string id, string name)
        {
            Id = id;
            Name = name;
        }
        public string IssueJwt()
        {
            string iss = AppSettings.app("JWTSettings", "Issuer");
            string aud = AppSettings.app("JWTSettings", "Audience");
            string secret = AppSettings.app("JWTSettings", "SecretKey");
            var expires = Convert.ToDouble(AppSettings.app("JWTSettings", "Expires"));

            var claims = new[]
            {
                new Claim("Id",Id),
                new Claim("Name",Name),
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
