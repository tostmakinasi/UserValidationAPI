using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserValidationAPI.FileOperations;
using UserValidationAPI.Models;
using UserValidationAPI.Models.AuthenticationManager;
using UserValidationAPI.Repository;

namespace UserValidationAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;

        public UserController(IUserRepository userRepository, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _userRepository = userRepository;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserValidation user)
        {
            //IJwtAuthenticationManager jwtAuthenticationManager;
            var token = _jwtAuthenticationManager.Authentication(user.username, user.password);

            if (token == null)
                return Unauthorized();
            return Ok(token);
        }


        [HttpGet("GetUsers")]//GetAll
        public async Task<ActionResult<User>> GetUsers()
        {
            try
            {
                var userList = await _userRepository.Get();
                if (userList == null)
                    return NotFound();

                return Ok(userList);
            }
            catch (Exception ex)
            {
                var err = BadRequest("İşlem sırasında bir hata alındı.");
                FileOparation.WriteText(err +" && "+ex.Message.ToString());
                return err;
            }
        }

        [HttpGet("GetUserById/{id}")]//GetById
        public async Task<ActionResult<User>> GetUser(int id)
        {
            try
            {
                var userInfo = await _userRepository.Get(id);

                if (userInfo == null)
                    return NotFound("Kullanıcı Bulunamadı.");

                return Ok(userInfo);

            }catch(Exception ex)
            {
                var err = BadRequest("İşlem sırasında bir hata alındı.");
                FileOparation.WriteText(err + " && " + ex.Message.ToString());
                return err;
            }
        }

        [HttpGet("SearchUserByName/{name}")]//Search
        public async Task<ActionResult<User>> GetUsert(string name)
        {
            try
            {
                var searchRes = await _userRepository.Search(name);

                if (searchRes.Equals(null))
                    return NotFound();

                return Ok(searchRes);
            }
            catch (Exception ex)
            {
                var err = BadRequest("İşlem sırasında bir hata alındı.");
                FileOparation.WriteText(err + " && " + ex.Message.ToString());
                return err;
            }
        }

        [HttpPost("CreateUser")]//CreateUser
        public async Task<ActionResult<User>> PostUsers([FromBody] User user)
        {
            try
            {
                var newUser = await _userRepository.Create(user);
                return CreatedAtAction(nameof(GetUsers), new { id = newUser.Id }, newUser);
            }
            catch (Exception ex)
            {
                var err = BadRequest("İşlem sırasında bir hata alındı.");
                FileOparation.WriteText(err + " && " + ex.Message.ToString());
                return err;
            }
        }

        [HttpPut]
        public async Task<ActionResult<User>> PutUser(int? id, [FromBody] User user)
        {
            try
            {
                if (id == null)
                    return BadRequest();

                 await _userRepository.Update(user);

                return Ok();

            }
            catch (Exception ex)
            {
                var err = BadRequest("İşlem sırasında bir hata alındı.");
                FileOparation.WriteText(err + " && " + ex.Message.ToString());
                return err;
            }
        }

        [HttpDelete("DeleteUserById/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var userDelete = await _userRepository.Get(id);
                if (userDelete == null)
                    return NotFound();

                await _userRepository.Delete(userDelete.Id);
                return NoContent();
            }
            catch (Exception ex)
            {
                var err = BadRequest("İşlem sırasında bir hata alındı.");
                FileOparation.WriteText(err + " && " + ex.Message.ToString());
                return err;
            }
        }
    }
}
