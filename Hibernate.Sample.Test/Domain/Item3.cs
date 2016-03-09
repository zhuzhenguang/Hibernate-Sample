namespace Hibernate.Sample.Test.Domain
{
    public class Item3
    {
        public virtual long Id { get; set; }
        public virtual string Manufacturer { get; set; }
        public virtual string Name { get; set; }  
    }

    public class Dvd3 : Item3
    {
        public virtual string RegionCode { get; set; }
    }

    public class Book3 : Item3
    {
        public virtual int PageCount { get; set; }
    }
}