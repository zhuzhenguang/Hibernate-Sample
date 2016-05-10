using System.Data;
using System.Linq;
using Hibernate.Sample.Test.Common;
using Hibernate.Sample.Test.Domain;
using Xunit;

namespace Hibernate.Sample.Test.DomainTests
{
    public class CallFacts : TestBase
    {
        public CallFacts()
        {
            DeleteAllTalbes();
        }

        [Fact]
        public void should_log_when_add_user()
        {
            var user = new User4 {LastName = "Jiao"};

            using (var session = GetSession())
            {
                session.Save(user);
                session.Flush();
            }

            Assert.Equal(1, user.Logs.Count);
            Assert.Equal(string.Format("save user {0}", user.LastName), user.Logs.First());
        }

        [Fact]
        public void should_log_when_update_user()
        {
            var user = PrepareUser();

            using (var session = GetSession())
            {
                user.LastName = "Zhu";
                session.Update(user);
                session.Flush();
            }

            Assert.Equal(2, user.Logs.Count);
            Assert.Equal(string.Format("update user {0}", user.Id), user.Logs.Last());
        }

        [Fact]
        public void should_log_when_delete_user()
        {
            var user = PrepareUser();

            using (var session = GetSession())
            {
                session.Delete(user);
                session.Flush();
            }

            Assert.Equal(2, user.Logs.Count);
            Assert.Equal(string.Format("delete user {0}", user.Id), user.Logs.Last());
        }

        [Fact]
        public void should_log_when_load_user()
        {
            var userId = PrepareUser().Id;
            User4 user1;
            User4 user2;

            using (var session = GetSession())
            {
                user2 = session.Get<User4>(userId);
                user1 = session.Load<User4>(userId);
            }

            Assert.Equal(1, user1.Logs.Count);
            Assert.Equal(1, user2.Logs.Count);
            Assert.Equal(string.Format("load user {0}", user1.Id), user1.Logs.ToList()[0]);
            Assert.Equal(string.Format("load user {0}", user2.Id), user2.Logs.ToList()[0]);
        }

        [Fact]
        public void should_throw_exception_when_last_name_is_empty()
        {
            var user = new User4 { LastName = "" };

            using (var session = GetSession())
            {
                Assert.Throws<DataException>(() => session.Save(user));
            }
        }

        private User4 PrepareUser()
        {
            var user = new User4 { LastName = "Jiao" };

            using (var session = GetSession())
            {
                session.Save(user);
                session.Flush();
            }
            return user;
        }
    }
}