using Application.DTOs.UserDtos;
using Application.Common;
using MediatR;

public class RegisterUserCommand : IRequest<OperationResult<UserDto>>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Phone { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
}

