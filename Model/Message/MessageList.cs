using System.Diagnostics;

namespace Model
{
    public class MessageList : List<Message>
    {
        public MessageList() { }
        public MessageList(IEnumerable<Message> list) : base(list) { }
        public MessageList(IEnumerable<BaseEntity> list) : base(list.Cast<Message>().ToList()) { }

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
                    $"\nMessage: {this[i].Content}" +
                    $"\nSender: {this[i].Sender}" +
                    $"\nReciever: {this[i].Receiver}" +
                    $"\nProduct Discussed: {this[i].Product}");
            }
            Debug.WriteLine("---------------------------");
        }
    }
}
