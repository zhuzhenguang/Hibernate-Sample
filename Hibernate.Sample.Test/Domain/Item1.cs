namespace Hibernate.Sample.Test.Domain
{
    public class Item1
    {
        public virtual long Id { get; set; }
        public virtual string Manufacturer { get; set; }
        public virtual string Name { get; set; }
    }

    public class Dvd1 : Item1
    {
        public virtual string RegionCode { get; set; }
    }

    public class Book1 : Item1
    {
        public virtual int PageCount { get; set; }
    }
}