using System.Collections.Generic;

namespace Hibernate.Sample.Test.Domain
{
    public class User6
    {
        public User6()
        {
            Addresses = new List<string>();
        }

        public virtual long Id { get; set; }
        public virtual string LastName { get; set; }
        public virtual IList<string> Addresses { get; set; }
    }
}