using System.Collections.Generic;

namespace Hibernate.Sample.Test.Domain
{
    public class User5
    {
        public User5()
        {
            Addresses = new Dictionary<string, string>();
        }

        public virtual long Id { get; set; }
        public virtual string LastName { get; set; }
        public virtual IDictionary<string, string> Addresses { get; set; }

        public virtual void AddAddress(string type, string detail)
        {
            Addresses[type] = detail;
        }
    }
}