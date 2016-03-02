namespace Hibernate.Sample.Test.Domain
{
    public class Passport
    {
        public virtual long Id { get; set; }
        public virtual string Serial { get; set; }
        public virtual string Expiry { get; set; }
        public virtual User User { get; set; }
    }
}