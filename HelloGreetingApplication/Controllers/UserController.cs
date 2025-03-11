using BusinessLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using Middleware.GlobalException;
using Microsoft.AspNetCore.Authorization;

namespace HelloGreetingApplication.Controllers
{
    [Route("User/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        
        private readonly IUserBL _userBL;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, IUserBL userBL)
        {
            _logger = logger;
            _userBL = userBL;
        }


        /// <summary>
        /// Register the user
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        [HttpPost("User_Register")]
        [Authorize]

        public IActionResult RegisterUser(UserDTO userDTO)
        {
            ResponseModel<string> responseModel = new ResponseModel<string>();

            try
            {
                var result = _userBL.RegisterUser(userDTO);
                if (result == true)
                {
                    responseModel.Success = true;
                    responseModel.Message = "User added successfully.";
                    responseModel.Data = $"{userDTO.UserName} added successfully";

                    return Ok(responseModel);
                }
                else
                {
                    responseModel.Success = false;
                    responseModel.Message = "User do not add in database.";
                    responseModel.Data = $"{userDTO.Email}  Email already present.";

                    return Unauthorized(responseModel);
                }
            }
            catch(ArgumentNullException ex)
            {
                _logger.LogError("Give the valid data" + ex);
                var errorResponse = ExceptionHandler.HandleException(ex, _logger);
                return BadRequest(errorResponse);
            }
        }

        /// <summary>
        /// Login of the user.
        /// </summary>
        /// <param name="loginDTO"></param>
        /// <returns></returns>

        [HttpPost("LoginUser")]

        public IActionResult Login(LoginDTO loginDTO)
        {
            ResponseModel<string> responseModel = new ResponseModel<string>();
            try
            {
                var result = _userBL.LogIn(loginDTO);

              
                    responseModel.Success = true;
                    responseModel.Message = "Login Successfully.";
                    responseModel.Data = result;

                    return Ok(responseModel);
            
               
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError("Check the email and password");
                var errorResponse = ExceptionHandler.HandleException(ex, _logger);
                return BadRequest(errorResponse);
            }
            catch(ArgumentNullException ex)
            {
                _logger.LogError("Check the email and password");
                var errorResponse = ExceptionHandler.HandleException(ex, _logger);
                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError("Check the email and password");
                var errorResponse = ExceptionHandler.HandleException(ex, _logger);
                return BadRequest(errorResponse);
            }
        }

        [HttpPost("ForgetPassword")]
        public IActionResult ForgetPassword()
        {
            return Ok();
        }

        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword()
        {
            return Ok();
        }
    }
}
