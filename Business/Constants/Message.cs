using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
    public class Message
    {
        public static string AlreadyExist => "Record has been already exist";
        public static string NotFound => "Record Not Found!";

        public static string AuthorizationDenied => "You are not authorize!";
        public static string AlreadyRegistered => "You have been already registered with this citizennumber";
        public static string WrongPassword => "Wrong Password!";
        public static string WrongCurrentPassword => "Your Password is wrong right now!";
        public static string PasswordChanged => "Your Password has been changed succesfully!";






    }
}
