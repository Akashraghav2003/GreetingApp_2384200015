﻿using ModelLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        bool RegisterUser(UserDTO userDTO);

        bool LogIn(LoginDTO loginDTO);
    }
}
