using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiKel38.Data;
using ApiKel38.Models.Dto;
using System.Collections.Generic;
using System.Linq;

namespace ApiKel38.Controllers
{
    [Route("api/UserAPI")]
    [ApiController]
    public class UserAPIController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<UserDTO>> GetUsers()
        {
            return Ok(UserStore.userList);
        }

        [HttpGet("{id:int}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserDTO> GetUser(int id)
        {
            if (id <= 0)
                return BadRequest();

            var user = UserStore.userList.FirstOrDefault(u => u.Id == id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public ActionResult<UserDTO> CreateAccount([FromBody] UserDTO userDTO)
        {
            if (userDTO == null)
                return BadRequest();

            if (UserStore.userList.Any(u => u.Username.ToLower() == userDTO.Username.ToLower()))
            {
                ModelState.AddModelError("CustomError", "Account already exists");
                return BadRequest(ModelState);
            }

            if (userDTO.Id != 0)
                return StatusCode(StatusCodes.Status500InternalServerError);

            userDTO.Id = UserStore.userList.Any() ? UserStore.userList.Max(u => u.Id) + 1 : 1;
            UserStore.userList.Add(userDTO);

            string response = $"Successfully added account data\nName: {userDTO.Username}";
            return CreatedAtRoute("GetUser", new { id = userDTO.Id }, response);
        }

        [HttpDelete("{id:int}", Name = "DeleteUser")]
        public IActionResult DeleteUser(int id)
        {
            if (id <= 0)
                return BadRequest();

            var user = UserStore.userList.FirstOrDefault(u => u.Id == id);

            if (user == null)
                return NotFound();

            UserStore.userList.Remove(user);
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateUser")]
        public IActionResult UpdateUser(int id, [FromBody] UserDTO userDTO)
        {
            if (userDTO == null || id != userDTO.Id)
                return BadRequest();

            var user = UserStore.userList.FirstOrDefault(u => u.Id == id);

            if (user == null)
                return NotFound();

            user.Username = userDTO.Username;
            user.Password = userDTO.Password;

            return NoContent();
        }

        [HttpPost("login")]
        public ActionResult<string> Login([FromBody] UserDTO loginDTO)
        {
            if (loginDTO == null)
                return BadRequest();

            var user = UserStore.userList.FirstOrDefault(u =>
                u.Username.ToLower() == loginDTO.Username.ToLower() &&
                u.Password == loginDTO.Password);

            if (user == null)
                return NotFound("Invalid username or password");

            return Ok("Login successful");
        }
    }
}
