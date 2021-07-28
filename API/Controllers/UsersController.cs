using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOS;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using System.Web.Http.Cors;
namespace API.Controllers
{
    //Deleted both [ApiController] [Route]
    //[EnableCors(origins: "http://localhost:4200", headers: "*", methods:"*")]
    [Authorize]
    public class UsersController : BaseApiController
    {
        //public readonly DataContext _context;
        public readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public readonly IPhotoService _photoService;
        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            _photoService = photoService;
            _mapper = mapper;
            _userRepository = userRepository;
            //_context = context;
        }
        // api/users/3 (3 here is going to be the "id")
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers(int id)
        {
            var users = await _userRepository.GetMembersAsync();

            //var usersToReturn =  _mapper.Map<IEnumerable<MemberDto>>(users);

            return Ok(users);
        }

        [HttpGet("{username}", Name = "GetUser")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            // var user = await _userRepository.GetUserByUsernameAsync(username);
            return await _userRepository.GetMemberAsync(username);

        }
        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            //var username= User.GetUsername();
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            _mapper.Map(memberUpdateDto, user);

            _userRepository.Update(user);

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user");

        }
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            var result = await _photoService.AddPhotoAsync(file);

            if(result.Error!=null){
                return BadRequest(result.Error.Message);
            }

            var photo = new Photo{
                Url= result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if(user.Photos.Count ==0){
                photo.IsMain= true;
            }
            user.Photos.Add(photo);
            if(await _userRepository.SaveAllAsync()){
                /* return _mapper.Map<PhotoDto>(photo); */
                return CreatedAtRoute("GetUser", new{Username = user.Username} ,_mapper.Map<PhotoDto>(photo));
            }
                
            return BadRequest("Problem adding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId){
            var user =await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if(photo.IsMain)return BadRequest("This is already your main photo!");

            var currentMain= user.Photos.FirstOrDefault(x=>x.IsMain);
            if(currentMain != null) currentMain.IsMain=false;
            photo.IsMain = true;
            if (await _userRepository.SaveAllAsync())return NoContent();

            return BadRequest("Failed to set main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId){
            var user= await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo= user.Photos.FirstOrDefault(x=> x.Id ==photoId);
            if(photo ==null){
                return NotFound();
            }
            if(photo.IsMain){
                return BadRequest("You can't delet your main photo");
            }
            if(photo.PublicId!= null){
                var result=await _photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error!=null){
                    return BadRequest(result.Error.Message);
                }
            }
            user.Photos.Remove(photo);

            if(await _userRepository.SaveAllAsync())return Ok();
            return BadRequest("Failed to delete the selected photo");
        }
    }
}