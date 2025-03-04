﻿using ModelLayer.Model;
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
        public string GetGreeting()
        {
            return "HelloWorld";
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
    }
}
