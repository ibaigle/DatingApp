using System;
using System.Web.Http;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _context;
        public BuggyController(DataContext context){
            _context = context;
        }
        [Authorize]
        [Microsoft.AspNetCore.Mvc.HttpGet("auth")]
        public ActionResult<string> GetSecret(){
            return "secret text";
        }
        [Microsoft.AspNetCore.Mvc.HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound(){
            var thing= _context.Users.Find(-1);

            if(thing==null) return NotFound();
            return Ok(thing);
        }
        [Microsoft.AspNetCore.Mvc.HttpGet("server-error")]
        public ActionResult<string> GetServerError(){
            //we comment try-catch block because its not 1990
            //try{
                    var thing= _context.Users.Find(-1);

                    var thingToReturn = thing.ToString();

                    return thingToReturn;
            /*}catch(Exception ex){
                System.Console.Out.WriteLine(ex);
                return StatusCode(500, "Computer says NO!!!");
            }*/
            
        }
        [Microsoft.AspNetCore.Mvc.HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest(){
            return BadRequest("This was not a good request");
        }
        
    }
}