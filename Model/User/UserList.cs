using System.Diagnostics;

namespace Model
{
    public class UserList : List<User>
    {
        public UserList() { }
        public UserList(IEnumerable<User> list) : base(list) { }
        public UserList(IEnumerable<BaseEntity> list) : base(list.Cast<User>().ToList()) { }

        public override string ToString()
        {
            string output = "";

            for (int i = 0; i < this.Count; i++)
            {
                output += this[i] + "\n";
            }
            return output;
        }

        public void Print()
        {
            for (int i = 0; i < this.Count; i++)
            {
                Debug.WriteLine("---------------------------");
                Debug.WriteLine($"ID: {this[i].ID}" +
                    $"\nUsername: {this[i].Username}" +
                    $"\nGroup Number: {this[i].GroupNumber}" +
                    $"\nUnique Code: {this[i].UniqueCode}");
            }
            Debug.WriteLine("---------------------------");
        }
    }
}
