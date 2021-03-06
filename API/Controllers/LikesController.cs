using System.Threading.Tasks;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Extensions;
using API.Entities;
using System.Collections.Generic;
using API.DTOS;
using API.Helpers;

namespace API.Controllers
{
    [Authorize]
    public class LikesController : BaseApiController
    {
        /* public readonly IUserRepository _userRepository ; 
        public readonly ILikesRepository _likesRepository ;  */
        public readonly IUnitOfWork _unitOfWork ; 
        public LikesController(IUnitOfWork unitOfWork/* IUserRepository userRepository, ILikesRepository likesRepository */)
        {
            /* _likesRepository = likesRepository;
            _userRepository = userRepository; */
            _unitOfWork = unitOfWork;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username){
            var sourceUserId= User.GetUserId();
            var likedUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            var sourceUser = await _unitOfWork.LikesRepository.GetUserWithLikes(sourceUserId);
            if(likedUser==null) return NotFound();
            if(sourceUser.UserName == username) return BadRequest("You cannot like yourself");
            var userLike = await _unitOfWork.LikesRepository.GetUserLike(sourceUserId,likedUser.Id);
            if(userLike!=null) return BadRequest("You alreeady liked this user");

            userLike = new UserLike{
                SourceUserId = sourceUserId,
                LikedUserId = likedUser.Id
            };
            if(await _unitOfWork.Complete())return Ok();

            return BadRequest("Failed to like the user");
            
        }
        [HttpGet] //the predicate is from the query string
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery]LikesParams likesParams){
            likesParams.UserId = User.GetUserId();
            var users = await _unitOfWork.LikesRepository.GetUserLikes(likesParams);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, 
                        users.TotalCount, users.TotalPages);
            return Ok(users);
        }
    }
}