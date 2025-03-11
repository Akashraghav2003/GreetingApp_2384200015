using ModelLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {
        bool RegisterUser(UserDTO userDTO);

        UserEntity LogIn(LoginDTO loginDTO);
        UserEntity ForgetPassword(ForgetPasswordDTO forgetPasswordDTO);

        UserEntity ResetPassword(String Email, string Password);
    }
}
