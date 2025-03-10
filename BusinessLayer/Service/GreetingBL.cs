using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Interface;
using BusinessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Service
{
    public class GreetingBL : IGreetingBL
    {

        private readonly IGreetingRL _greetingRL;
        private readonly ILogger<GreetingBL> _logger;

        public GreetingBL(IGreetingRL greetingRL, ILogger<GreetingBL> logger)
        {
            _greetingRL = greetingRL;
            _logger = logger;
        }

        public string GreetMessage(string firstName, string lastName)
        {
            try
            {
                if (firstName == null && lastName != null)
                {
                    return $"Hello, Mr./Ms. {lastName}.";
                }
                else if (firstName != null && lastName == null)
                {
                    return $"Hello, {firstName}";
                }
                else if (firstName == null && lastName == null)
                {
                    return "Hello, World";
                }
                return $"Hello, {firstName} {lastName}";
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Greeting already presents.");

                throw ;
            }
        }

        public string GetGreeting()
        {
           return  _greetingRL.GetGreeting();
        }

        public string SavedGreeting(GreetingModel greetingModel)
        {
            try
            {
                var result = _greetingRL.SavedGreeting(greetingModel);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greeting already presents.");
                throw;
            }
        }

        public GreetingEntity CheckGreeting(CheckGreetingModel checkGreetingModel)
        {
            try
            {
                var result = _greetingRL.CheckGreeting(checkGreetingModel);

                return result;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Greeting is not found");
                throw;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Greeting is not found");
                throw;
            }
        }

        public List<GreetingEntity> ListGreetingMessage()
        {
            try
            {
                var result = _greetingRL.ListGreetingMessage();
                return result;
            }
            catch
            {
                throw new Exception();
            }
             
        }

        public string UpdateGreeting(GreetingIdModel updateGreetingModel)
        {
            try
            {
                var result = _greetingRL.UpdateGreeting(updateGreetingModel);
                return result;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Greeting is not found");
                throw;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Invalid Greeting Message");
                throw;
            }
        }

        public string DeleteGreeting(CheckGreetingModel checkGreetingModel)
        {
            try
            {
                var result = _greetingRL.DeleteGreeting(checkGreetingModel);
                return result;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Greeting is not found");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Invalid Greeting Message");
                throw;
            }
        }

    }
}
