using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model.UserModels
{
    public class AuthenticateModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AuthenticateResponseModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
    }




}
