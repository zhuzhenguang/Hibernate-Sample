namespace Hibernate.Sample.Domain
{
    public class User
    {
        private long _id;
        private string _name;

        public virtual long Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}