namespace Hibernate.Sample.Test.Domain
{
    public class Item2
    {
        public virtual long Id { get; set; }
        public virtual string Manufacturer { get; set; }
        public virtual string Name { get; set; } 
    }

    public class Dvd2 : Item2
    {
        public virtual string RegionCode { get; set; }
    }

    public class Book2 : Item2
    {
        public virtual int PageCount { get; set; }
    }
}