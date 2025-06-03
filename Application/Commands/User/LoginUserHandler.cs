using Application.Common;
using Application.DTOs.UserDtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using MediatR;

namespace Application.Commands.User
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, OperationResult<LoginResponseDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public LoginUserHandler(IUserRepository userRepository, IMapper mapper, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<OperationResult<LoginResponseDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return OperationResult<LoginResponseDto>.Failure("Invalid credentials.");
            }

            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _userRepository.UpdateAsync(user.UserId, user);
            var token = _tokenService.CreateToken(user);

            var response = new LoginResponseDto
            {
                Token = token,
                Expiry = DateTime.UtcNow.AddMinutes(60),
                User = _mapper.Map<UserDto>(user)
            };

            return OperationResult<LoginResponseDto>.SuccessResult(response);
        }

        private string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            System.Security.Cryptography.RandomNumberGenerator.Fill(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}

