using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CST350Milestone5.Models
{
    public class PlayerLoginModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public PlayerLoginModel(int id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
        }
    }
}
