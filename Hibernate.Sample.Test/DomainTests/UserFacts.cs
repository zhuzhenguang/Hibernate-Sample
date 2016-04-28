using System;
using System.Linq;
using Hibernate.Sample.Test.Common;
using Hibernate.Sample.Test.Domain;
using NHibernate.Linq;
using NHibernate.Util;
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

            var user = new User3("Zhu");
            var address = new Address2 {Detail = "dongzhimen"};

            user.AddAddress(address);

            var session = GetSession();
            using (var transaction = session.BeginTransaction())
            {
                session.Save(user);
                transaction.Commit();
            }

            Assert.True(user.Id > 0);
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
                user1.Name = new Name {FirstName = "Ming", LastName = "Jiao"};
                user2.Name = new Name {FirstName = "Ming", LastName = "Jiao"};
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
            DeleteAllTalbes();
            ;

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
            DeleteAllTalbes();
            ;

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

        // entity key
        [Fact]
        public void should_recognize_identity()
        {
            DeleteAllTalbes();
            PrepareUser2();

            var session = GetSession();

            var user1 = session.Load<User2>(1L);
            user1.Name = "Jiao";

            var user2 = session.Load<User2>(1L);

            Assert.True(user1 == user2);
        }

        // override equal/hashcode with id
        [Fact]
        public void should_save_address()
        {
            DeleteAllTalbes();

            var user = new User("Zhu");
            user.AddAddress(new Address {AddressDetail = "dongzhimen", User = user});
            user.AddAddress(new Address {AddressDetail = "xizhimen", User = user});
            var session = GetSession();
            using (var tx = session.BeginTransaction())
            {
                session.Save(user);
                tx.Commit();
            }
            session.Close();

            var searchedUser = GetSession().Get<User>(user.Id);
            Assert.Equal(2, searchedUser.Contact.Addresses.Count);
        }

        // not override equal hashcode
        [Fact]
        public void should_save_across_session()
        {
            DeleteAllTalbes();
            var userId = PrepareUserAndAddress();

            var session1 = GetSession();
            var user1 = session1.Load<User>(userId);
            Assert.Equal(2, user1.Contact.Addresses.Count);
            var address1 = user1.Contact.Addresses.First();
            session1.Close();

            ////////////////////////////////////////////////

            var session2 = GetSession();
            var user2 = session2.Load<User>(userId);
            user2.AddAddress(address1);
            Assert.Equal(2, user2.Contact.Addresses.Count);
            session2.Save(user2);
            session2.Flush();
            session2.Close();
        }

        [Fact]
        public void should_save_address_using_unsaved_value()
        {
            DeleteAllTalbes();

            var user = new User("Zhu");
            user.AddAddress(new Address {AddressDetail = "dongzhimen", User = user});

            var session = GetSession();
            using (var tx = session.BeginTransaction())
            {
                session.Save(user);
                tx.Commit();
            }
            session.Close();

            var users = GetSession().Query<User>().ToList();
            Assert.Equal(1, users.Count);
            Assert.Equal(1, users.First().Contact.Addresses.Count);
            Assert.True(users.First().Contact.Addresses.First().Id != 2);
        }

        [Fact]
        public void should_query_by_inner_join_with_hql()
        {
            DeleteAllTalbes();

            PrepareUserDataForJoin();

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

            PrepareUserDataForJoin();

            var session = GetSession();
            //var users = session.Query<User>().Where(user => user.Contact.Addresses.Count > 0).ToList();
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
            Assert.Equal("Zhu", searchedUser[0].Name.LastName);
            Assert.Equal("Shanghai", searchedUser[0].Contact.Addresses.First().AddressDetail);
            Assert.Equal("Zhu", searchedUser[1].Name.LastName);
            Assert.Equal("Beijing", searchedUser[1].Contact.Addresses.First().AddressDetail);
            Assert.Equal("Jiao", searchedUser[2].Name.LastName);
            Assert.Equal("GuangZhou", searchedUser[2].Contact.Addresses.First().AddressDetail);
        }

        [Fact]
        public void should_query_by_inner_join_with_linq()
        {
            DeleteAllTalbes();

            PrepareUserDataForJoin();

            var session = GetSession();
            var users = session.Query<User>().Where(user => user.Contact.Addresses.Count > 0).ToList();
            Assert.Equal(3, users.Count);
            Assert.Equal("Zhu", users[0].Name.LastName);
            Assert.Equal("Shanghai", users[0].Contact.Addresses.ToList()[0].AddressDetail);
            Assert.Equal("Beijing", users[0].Contact.Addresses.ToList()[1].AddressDetail);
            Assert.Equal("Jiao", users[2].Name.LastName);
            Assert.Equal("GuangZhou", users[2].Contact.Addresses.First().AddressDetail);
        }

        [Fact]
        public void should_query_by_left_join_with_hql()
        {
            DeleteAllTalbes();

            PrepareUserDataForJoin();

            var session = GetSession();
            var users = session.CreateQuery("from User user left join fetch user.Contact.Addresses").List<User>();

            Assert.Equal(5, users.Count);
            Assert.Equal("Zhu", users[0].Name.LastName);
            Assert.Equal("Shanghai", users[0].Contact.Addresses.First().AddressDetail);
            Assert.Equal("Beijing", users[0].Contact.Addresses.Last().AddressDetail);
            Assert.Equal("Zhu", users[1].Name.LastName);
            Assert.Equal("Shanghai", users[1].Contact.Addresses.First().AddressDetail);
            Assert.Equal("Beijing", users[1].Contact.Addresses.Last().AddressDetail);
            Assert.Equal("Jiao", users[2].Name.LastName);
            Assert.Equal("GuangZhou", users[2].Contact.Addresses.First().AddressDetail);
            Assert.Equal("ZhuZhu", users[3].Name.LastName);
            Assert.Empty(users[3].Contact.Addresses);
            Assert.Equal("JiaoJiao", users[4].Name.LastName);
            Assert.Empty(users[4].Contact.Addresses);
        }

        [Fact]
        public void should_query_by_left_join_with_iqueryable()
        {
            DeleteAllTalbes();

            PrepareUserDataForJoin();

            var session = GetSession();
            var addresses = session.Query<Address>().ToList();
            var users = session.Query<User>().ToList();
            var searchedUsers = users
                .GroupJoin(
                    addresses,
                    user => user,
                    address => address.User,
                    (user, addressList) => new {User = user, Addresses = addressList})
                .SelectMany(
                    ua => ua.Addresses.DefaultIfEmpty(),
                    (ua, address) =>
                    {
                        var user = ua.User;
                        if (address != null)
                        {
                            user.AddAddress(address);
                        }
                        return user;
                    })
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

            PrepareUserDataForJoin();

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

        private void PrepareUserDataForJoin()
        {
            var user1 = new User("Zhu");
            user1.AddAddress(new Address {AddressDetail = "Shanghai", User = user1});
            user1.AddAddress(new Address {AddressDetail = "Beijing", User = user1});

            var user2 = new User("Jiao");
            user2.AddAddress(new Address {AddressDetail = "GuangZhou", User = user2});

            var user3 = new User("ZhuZhu");
            var user4 = new User("JiaoJiao");
            var address = new Address {AddressDetail = "Hongkongs"};

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

        private long PrepareUserAndAddress()
        {
            var user = new User("Zhu");
            user.AddAddress(new Address {AddressDetail = "dongzhimen", User = user});
            user.AddAddress(new Address {AddressDetail = "xizhimen", User = user});

            var session = GetSession();
            using (var tx = session.BeginTransaction())
            {
                session.Save(user);
                tx.Commit();
            }
            session.Close();
            return user.Id;
        }

        private void PrepareUser2()
        {
            var session1 = GetSession();
            using (var tx = session1.BeginTransaction())
            {
                session1.Save(new User2 {Id = 1, Name = "Zhu"});
                tx.Commit();
            }
            session1.Close();
        }
    }
}