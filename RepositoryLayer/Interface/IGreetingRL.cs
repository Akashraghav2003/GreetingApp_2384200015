using RepositoryLayer.Entity;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;

namespace RepositoryLayer.Interface
{
    public interface IGreetingRL
    {
        string GetGreeting();

        string SavedGreeting(GreetingModel greetingModel);

        GreetingEntity CheckGreeting(CheckGreetingModel checkGreetingModel);

        List<GreetingEntity> ListGreetingMessage();

<<<<<<< HEAD
        string UpdateGreeting(GreetingIdModel updateGreetingModel);

        
=======
        string UpdateGreeting(UpdateGreetingModel updateGreetingModel);
        string DeleteGreeting(CheckGreetingModel checkGreetingModel);

>>>>>>> UC8
    }
}
