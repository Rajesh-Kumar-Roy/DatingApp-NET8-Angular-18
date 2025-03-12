using API.Dtos;
using API.Entites;
using API.Helpers;

namespace API.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike?> GetUserLikes(int sourceId, int targetUserId);
        Task<PagedList<MemberDto>> GetUserLikes(LikesParams likesParams);
        Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentUserId);
        void DeleteLike(UserLike like);
        void AddLike(UserLike like);
        Task<bool> SaveChanges();
    }
}
