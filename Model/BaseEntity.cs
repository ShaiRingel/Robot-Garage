namespace Model
{
    public abstract class BaseEntity
    {
        private int id;
		public int ID {
			get => id;
			set => id = value;
		}
	}
}
