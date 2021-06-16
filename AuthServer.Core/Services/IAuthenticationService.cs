using AuthServer.Core.Dto;
using SharedLibrary.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface IAuthenticationService
    {
        Task<Response<TokenDto>> CreateToken(LoginDto loginDto);

        Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken);

        Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToke);

        Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto);
    }
}