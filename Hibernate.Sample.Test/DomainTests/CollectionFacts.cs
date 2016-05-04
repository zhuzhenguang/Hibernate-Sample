using System.Linq;
using Hibernate.Sample.Test.Common;
using Hibernate.Sample.Test.Domain;
using Xunit;

namespace Hibernate.Sample.Test.DomainTests
{
    public class CollectionFacts : TestBase
    {
        public CollectionFacts()
        {
            DeleteAllTalbes();
        }

        [Fact]
        public void should_return_set()
        {
            var userId = PrepareUserWithSameAddresses();

            using (var session = GetSession())
            {
                var user = session.Load<User4>(userId);
                Assert.Equal(1, user.Addresses.Count);
            }
        }

        [Fact]
        public void should_delete_set()
        {
            var userId = PrepareUserWithSameAddresses();

            using (var session = GetSession())
            {
                var user = session.Load<User4>(userId);
                user.Addresses.Remove(user.Addresses.First());

                session.Flush();
            }

            using (var session = GetSession())
            {
                var user = session.Load<User4>(userId);
                Assert.Empty(user.Addresses);
            }
        }

        [Fact]
        public void should_return_bag()
        {
            var userId = PrepareUserWithSameAddresses();

            using (var session = GetSession())
            {
                var user = session.Load<User>(userId);
                Assert.Equal(3, user.Contact.Addresses.Count);
            }
        }

        [Fact]
        public void should_delete_bag()
        {
            var userId = PrepareUserWithSameAddresses();

            using (var session = GetSession())
            {
                var user = session.Load<User>(userId);
                //user.RemoveAddress(user.Contact.Addresses.First());
                user.Contact.Addresses.First().User = null;
                session.Flush();
            }

            using (var session = GetSession())
            {
                var user = session.Load<User>(userId);
                Assert.Equal(2, user.Contact.Addresses.Count);
            }
        }

        [Fact]
        public void should_return_map()
        {
            var user = new User5 {LastName = "Zhu"};
            user.AddAddress("Home", "dongzhimen");
            user.AddAddress("Office", "xizhimen");

            using (var session = GetSession())
            {
                session.Save(user);
                session.Flush();
            }

            using (var session = GetSession())
            {
                var user5 = session.Load<User5>(user.Id);
                Assert.Equal("dongzhimen", user5.Addresses["Home"]);
                Assert.Equal("xizhimen", user5.Addresses["Office"]);
            }
        }

        [Fact]
        public void should_return_list()
        {
            var user = new User6 { LastName = "Zhu" };
            user.Addresses.Add("dognzhimen");
            user.Addresses.Add("xizhimen");

            using (var session = GetSession())
            {
                session.Save(user);
                session.Flush();
            }

            using (var session = GetSession())
            {
                var user6 = session.Load<User6>(user.Id);
                user6.Addresses[0] = "xizhimen";
                user6.Addresses[1] = "dongzhimen";
                session.Flush();
            }
        }

        private long PrepareUserWithSameAddresses()
        {
            var user = new User("Jiao");
            user.AddAddress(new Address {AddressDetail = "dongzhimen"});
            user.AddAddress(new Address {AddressDetail = "dongzhimen"});
            user.AddAddress(new Address {AddressDetail = "dongzhimen"});

            using (var session = GetSession())
            {
                session.Save(user);
                session.Flush();
            }
            return user.Id;
        }
    }
}