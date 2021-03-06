﻿using System.Collections.Generic;
using System.Dynamic;

namespace Hibernate.Sample.Test.Domain
{
    public class Group
    {
        public Group()
        {
            Roles = new HashSet<Role>();
        }

        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}