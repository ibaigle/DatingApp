using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOS;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
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

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
           // var user = await _userRepository.GetUserByUsernameAsync(username);
            return await _userRepository.GetMemberAsync(username);

        }
    }
}