using API.Entites;

namespace API.Interfaces
{
    public interface ITokenService
    {
        string CrateToken(AppUser user);
    }
}
