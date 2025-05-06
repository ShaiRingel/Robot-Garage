using Model;
using View_Model.DB;

namespace View_Model.Services
{
    public class ProductSVC
    {
        ProductDB productDB = new ProductDB();

        public List<Product> GetAll()
        {
            return productDB.GetAllProducts();
        }

        public Product GetByID(int id)
        {
            return productDB.GetProductByID(id);
        }

        public void InsertProduct(Product product)
        {
            productDB.Insert(product);
        }

        public void UpdateProduct(Product product)
        {
            productDB.Update(product);
        }

        public void DeleteProduct(Product product)
        {
            productDB.Delete(product);
        }
    }
}

/*
public static ObservableCollection<Event> Load(User usr)
        {
            return new CaleDb().SelectPerId(usr);
        }

        public static Event GetEvent(ObservableCollection<Event> l, int id)
        {
            for (int i = 0; i < l.Count; i++)
            {
                if (l[i]._ID == id) return l[i];
            }
            return null;
        }

        public static ObservableCollection<Event> Storm(ObservableCollection<Event> l, string d) // an unfitting name ik, but I like it (returns all event._date == d for user)
        {
            ObservableCollection<Event> strm = new ObservableCollection<Event>();
            foreach (Event ev in l)
            {
                if (ev._date == d) strm.Add(ev);
            }
            return strm;
        }
        public static ObservableCollection<Event> Vortex(User u, string d) // the upgrade of storm; d is unspaced date
        {
            ObservableCollection<Event> events = new CaleDb().SelectPerId(u);
            ObservableCollection<Event> ell = new ObservableCollection<Event>();
            foreach (Event e in events)
            {
                if (e._date == d) ell.Add(e);
            }
            return ell;
        }

        public static int CreateNewID()
        {
            return new CaleDb().ReturnNextID();
        }

        public static Event Find(string name, User u, string date) => new CaleDb().FindEvent(name, u, date);


        public static void Tear(Event ev)
        {
            new CaleDb().Remove(ev);
        }
        public static void Write(Event ev)
        {
            new CaleDb().Insert(ev);
        }
        public static void ReWrite(Event Old, Event New)
        {
            new CaleDb().Update(Old, New);
        }

        public static List<string> ActiveDays(int id, string month, string year) => new CaleDb().DaysActive(id, month, year, 0);
        public static List<string> SuperActiveDays(int id, string month, string year) => new CaleDb().DaysActive(id, month, year, 1);


        // full writing mechanism, command template function
        public static void WriteEclipse(User u, Event target, string Name_, string Text_, string _date_, string cid = "--")
        {
            if (target == null) // add new
            {
                target = new Event();
                target._cid = cid; // its already "--", but per corp write it sets it accordingly (only for new events ofc)

                // set info
                target._name = Name_;
                target._description = Text_;

                //target._ID = CreateNewID(); // not needed, id is autonumber

                target._date = _date_.Replace(" ", ""); // in case of
                target._uid = u._absId; // set id to user

                Write(target); // call insert
            }
            else // edit selected
            {
                Event tor = target.Copy(); // all properties besides main info set

                target._name = Name_;
                target._description = Text_;
                // no need to change anything else, we have add + delete for that

                ReWrite(tor, target); // call update
            }
        }
*/