using AuthServer.Core.Configuration;
using AuthServer.Core.Dto;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SharedLibrary.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly CustomTokenOption _tokenOption;

        
        public TokenService(IOptions<CustomTokenOption> tokenOption, UserManager<UserApp> userManager)
        {
            _tokenOption = tokenOption.Value;
            _userManager = userManager;
        }
        private string CreateRefreshToken()
        {
            // System.Security.Cryptography; kütüphanesi ile random number üretme ve onu byte'a çevirme
            var numberByte = new Byte[32];
            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(numberByte);


            return Convert.ToBase64String(numberByte);
        }
        private IEnumerable<Claim> GetClaims(UserApp userApp,List<string> audiences)
        {
            var userList = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,userApp.Id),
                new Claim(JwtRegisteredClaimNames.Email,userApp.Email),
                new Claim(ClaimTypes.Name,userApp.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            //audiences lerini ekledik
            userList.AddRange(audiences.Select(p => new Claim(JwtRegisteredClaimNames.Aud, p)));
            return userList;


        }
        private IEnumerable<Claim> GetClaimsByClient(Client client)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                //Sub => Kimin için oluşturuyoruz
                new Claim(JwtRegisteredClaimNames.Sub,client.Id.ToString())
            };
            claims.AddRange(client.Audiences.Select(p => new Claim(JwtRegisteredClaimNames.Aud, p)));
            return claims;


        }

        public TokenDto CreateToken(UserApp userApp)
        {
            throw new NotImplementedException();
        }

        public ClientTokenDto CreateTokenByClient(Client client)
        {
            throw new NotImplementedException();
        }
    }
}
