using API.Data;
using API.Dtos;
using API.Entites;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class LikesController(ILikesRepository likesRepository) : BaseApiController
    {
        [HttpPost("{targetUserId:int}")]
        public async Task<ActionResult> ToggleLike(int targetUserId)
        {
            var sourceUserId = User.GetUserId();

            if (sourceUserId == targetUserId) return BadRequest("You cannot like yourself");

            var existingLike = await likesRepository.GetUserLikes(sourceUserId, targetUserId);

            if(existingLike == null)
            {
                var like = new UserLike
                {
                    SourceUserId = sourceUserId,
                    TargetUserId = targetUserId
                };

                likesRepository.AddLike(like);
            }
            else
            {
                likesRepository.DeleteLike(existingLike);
            }

            if (await likesRepository.SaveChanges()) return Ok();
            return BadRequest("Failed to update like");
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<int>>> GetCurrentUserLikeIds()
        {
            return Ok(await likesRepository.GetCurrentUserLikeIds(User.GetUserId()));

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUserLikes(string predicate)
        {
            var users = await likesRepository.GetUserLikes(predicate, User.GetUserId());
            return Ok(users);
        }
    }
}
