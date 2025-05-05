using Domain.Models;

namespace Application.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
