using System.Collections.Generic;

namespace Hibernate.Sample.Test.Domain
{
    public class User4
    {
        public User4()
        {
        }

        public virtual long Id { get; set; }
        public virtual string LastName { get; set; }
        public virtual ICollection<string> Addresses { get; set; }
    }
}