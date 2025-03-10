using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;



namespace RepositoryLayer.Service
{
    public class GreetingRL : IGreetingRL
    {
        private readonly GreetingContext _dbContext;
        private readonly ILogger<GreetingRL> _logger;

        public GreetingRL(GreetingContext dbContext, ILogger<GreetingRL> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public GreetingEntity CheckGreeting(CheckGreetingModel checkGreetingModel)
        {
            if (checkGreetingModel == null)
            {
                throw new ArgumentException("Invalid Greeting Message");
            }

            try
            {
                var result = _dbContext.GreetingEntities.FirstOrDefault<GreetingEntity>(e => e.id == checkGreetingModel.id);

                if (result == null)
                {
                    throw new KeyNotFoundException($"Greeting with Id {checkGreetingModel.id} not found.");
                }

                return result;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Greeting is not found");
                throw;
            }
            catch(ArgumentException ex)
            {
                _logger.LogError(ex, "Greeting is not found");
                throw;
            }
        }

        
        

        public string GetGreeting()
        {
            return "HelloWorld";
        }

        public List<GreetingEntity> ListGreetingMessage()
        {
            try
            {
                var result = _dbContext.GreetingEntities.ToList();
                if (result == null)
                {
                    throw new Exception("There is not Greeting Present.");
                }

                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "There is not Greeting Present.");
                throw;
            }


        }

        public string SavedGreeting(GreetingModel greetingModel)
        {

            if (greetingModel == null || string.IsNullOrWhiteSpace(greetingModel.greetingMessage))
            {
                throw new ArgumentException("Invalid Greeting Message");
            }
            try
            {
                var result = _dbContext.GreetingEntities.FirstOrDefault<GreetingEntity>(e => e.greetingMessage == greetingModel.greetingMessage);

                if (result != null)
                {
                    throw new Exception("Greeting already presents.");
                }

                var newGreeting = new GreetingEntity
                {
                    greetingMessage = greetingModel.greetingMessage
                };
                _dbContext.Add(newGreeting);
                _dbContext.SaveChanges();

                return "New message added to database.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greeting already presents.");
                throw;
            }
        }

        public string UpdateGreeting(GreetingIdModel updateGreetingModel)
        {
            if (updateGreetingModel == null || string.IsNullOrWhiteSpace(updateGreetingModel.greetingMessage))
            {
                throw new ArgumentException("Invalid Greeting Message");
            }
            try
            {
                var result = _dbContext.GreetingEntities.FirstOrDefault<GreetingEntity>(e => e.id == updateGreetingModel.id);

                if(result == null)
                {
                    throw new KeyNotFoundException($"ID {updateGreetingModel.id} not found.");
                }

                result.greetingMessage = updateGreetingModel.greetingMessage;

                _dbContext.SaveChanges();
                return "Greeting Message Updaet succesfull.";
            }
            catch(KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Greeting is not found");
                throw;

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Invalid Greeting Message");
                throw;
            }
        }
        public string DeleteGreeting(CheckGreetingModel checkGreetingModel)
        {
            if (checkGreetingModel == null)
            {
                throw new ArgumentException("Invalid Greeting Message");
            }

            try
            {
                var result = _dbContext.GreetingEntities.FirstOrDefault<GreetingEntity>(e => e.id == checkGreetingModel.id);

                if (result == null)
                {
                    throw new KeyNotFoundException($"Greeting with ID {checkGreetingModel.id} not found.");
                }

                _dbContext.GreetingEntities.Remove(result);
                _dbContext.SaveChanges();

                return "Greeting Delete successfull.";
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Greeting is not found");
                throw;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Invalid Greeting Message");
                throw;
            }
        }
    }
}
