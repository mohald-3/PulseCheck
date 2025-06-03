using Application.Common;
using Application.Interfaces.Repositories;
using Application.DTOs.UserDtos;
using AutoMapper;
using Domain.Models;
using MediatR;
using System.Numerics;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, OperationResult<UserDto>>
{
    private readonly IUserRepository _repo;
    private readonly IMapper _mapper;

    public RegisterUserHandler(IUserRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<OperationResult<UserDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // Optional: add email exists check, etc.
        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Phone = request.Phone,
            EmergencyContactName = request.EmergencyContactName,
            EmergencyContactPhone = request.EmergencyContactPhone
        };

        await _repo.CreateAsync(user);

        var dto = _mapper.Map<UserDto>(user);
        return OperationResult<UserDto>.SuccessResult(dto, "User registered successfully");
    }
}


