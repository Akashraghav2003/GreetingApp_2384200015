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
    }
}
