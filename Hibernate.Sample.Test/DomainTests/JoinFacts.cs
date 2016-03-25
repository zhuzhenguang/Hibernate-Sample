using System.Linq;
using Hibernate.Sample.Test.Common;
using Hibernate.Sample.Test.Domain;
using NHibernate.Linq;
using Xunit;

namespace Hibernate.Sample.Test.DomainTests
{
    public class JoinFacts : TestBase
    {
        [Fact]
        public void should_query_by_inner_join_with_hql()
        {
            DeleteAllTalbes();

            PrepareUserAddressData();

            var session = GetSession();
            var users =
                session.CreateQuery("from User user inner join fetch user.Contact.Addresses")
                    .List<User>()
                    .Distinct()
                    .ToList();
            Assert.Equal(2, users.Count);
            Assert.Equal("Zhu", users[0].Name.LastName);
            Assert.Equal("Shanghai", users[0].Contact.Addresses.First().AddressDetail);
            Assert.Equal("Beijing", users[0].Contact.Addresses.Last().AddressDetail);
            Assert.Equal("Jiao", users[1].Name.LastName);
            Assert.Equal("GuangZhou", users[1].Contact.Addresses.First().AddressDetail);
        }

        [Fact]
        public void should_query_by_inner_join_with_iqueryable()
        {
            DeleteAllTalbes();

            PrepareUserAddressData();

            var session = GetSession();
            var users = session.Query<User>();
            var addresses = session.Query<Address>();
            var searchedUser =
                users.Join(
                    addresses,
                    user => user,
                    address => address.User,
                    (user, address) => new User(user, address))
                    .ToList();

            Assert.Equal(3, searchedUser.Count);
            Assert.Equal(1, searchedUser[0].Contact.Addresses.Count);
            Assert.Equal("Zhu", searchedUser[0].Name.LastName);
            Assert.Equal("Shanghai", searchedUser[0].Contact.Addresses.First().AddressDetail);
            Assert.Equal(1, searchedUser[1].Contact.Addresses.Count);
            Assert.Equal("Zhu", searchedUser[1].Name.LastName);
            Assert.Equal("Beijing", searchedUser[1].Contact.Addresses.First().AddressDetail);
            Assert.Equal("Jiao", searchedUser[2].Name.LastName);
            Assert.Equal("GuangZhou", searchedUser[2].Contact.Addresses.First().AddressDetail);
        }

        [Fact]
        public void should_query_by_inner_join_with_linq()
        {
            DeleteAllTalbes();

            PrepareUserAddressData();

            var session = GetSession();
            var users = session.Query<User>().Where(user => user.Contact.Addresses.Count > 0).ToList();
            Assert.Equal(2, users.Count);
            Assert.Equal("Zhu", users[0].Name.LastName);
            Assert.Equal("Shanghai", users[0].Contact.Addresses.ToList()[0].AddressDetail);
            Assert.Equal("Beijing", users[0].Contact.Addresses.ToList()[1].AddressDetail);
            Assert.Equal("Jiao", users[1].Name.LastName);
            Assert.Equal("GuangZhou", users[1].Contact.Addresses.First().AddressDetail);
        }

        [Fact]
        public void should_query_by_left_join_with_hql()
        {
            DeleteAllTalbes();

            PrepareUserAddressData();

            var session = GetSession();
            var users = session.CreateQuery("from User user left join fetch user.Contact.Addresses").List<User>();

            Assert.Equal(5, users.Count);
            Assert.Equal("Zhu", users[0].Name.LastName);
            Assert.Equal("Shanghai", users[0].Contact.Addresses.ToList()[0].AddressDetail);
            Assert.Equal("Beijing", users[0].Contact.Addresses.ToList()[1].AddressDetail);
            Assert.Equal("Zhu", users[1].Name.LastName);
            Assert.Equal("Shanghai", users[1].Contact.Addresses.ToList()[0].AddressDetail);
            Assert.Equal("Beijing", users[1].Contact.Addresses.ToList()[1].AddressDetail);
            Assert.Equal("Jiao", users[2].Name.LastName);
            Assert.Equal("GuangZhou", users[2].Contact.Addresses.ToList()[0].AddressDetail);
            Assert.Equal("ZhuZhu", users[3].Name.LastName);
            Assert.Empty(users[3].Contact.Addresses);
            Assert.Equal("JiaoJiao", users[4].Name.LastName);
            Assert.Empty(users[4].Contact.Addresses);
        }

