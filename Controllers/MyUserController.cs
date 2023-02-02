using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using ToDoList.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ToDoList.Services;

namespace ToDoList.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MyUserController : ControllerBase
    {
        IUserService UserService;
        public MyUserController(IUserService UserService)
        {
            this.UserService = UserService;
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<String> Login([FromBody] MyUser user)
        {
            List<MyUser> users = UserService.GetAll();
            MyUser myUser = users.FirstOrDefault(u => u.Name.Equals(user.Name) && u.Password.Equals(user.Password));
            if (myUser == null)
                return Unauthorized();
            var claims = new List<Claim>();
            if (myUser.IsAdmin)
                claims.Add(new Claim("type", "Admin"));
            else
                claims.Add(new Claim("type", "User"));
            claims.Add(new Claim("userid", myUser.Id.ToString()));
            var token = TokenService.GetToken(claims);
            return new OkObjectResult(TokenService.WriteToken(token));
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize(Policy = "Admin")]
        public ActionResult<List<MyUser>> GetAll() =>
            UserService.GetAll();

        [HttpGet("{id}")]
        [Authorize(Policy = "Admin")]
        public ActionResult<MyUser> Get(int id)
        {
            var user = UserService.Get(id);
            if (user == null)
                return NotFound();
            return user;
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Create(MyUser user)
        {
            UserService.Add(user);
            return CreatedAtAction(nameof(Create), new { id = user.Id }, user);
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public IActionResult Delete(int id)
        {
            var user = UserService.Get(id);
            if (user is null)
                return NotFound();
            if (user.IsAdmin)
                return Forbid();
            UserService.Delete(id);

            return Content(UserService.Count.ToString());
        }
    }
}