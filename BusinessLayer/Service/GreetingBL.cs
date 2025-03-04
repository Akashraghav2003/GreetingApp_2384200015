using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Interface;
using BusinessLayer.Interface;
using ModelLayer.Model;

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
    }
}
