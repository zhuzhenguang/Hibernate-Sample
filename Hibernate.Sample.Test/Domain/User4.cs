using System;
using System.Collections.Generic;
using System.Data;
using NHibernate;
using NHibernate.Classic;

namespace Hibernate.Sample.Test.Domain
{
    // test for set/bag/idbag
    // test lifecycle, validatable
    public class User4 : ILifecycle, IValidatable
    {
        public User4()
        {
            Logs = new List<string>();
        }

        public virtual long Id { get; set; }
        public virtual string LastName { get; set; }
        public virtual ICollection<string> Addresses { get; set; }

        public virtual ICollection<string> Logs { get; set; }

        public virtual LifecycleVeto OnSave(ISession s)
        {
            Logs.Add(string.Format("save user {0}", LastName));
            return LifecycleVeto.NoVeto;
        }

        public virtual LifecycleVeto OnUpdate(ISession s)
        {
            Logs.Add(string.Format("update user {0}", Id));
            return LifecycleVeto.NoVeto;
        }

        public virtual LifecycleVeto OnDelete(ISession s)
        {
            Logs.Add(string.Format("delete user {0}", Id));
            return LifecycleVeto.NoVeto;
        }

        public virtual void OnLoad(ISession s, object id)
        {
            Logs.Add(string.Format("load user {0}", id));
        }

        public virtual void Validate()
        {
            if (string.IsNullOrEmpty(LastName))
            {
                throw new DataException("Last name is not allowed to empty.");
            }
        }
    }
}