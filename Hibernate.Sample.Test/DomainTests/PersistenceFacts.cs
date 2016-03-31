using System;
using System.Linq;
using Hibernate.Sample.Test.Common;
using Hibernate.Sample.Test.Domain;
using Xunit;

namespace Hibernate.Sample.Test.DomainTests
{
    public class PersistenceFacts : TestBase
    {
        [Fact]
        public void should_get_user()
        {
            DeleteAllTalbes();

            using (var session = GetSession())
            {
                var user = session.Get<User>(1L);
                Assert.Null(user);
            }
        }

        [Fact]
        public void should_load_user()
        {
            DeleteAllTalbes();

            using (var session = GetSession())
            {
                var user = session.Load<User>(1L);
                Assert.ThrowsAny<Exception>(() => user.Name);
            }
        }

        [Fact]
        public void should_list_user()
        {
            DeleteAllTalbes();
            PrepareUser2();

            using (var session = GetSession())
            {
                var users = session.CreateQuery("from User2 where Name like 'Z%'").List<User2>();
                Assert.Equal(2, users.Count);
            }
        }

        [Fact]
        public void should_enumerable_user()
        {
            DeleteAllTalbes();
            PrepareUser2();

            using (var session = GetSession())
            {
                var users = session.CreateQuery("from User2 where Name like 'Z%'").Enumerable<User2>();
                Assert.Equal(2, users.Count());
            }
        }

        private void PrepareUser2()
        {
            var session = GetSession();
            using (var tx = session.BeginTransaction())
            {
                session.Save(new User2 {Id = 1, Name = "Zhu"});
                session.Save(new User2 {Id = 2, Name = "ZhuZhu"});
                session.Save(new User2 {Id = 3, Name = "Jiao"});
                tx.Commit();
            }
        }
    }
}