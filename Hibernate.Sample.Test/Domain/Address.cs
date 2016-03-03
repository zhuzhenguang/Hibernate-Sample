namespace Hibernate.Sample.Test.Domain
{
    public class Address
    {
        public virtual long Id { get; set; }
        public virtual string AddressDetail { get; set; }
        public virtual string ZipCode { get; set; }
        public virtual User User { get; set; }
    }
}