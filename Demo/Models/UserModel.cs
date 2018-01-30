using System;
namespace Demo.Models
{
    public class UserModel
    {
        
            public int ID { set; get; }
            public string User_Name { set; get; }
            public string User_Password { set; get; }
        private bool Sign_In = true;
        public bool User_SignIn_Status { set{ if (value != Sign_In) Sign_In = value; } get { return Sign_In; } }


    }
}
