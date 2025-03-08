using API.Dtos;
using API.Entites;

namespace API.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike?> GetUserLikes(int sourceId, int targetUserId);
        Task<IEnumerable<MemberDto>> GetUserLikes(string predicate, int userId);
        Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentUserId);
        void DeleteLike(UserLike like);
        void AddLike(UserLike like);
        Task<bool> SaveChanges();
    }
}
