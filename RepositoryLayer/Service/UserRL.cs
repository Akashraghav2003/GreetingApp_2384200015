using Microsoft.Extensions.Logging;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        private readonly GreetingContext _dbContext;
        private readonly ILogger<UserRL> _logger;

        public UserRL(GreetingContext dbContext, ILogger<UserRL> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public bool RegisterUser(UserDTO userDTO)
        {
            try
            {
                if (userDTO == null)
                {
                    throw new ArgumentNullException("User is not valid");
                }
                var result = _dbContext.UserEntities.FirstOrDefault(e => e.Email == userDTO.Email);

                if (result != null)
                {
                    return false;
                }

                var newUser = new UserEntity
                {
                    UserName = userDTO.UserName,
                    Email = userDTO.Email,
                    Password = userDTO.Password
                };
            
                _dbContext.Add(newUser);
                _dbContext.SaveChanges();

                return true;
            }
            catch(ArgumentNullException ex)
            {
                _logger.LogError("User is Invalid " + ex);
                throw;
            }

        }

        public UserEntity LogIn(LoginDTO loginDTO)
        {
            try
            {
                var result = _dbContext.UserEntities.FirstOrDefault(e => e.Email == loginDTO.Email);

                if(result == null)
                {
                    throw  new KeyNotFoundException("Email is not present");
                }

                return result;
            }catch(KeyNotFoundException ex)
            {
                _logger.LogError("Email is already present.");
                throw;
            }catch(Exception ex)
            {
                _logger.LogError("Some Unexpected error occurred.");
                throw;
            }
        }
    }
}
