using Application.DTOs.UserDtos;
using Application.Interfaces.Repositories;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Models;

namespace Application.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public UserService(IUserRepository userRepository, IMapper mapper, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<IEnumerable<UserDto>> GetFilteredUsersAsync(string? search, string? sortBy, string? sortOrder, int page = 1, int pageSize = 10)
        {
            var users = await _userRepository.GetFilteredUsersAsync(search, sortBy, sortOrder, page, pageSize);
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> RegisterUserAsync(UserCreateDto userRegistrationData)
        {
            var newUser = _mapper.Map<User>(userRegistrationData);
            newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userRegistrationData.Password);

            var createdUser = await _userRepository.CreateAsync(newUser);
            return _mapper.Map<UserDto>(createdUser);
        }

        public async Task<LoginResponseDto?> LoginUserAsync(LoginDto loginData)
        {
            var user = await _userRepository.GetByEmailAsync(loginData.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginData.Password, user.PasswordHash))
                return null;

            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _userRepository.UpdateAsync(user.UserId, user);

            return new LoginResponseDto
            {
                Token = _tokenService.CreateToken(user),
                Expiry = DateTime.UtcNow.AddMinutes(60),
                User = _mapper.Map<UserDto>(user)
            };
        }

        public async Task<UserDto?> UpdateUserAsync(int userId, UserUpdateDto updatedData)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.IsDeleted) return null;

            if (!string.IsNullOrWhiteSpace(updatedData.FirstName))
                user.FirstName = updatedData.FirstName;

            if (!string.IsNullOrWhiteSpace(updatedData.LastName))
                user.LastName = updatedData.LastName;

            if (!string.IsNullOrWhiteSpace(updatedData.Phone))
                user.Phone = updatedData.Phone;

            if (!string.IsNullOrWhiteSpace(updatedData.EmergencyContactName))
                user.EmergencyContactName = updatedData.EmergencyContactName;

            if (!string.IsNullOrWhiteSpace(updatedData.EmergencyContactPhone))
                user.EmergencyContactPhone = updatedData.EmergencyContactPhone;

            user.AccountModificationTime = DateTime.UtcNow;

            var updated = await _userRepository.UpdateAsync(userId, user);
            return updated != null ? _mapper.Map<UserDto>(updated) : null;
        }

        public async Task<bool> SoftDeleteUserAsync(int userId)
        {
            return await _userRepository.SoftDeleteAsync(userId);
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            System.Security.Cryptography.RandomNumberGenerator.Fill(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
