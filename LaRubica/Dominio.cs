using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaRubica
{
    public class Dominio
    {
        public string Username { get; set; }

        
        public string Password { get; set; }

        public Dominio(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
