using System.Linq;
using Hibernate.Sample.Test.Common;
using Hibernate.Sample.Test.Domain;
using NHibernate.Linq;
using Xunit;

namespace Hibernate.Sample.Test.DomainTests
{
    public class UserFacts : TestBase
    {
        [Fact]
        public void should_save_user()
        {
            DeleteAllTalbes();

            var user = new User("Zhu");

            var session = GetSession();
            var transaction = session.BeginTransaction();

            using (session)
            using (transaction)
            {
                session.Save(user);
                session.Flush();
                transaction.Commit();
            }

            Assert.True(user.Id > 0);
        }

        [Fact]
        public void should_save_user_passport()
        {
            DeleteAllTalbes();

            var user = new User("Zhu");
            var passport = new Passport {Serial = "df890890", Expiry = "20190101"};

            user.Passport = passport;
            passport.User = user;

            var session = GetSession();
            using (var transaction = session.BeginTransaction())
            {
                session.Save(user);
                transaction.Commit();
            }

            var user2 = GetSession().Get<User>(user.Id);
            Assert.Equal("20190101", user2.Passport.Expiry);
            Assert.Equal("df890890", user2.Passport.Serial);
        }

        [Fact]
        public void should_save_user_group()
        {
            DeleteAllTalbes();

            var user = new User("Zhu");
            var group = new Group {Name = "Admin Group"};

            user.Group = group;

            var session = GetSession();
            using (var transaction = session.BeginTransaction())
            {
                session.Save(user);
                transaction.Commit();
            }

            Assert.True(user.Id > 0);
            Assert.True(group.Id > 0);
        }

        [Fact]
        public void should_save_user_address()
        {
            DeleteAllTalbes();

            var user = new User("Zhu");
            var address = new Address {ZipCode = "100101", AddressDetail = "Beijing Dongzhimen"};
            user.AddAddress(address);

            var session = GetSession();
            using (var transaction = session.BeginTransaction())
            {
                session.Save(user);
                transaction.Commit();
            }

            Assert.True(user.Id > 0);
            Assert.NotEmpty(user.Contact.Addresses);
            Assert.True(user.Contact.Addresses.First().Id > 0);
        }

        [Fact]
        public void should_save_user_address_two_ways()
        {
            DeleteAllTalbes();

            var user = new User("Zhu");
            var address = new Address
            {
                ZipCode = "100101",
                AddressDetail = "Beijing Dongzhimen",
                User = user
            };
            user.AddAddress(address);

            var session = GetSession();
            using (var transaction = session.BeginTransaction())
            {
                session.Save(user);
                transaction.Commit();
            }

            Assert.True(user.Id > 0);
            Assert.NotEmpty(user.Contact.Addresses);
            Assert.True(user.Contact.Addresses.First().Id > 0);
        }

        [Fact]
        public void should_save_query_user_with_resume()
        {
            DeleteAllTalbes();

            var user = new UserWithResume("Zhu") {Resume = "My name is Zhu."};

            var session = GetSession();
            using (var transaction = session.BeginTransaction())
            {
                session.Save(user);
                transaction.Commit();
            }

            Assert.True(user.Id > 0);
        }

        [Fact]
        public void should_persistent()
        {
            DeleteAllTalbes();

            var user1 = new User("Zhu");
            var user2 = new User("ZhuZhu");

            var session = GetSession();
            using (var tx1 = session.BeginTransaction())
            {
                session.Save(user1);
                tx1.Commit();
            }

            using (var tx2 = session.BeginTransaction())
            {
                user1.Name = new Name { FirstName = "Ming", LastName = "Jiao" };
                user2.Name = new Name { FirstName = "Ming", LastName = "Jiao" };
                tx2.Commit();
            }

            var users = GetSession().Query<User>().ToList();
            Assert.Equal(1, users.Count);
            Assert.Equal("Jiao", users[0].Name.LastName);
            Assert.Equal("Ming", users[0].Name.FirstName);
        }

        [Fact]
        public void should_detached()
        {
            DeleteAllTalbes();;

            var user = new User("Zhu");

            var session1 = GetSession();
            using (var tx1 = session1.BeginTransaction())
            {
                session1.Save(user);
                tx1.Commit();
            }

            Assert.True(user.Id > 0);
            session1.Close();

            var session2 = GetSession();
            using (var tx2 = session2.BeginTransaction())
            {
                session2.Update(user);
                tx2.Commit();
            }

            var users = GetSession().Query<User>().ToList();
            Assert.Equal(1, users.Count);
        }

        [Fact]
        public void should_delete_and_return_to_transient()
        {
            DeleteAllTalbes(); ;

            var user = new User("Zhu");

            var session1 = GetSession();
            using (var tx1 = session1.BeginTransaction())
            {
                session1.Save(user);
                tx1.Commit();
            }

            using (var tx2 = session1.BeginTransaction())
            {
                session1.Delete(user);
                tx2.Commit();
            }

            var users = GetSession().Query<User>().ToList();
            Assert.Equal(0, users.Count);
        }
    }
}