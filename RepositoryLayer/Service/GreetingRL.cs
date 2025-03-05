using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RepositoryLayer.Service
{
    public class GreetingRL : IGreetingRL
    {
        private readonly GreetingContext _dbContext;

        public GreetingRL(GreetingContext dbContext)
        {
            _dbContext = dbContext;
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
                throw;
            }
            catch
            {
                throw new Exception();
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
            catch
            {
                throw new Exception();
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
                    return "Greeting is already exits.";
                }

                var newGreeting = new GreetingEntity
                {
                    greetingMessage = greetingModel.greetingMessage
                };
                _dbContext.Add(newGreeting);
                _dbContext.SaveChanges();

                return "New message added to database.";
            }
            catch 
            {
                throw new Exception();
            }
        }

        public string UpdateGreeting(UpdateGreetingModel updateGreetingModel)
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
                throw; 
                
            }
            catch(Exception ex)
            {
                throw new Exception();
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
                throw;
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
