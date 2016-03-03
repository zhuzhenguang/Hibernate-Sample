using System.Collections.Generic;

namespace Hibernate.Sample.Test.Domain
{
    public class User
    {
        public User()
        {
            Addresses = new HashSet<Address>();
        }

        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual Passport Passport { get; set; }
        public virtual Group Group { get; set; }
        public virtual ISet<Address> Addresses { get; set; } 
    }
}