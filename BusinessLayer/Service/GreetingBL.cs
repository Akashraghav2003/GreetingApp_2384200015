using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Interface;
using BusinessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace BusinessLayer.Service
{
    public class GreetingBL : IGreetingBL
    {

        private readonly IGreetingRL _greetingRL;

        public GreetingBL(IGreetingRL greetingRL)
        {
            _greetingRL = greetingRL;
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
            catch
            {
                throw new Exception();
            }
        }

        public string GetGreeting()
        {
           return  _greetingRL.GetGreeting();
        }

        public string SavedGreeting(GreetingModel greetingModel)
        {
                var result = _greetingRL.SavedGreeting(greetingModel);
                return result; 
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
                throw;
            }
            catch
            {
                throw new Exception();
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
            catch(KeyNotFoundException ex)
            {
                throw;
            }
            catch
            {
                throw new Exception();
            }
        }
<<<<<<< HEAD

        
=======
        public string DeleteGreeting(CheckGreetingModel checkGreetingModel)
        {
            try
            {
                var result = _greetingRL.DeleteGreeting(checkGreetingModel);
                return result;
            }
            catch(KeyNotFoundException ex)
            {
                throw;
            }
            catch
            {
                throw new Exception();
            }
        }
>>>>>>> UC8
    }
}