        [Fact]
        public void should_query_by_left_join_with_iqueryable()
        {
            DeleteAllTalbes();

            PrepareUserAddressData();

            var session = GetSession();
            var addresses = session.Query<Address>().ToList();
            var users = session.Query<User>().ToList();
            var searchedUsers = users
                .GroupJoin(
                    addresses,
                    user => user,
                    address => address.User,
                    (user, addressList) => new User(user, addressList.ToArray()))
                .SelectMany(
                    user => user.Contact.Addresses.DefaultIfEmpty(),
                    (user, address) => user)
                .ToList();
            Assert.Equal(5, searchedUsers.Count());

            Assert.Equal("Zhu", searchedUsers[0].Name.LastName);
            Assert.Equal("Shanghai", searchedUsers[0].Contact.Addresses.First().AddressDetail);
            Assert.Equal("Beijing", searchedUsers[0].Contact.Addresses.Last().AddressDetail);
            Assert.Equal("Zhu", searchedUsers[1].Name.LastName);
            Assert.Equal("Shanghai", searchedUsers[1].Contact.Addresses.First().AddressDetail);
            Assert.Equal("Beijing", searchedUsers[1].Contact.Addresses.Last().AddressDetail);
            Assert.Equal("Jiao", searchedUsers[2].Name.LastName);
            Assert.Equal("GuangZhou", searchedUsers[2].Contact.Addresses.First().AddressDetail);
            Assert.Equal("ZhuZhu", searchedUsers[3].Name.LastName);
            Assert.Empty(searchedUsers[3].Contact.Addresses);
            Assert.Equal("JiaoJiao", searchedUsers[4].Name.LastName);
            Assert.Empty(searchedUsers[4].Contact.Addresses);
        }

        [Fact]
        public void should_query_by_right_join()
        {
            DeleteAllTalbes();

            PrepareUserAddressData();

            var session = GetSession();
            var users = session.CreateQuery("from User user right join fetch user.Contact.Addresses").List<User>();

            Assert.Equal(4, users.Count);
            Assert.Equal("Zhu", users[0].Name.LastName);
            Assert.Equal("Shanghai", users[0].Contact.Addresses.First().AddressDetail);
            Assert.Equal("Beijing", users[0].Contact.Addresses.Last().AddressDetail);
            Assert.Equal("Zhu", users[1].Name.LastName);
            Assert.Equal("Shanghai", users[1].Contact.Addresses.First().AddressDetail);
            Assert.Equal("Beijing", users[1].Contact.Addresses.Last().AddressDetail);
            Assert.Equal("Jiao", users[2].Name.LastName);
            Assert.Equal("GuangZhou", users[2].Contact.Addresses.First().AddressDetail);
            Assert.Null(users[3]);
            //Assert.Equal("Hongkong", users[3].Contact.Addresses.First().AddressDetail);
        }

        private void PrepareUserAddressData()
        {
            var user1 = new User("Zhu");
            user1.AddAddress(new Address { AddressDetail = "Shanghai", User = user1 });
            user1.AddAddress(new Address { AddressDetail = "Beijing", User = user1 });

            var user2 = new User("Jiao");
            user2.AddAddress(new Address { AddressDetail = "GuangZhou", User = user2 });

            var user3 = new User("ZhuZhu");
            var user4 = new User("JiaoJiao");
            var address = new Address { AddressDetail = "Hongkongs" };

            var session = GetSession();
            using (var tx = session.BeginTransaction())
            {
                session.Save(user1);
                session.Save(user2);
                session.Save(user3);
                session.Save(user4);
                session.Save(address);
                tx.Commit();
            }
            session.Close();
        }
    }
}