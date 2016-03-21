using System.Collections.Generic;

namespace Hibernate.Sample.Test.Domain
{
    public class User
    {
        public User(string lastName) : this()
        {
            Name = new Name {LastName = lastName};
        }

        public User(User user, params Address[] addresses) : this()
        {
            Name = new Name {FirstName = user.Name.FirstName, LastName = user.Name.LastName};
            foreach (var address in addresses)
            {
                AddAddress(address);
            }
        }

        public User()
        {
            Contact = new Contact();
        }

        public virtual long Id { get; set; }
        public virtual Name Name { get; set; }
        public virtual Passport Passport { get; set; }
        public virtual Group Group { get; set; }
        public virtual Contact Contact { get; set; }

        public virtual void AddAddress(Address address)
        {
            Contact.Addresses.Add(address);
        }


    }

    public class Name
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
    }

    public class Contact
    {
        public Contact()
        {
            Addresses = new HashSet<Address>();
        }

        public virtual string Telephone { get; set; }
        public virtual ISet<Address> Addresses { get; set; }
    }
}