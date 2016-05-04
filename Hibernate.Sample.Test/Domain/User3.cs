using System.Collections.Generic;

namespace Hibernate.Sample.Test.Domain
{
    public class User3
    {
        public User3()
        {
            Addresses = new HashSet<Address2>();
        }

        public User3(string name) : this()
        {
            Name = name;
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