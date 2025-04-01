using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class User
    {
        private int id;
        private string username;
        private string password;
        private int groupNumber;
        private string uniqueCode;

        public int ID
        {
            get => id;
            set => id = value;
        }

        public string Username
        {
            get => username;
            set => username = value;
        }

        public string Password
        {
            get => password;
            set => password = value;
        }

        public int GroupNumber
        {
            get => groupNumber;
            set => groupNumber = value;
        }

        public string UniqueCode
        {
            get => uniqueCode;
            set => uniqueCode = value;
        }
    }
}
