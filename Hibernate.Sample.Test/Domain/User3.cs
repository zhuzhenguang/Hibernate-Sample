using System.Collections.Generic;

namespace Hibernate.Sample.Test.Domain
{
    public class User3
    {
        public User3()
        {
        }

        public User3(string name)
        {
            Name = name;
            Addresses = new HashSet<Address2>();
        }

        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ICollection<Address2> Addresses { get; set; }

        public virtual void AddAddress(Address2 address)
        {
            Addresses.Add(address);
        }
    }
}