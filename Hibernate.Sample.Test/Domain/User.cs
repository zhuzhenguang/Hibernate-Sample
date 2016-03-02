namespace Hibernate.Sample.Test.Domain
{
    public class User
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual Passport Passport { get; set; }
        public virtual Group Group { get; set; }
    }
}