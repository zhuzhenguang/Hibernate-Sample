using System.Linq;
using Hibernate.Sample.Test.Common;
using Hibernate.Sample.Test.Domain;
using Xunit;

namespace Hibernate.Sample.Test.DomainTests
{
    public class UserFacts : TestBase
    {
        [Fact]
        public void should_save_user()
        {
            DeleteAllTalbes();

            var user = new User {Name = "Zhu"};

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

            var user = new User {Name = "Zhu"};
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

            var user = new User {Name = "Zhu"};
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

            var user = new User {Name = "Zhu"};
            var address = new Address {ZipCode = "100101", AddressDetail = "Beijing Dongzhimen"};
            user.Addresses.Add(address);

            var session = GetSession();
            using (var transaction = session.BeginTransaction())
            {
                session.Save(user);
                transaction.Commit();
            }

            Assert.True(user.Id > 0);
            Assert.NotEmpty(user.Addresses);
            Assert.True(user.Addresses.First().Id > 0);
        }

        [Fact]
        public void should_save_user_address_two_ways()
        {
            DeleteAllTalbes();

            var user = new User { Name = "Zhu" };
            var address = new Address
            {
                ZipCode = "100101",
                AddressDetail = "Beijing Dongzhimen",
                User = user
            };
            user.Addresses.Add(address);

            var session = GetSession();
            using (var transaction = session.BeginTransaction())
            {
                session.Save(user);
                transaction.Commit();
            }

            Assert.True(user.Id > 0);
            Assert.NotEmpty(user.Addresses);
            Assert.True(user.Addresses.First().Id > 0);
        }
    }
}