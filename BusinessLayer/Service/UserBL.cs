using BusinessLayer.Interface;
using Microsoft.Extensions.Logging;
using ModelLayer.Model;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL _userRL;
        private readonly ILogger<UserBL> _logger;
        private readonly ITokenService _tokenService;

        public UserBL(IUserRL userRL, ILogger<UserBL> logger, ITokenService tokenService)
        {
            _userRL = userRL;
            _logger = logger;
            _tokenService = tokenService;
        }

        public string LogIn(LoginDTO loginDTO)
        {
            if(loginDTO == null)
            {
                throw new ArgumentNullException("Give valid Email.");
            }
            try
            {
                var result = _userRL.LogIn(loginDTO);

                if (PasswordHashing.VerifyPassword(loginDTO.Password, result.Password)) 
                {
                    var token = _tokenService.GenerateToken(result);
                    return token;
                }
                else
                {
                    throw new KeyNotFoundException("Give correct password.");
                }
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError("Email is already present. "+ex);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Some Unexpected error occurred. "+ex);
                throw;
            }
        }

        public bool RegisterUser(UserDTO userDTO)
        {
            try
            {
                var userPasswordHasing = new UserDTO
                {
                    Email = userDTO.Email,
                    UserName = userDTO.UserName,
                    Password = PasswordHashing.Hashing(userDTO.Password)
                };

                var result = _userRL.RegisterUser(userPasswordHasing);

                return result;
            }
            catch(ArgumentNullException ex)
            {
                _logger.LogError("Invalid user " + ex);
                throw;
            }
        }

        public string ForgetPassword(ForgetPasswordDTO forgetPasswordDTO)
        {
            try
            {
                if(forgetPasswordDTO == null)
                {
                    throw new ArgumentNullException("Give the correct Email.");
                }

                var result = _userRL.ForgetPassword(forgetPasswordDTO);

                var token = _tokenService.GenerateToken(result);

                return token;
            }catch(KeyNotFoundException ex)
            {
                _logger.LogError("Email not found." + ex);
                throw;
            }catch(ArgumentNullException ex)
            {
                _logger.LogError("Give correct Email " + ex);
                throw;
            }
        }

        public bool ResetPassword(string token, string Password)
        {
            try
            {
                var email = _tokenService.ValidateToken(token);
                var newPassword = PasswordHashing.Hashing(Password);
                var result = _userRL.ResetPassword(email, newPassword);

                return true;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError("Invalid Email.");
                throw;
            }
            catch(Exception ex)
            {
                _logger.LogError("Some Error Occurred.");
                throw;
            }
        }
    }
}
