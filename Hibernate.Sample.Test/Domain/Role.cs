using System.Collections.Generic;

namespace Hibernate.Sample.Test.Domain
{
    public class Role
    {
        public Role()
        {
            Groups = new HashSet<Group>();
        }

        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ICollection<Group> Groups { get; set; } 
    }
}