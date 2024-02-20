using BAL.Models;
using BAL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserRegistrationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }
        //public UserController(IUserService userService)
        //{
        //    _userService = userService;
        //}

        //[HttpPost]
        //[Route("UserRegistration")]
        //public IActionResult RegisterUser(UserModel user)
        //{
        //    return Ok(_userService.RegisterUser(user));
        //}
        [HttpPost]
        [Route("UserRegistration")]
        public async Task<IActionResult> RegisterUser(UserModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _userService.RegisterUser(user);

                if (result)
                    return Ok(new { success = true, message = "User registration successful" });

                _logger.LogError("User registration failed");
                return BadRequest(new { success = false, error = "User registration failed" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred during user registration: {ex}");
                return StatusCode(500, new { success = false, error = "An unexpected error occurred" });
            }
        }




        //[HttpGet]
        //[Route("GetAllUser")]
        //public IActionResult GetUsers()
        //{
        //    return Ok(_userService.GetUsers());
        //}

        [HttpGet]
[Route("GetAllUser")]
public async Task<ActionResult<List<UserModel>>> GetUsers()
{
    try
    {
        var users = await _userService.GetUsers();

        if (users != null)
        {
            return Ok(users);
        }
        else
        {
            return NotFound("No users found");
        }
    }
    catch (Exception ex)
    {
        _logger.LogError($"An unexpected error occurred while fetching users: {ex}");
        return StatusCode(500, "An unexpected error occurred");
    }
}


        //[HttpDelete("DeleteUser/{id}")]
        //[Route("DeleteUser")]
        //public IActionResult DeleteUser(int id)
        //{
        //    return Ok(_userService.DeleteUser(id));
        //}


        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _userService.DeleteUser(id);

                if (result)
                {
                    return Ok(new { success = true, message = "User deleted successfully" });
                }
                else
                {
                    return BadRequest(new { success = false, error = "User ID not found" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred during user deletion: {ex}");
                return StatusCode(500, new { success = false, error = "An unexpected error occurred" });
            }
        }


        //[HttpPut]
        //[Route("UpdateAllUser")]
        //public IActionResult UpdateUser(UserModel user)
        //{
        //    return Ok(_userService.UpdateUser(user));
        //}

        //[HttpPost]
        //[Route("LoginUser")]
        //public IActionResult LoginUser(string mobile, string password)
        //{
        //    return Ok(_userService.LoginUser(mobile, password));
        //}

        [HttpPost]
        [Route("LoginUser")]
        public async Task<IActionResult> LoginUser(UserLoginModel userModel)
        {
            try
            {
                bool result = await _userService.LoginUser(userModel);
                if (result)
                {
                    return Ok(new { success = true, message = "Login successful" });
                }


                else
                {
                    // User not found, login failed
                    return Ok(new { success = false, message = "Invalid credentials" });
                }
            }
            catch (Exception ex) { 
            throw ex;
                    }
        
            
}





[HttpPut]
        [Route("UpdateAllUser")]
        public async Task<IActionResult> UpdateUser(UserModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                bool updateResult = await _userService.UpdateUser(user);

                if (updateResult)
                {
                    return Ok(new { success = true, message = "User updated successfully" });
                }
                else
                {
                    _logger.LogError("User update failed");
                    return BadRequest(new { success = false, error = "User update failed" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred during user update: {ex}");
                return StatusCode(500, new { success = false, error = "An unexpected error occurred" });
            }
        }


    }

}