using AuthServer.Core.Configuration;
using AuthServer.Core.Dto;
using AuthServer.Core.Models;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedLibrary.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;
        private readonly UserManager<UserApp> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserRefreshToken> _userRefreshTokenRepository;

        public AuthenticationService(
            IGenericRepository<UserRefreshToken> userRefreshTokenRepository,
            IOptions<List<Client>> optionsClient,
            ITokenService tokenService,
            UserManager<UserApp> userManager,
            IUnitOfWork unitOfWork
            )
        {
            _userRefreshTokenRepository = userRefreshTokenRepository;
            _clients = optionsClient.Value;
            _tokenService = tokenService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<TokenDto>> CreateToken(LoginDto loginDto)
        {
            //Savunmacı kod yaklaşımı
            if (loginDto == null) throw new ArgumentNullException(nameof(loginDto));
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            //Savunmacı kod yaklaşımı
            if (user == null) return Response<TokenDto>.Fail("Email or Password is wrong", 400, true);
            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password)) return Response<TokenDto>.Fail("Email or Password is wrong", 400, true);
            var token = _tokenService.CreateToken(user);
            var userRefreshToken = await _userRefreshTokenRepository.Where(p => p.UserId == user.Id).SingleOrDefaultAsync();
            // Token yoksa sıfırdan oluştur.
            if (userRefreshToken == null) await _userRefreshTokenRepository.AddAsync(new UserRefreshToken
            {
                UserId = user.Id,
                Code = token.RefreshToken,
                Expiration = token.RefreshTokenExpression
            });
            else
            {
                userRefreshToken.Code = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpression;

            }
            await _unitOfWork.CommitAsync();
            return Response<TokenDto>.Success(token, 200);
        }

        public Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var client = _clients.SingleOrDefault(p => p.Id == clientLoginDto.ClientId && p.Secret == clientLoginDto.ClientSecret);
            if (client == null) return Response<ClientTokenDto>.Fail("ClientId or Secret not found", 404, true);
            var token = _tokenService.CreateTokenByClient(client);
            return Response<ClientTokenDto>.Success(token, 200);
        }

        public async Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
        {
            var exitsRefreshToken = await _userRefreshTokenRepository.Where(p => p.Code == refreshToken).SingleOrDefaultAsync();
            if (exitsRefreshToken == null) return Response<TokenDto>.Fail("Refresh Token not found", 404, true);
            var user = await _userManager.FindByIdAsync(exitsRefreshToken.UserId);
            if (user == null) return Response<TokenDto>.Fail("UserId not found", 404, true);
            var tokenDto = _tokenService.CreateToken(user);
            exitsRefreshToken.Code = tokenDto.RefreshToken;
            exitsRefreshToken.Expiration = tokenDto.RefreshTokenExpression;
            await _unitOfWork.CommitAsync();
            return Response<TokenDto>.Success(tokenDto, 200);

        }

        public async Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToke)
        {
            var exitsRefreshToken =await _userRefreshTokenRepository.Where(p => p.Code == refreshToke).SingleOrDefaultAsync();
            if (exitsRefreshToken == null) Response<NoDataDto>.Fail("Refresh token not found", 404, true);

            _userRefreshTokenRepository.Remove(exitsRefreshToken);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(200);


        }
    }
}
