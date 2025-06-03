using Application.Common;
using Application.DTOs.UserDtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Models;
using MediatR;

namespace Application.Commands.User
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, OperationResult<LoginResponseDto>>
    {
        private readonly IUserRepository _userRepo;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public RefreshTokenHandler(IUserRepository userRepo, ITokenService tokenService, IMapper mapper)
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<OperationResult<LoginResponseDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByEmailAsync(request.Email);

            if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                return OperationResult<LoginResponseDto>.Failure("Invalid or expired refresh token.");
            }

            // Generate new tokens
            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _userRepo.UpdateAsync(user.UserId, user);

            var accessToken = _tokenService.CreateToken(user);

            return OperationResult<LoginResponseDto>.SuccessResult(new LoginResponseDto
            {
                Token = accessToken,
                Expiry = DateTime.UtcNow.AddMinutes(60),
                User = _mapper.Map<UserDto>(user)
            });
        }

        private string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            System.Security.Cryptography.RandomNumberGenerator.Fill(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
