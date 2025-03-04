using ModelLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IGreetingBL
    {
        string GetGreeting();
        string GreetMessage(string firstName, string lastName);

        string SavedGreeting(GreetingModel greetingModel);
        GreetingEntity CheckGreeting(CheckGreetingModel checkGreetingModel);
    }
}
