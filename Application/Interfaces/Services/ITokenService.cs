using Domain.Models;

namespace Application.Interfaces.Services
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
