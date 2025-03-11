using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Middleware.GlobalException;
using ModelLayer.Model;
using RepositoryLayer.Interface;

namespace HelloGreetingApplication.Controllers
{
    [Route("User/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        
        private readonly IUserBL _userBL;
        private readonly ILogger<UserController> _logger;
        private readonly IEmailService _emailService;

        public UserController(ILogger<UserController> logger, IUserBL userBL, IEmailService emailService)
        {
            _logger = logger;
            _userBL = userBL;
            _emailService = emailService;
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

        /// <summary>
        /// Method to Use Forget Password
        /// </summary>
        /// <returns>Token</returns>
        [HttpPost("ForgetPassword")]
        public IActionResult ForgetPassword(ForgetPasswordDTO forgetPasswordDTO)
        {
            ResponseModel<string> responseModel = new ResponseModel<string>();
            try
            {
                var result = _userBL.ForgetPassword(forgetPasswordDTO);

                responseModel.Success = true;
                responseModel.Message = "Check your Email.";
                responseModel.Data = "";

                _emailService.SendEmail(forgetPasswordDTO.Email, "Token for Forget Password", result);
                return Ok(responseModel);
            }
            catch(KeyNotFoundException ex)
            {
                _logger.LogError("Give the correct Email.");
                var errorResponse = ExceptionHandler.HandleException(ex, _logger);
                return Unauthorized(errorResponse);
            }
            catch(ArgumentNullException ex)
            {
                _logger.LogError("Give the correct Email.");
                var errorResponse = ExceptionHandler.HandleException(ex, _logger);
                return Unauthorized(errorResponse);
            }
            catch(Exception ex)
            {
                _logger.LogError("Some Error occurred.");
                var errorResponse = ExceptionHandler.HandleException(ex, _logger);
                return Unauthorized(errorResponse);
            }
        }

        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns>Response body</returns>
        [HttpPost("reset-password")]
        public IActionResult ResetPassword( string token,  string newPassword)
        {
            ResponseModel<string> responseModel = new ResponseModel<string>();
            try
            {
                var result = _userBL.ResetPassword(token, newPassword);

                responseModel.Success = true;
                responseModel.Message = "Password Reset Successfully.";
                responseModel.Data = "";

                return Ok(responseModel);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError("Give the correct Email.");
                var errorResponse = ExceptionHandler.HandleException(ex, _logger);
                return Unauthorized(errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError("Some Error occurred.");
                var errorResponse = ExceptionHandler.HandleException(ex, _logger);
                return Unauthorized(errorResponse);
            }
        }

    }
}
