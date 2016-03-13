namespace Hibernate.Sample.Test.Domain
{
    public class UserWithResume : User
    {
        public UserWithResume(string lastName) : base(lastName)
        {
        }

        public UserWithResume()
        {
        }

        public virtual string Resume { get; set; }
    }
}