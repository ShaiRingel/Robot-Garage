namespace Model
{
    public class User : BaseEntity
    {
        private string username;
        private string password;
        private int groupNumber;
        private string uniqueCode;

        public User()
        {

        }

        public User(string username, string password, int groupNumber, string uniqueCode)
        {
            this.username = username;
            this.password = password;
            this.groupNumber = groupNumber;
            this.uniqueCode = uniqueCode;
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
