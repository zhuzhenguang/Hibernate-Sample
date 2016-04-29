using System;
using System.Collections.Generic;
using System.Linq;
using Hibernate.Sample.Test.Common;
using Hibernate.Sample.Test.Domain;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using Xunit;

namespace Hibernate.Sample.Test.DomainTests
{
    public class PersistenceFacts : TestBase
    {
        public PersistenceFacts()
        {
            DeleteAllTalbes();
        }

        [Fact]
        public void should_get_user()
        {
            using (var session = GetSession())
            {
                var user = session.Get<User>(1L);
                Assert.Null(user);
            }
        }

        [Fact]
        public void should_load_user()
        {
            using (var session = GetSession())
            {
                var user = session.Load<User>(1L);
                Assert.ThrowsAny<Exception>(() => user.Name);
            }
        }

        [Fact]
        public void should_list_user()
        {
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
            PrepareUser2();

            using (var session = GetSession())
            {
                var users = session.CreateQuery("from User2 where Name like 'Z%'").Enumerable<User2>();
                Assert.Equal(2, users.Count());
            }
        }

        [Fact]
        public void should_throw_exception_when_load_collection_out_of_session()
        {
            PrepareUserAddress();

            ISet<Address> addresses;
            using (var session = GetSession())
            {
                //var user = session.Query<User>().Single(u => u.Name.LastName == "Zhu");
                var user = session.CreateQuery("from User where Name.LastName = 'Zhu'").UniqueResult<User>();
                Console.WriteLine(user.Name);
                addresses = user.Contact.Addresses;
            }

            Assert.Throws<LazyInitializationException>(() => addresses.ToArray());
        }

        [Fact]
        public void should_fetch_lazy_load_collection()
        {
            PrepareUserAddress();

            User user;
            using (var session = GetSession())
            {
                /*user = session.CreateQuery("from User where Name.LastName = 'Zhu'").UniqueResult<User>();
                Console.WriteLine(user.Name);
                NHibernateUtil.Initialize(user.Contact.Addresses);*/

                /*user = session.Query<User>()
                    .Where(u => u.Name.LastName == "Zhu")
                    .Fetch(u => u.Contact.Addresses)
                    .FirstOrDefault();*/

                user = session.CreateCriteria<User>()
                    .Add(Restrictions.Eq("Name.LastName", "Zhu"))
                    .SetFetchMode("Contact.Addresses", FetchMode.Eager)
                    .UniqueResult<User>();

                /*user = session.QueryOver<User>()
                    .Where(u => u.Name.LastName == "Zhu")
                    .Fetch(u => u.Contact.Addresses)
                    .Eager
                    .SingleOrDefault<User>();*/
            }

            var addresses = user.Contact.Addresses;
            var telephone = user.Contact.Telephone;
            Assert.Equal("123", telephone);
            Assert.Equal("Shanghai", addresses.First().AddressDetail);
        }

        [Fact]
        public void should_lazy_load_resume()
        {
            PrepareUserWithResume();

            User2 user;
            using (var session = GetSession())
            {
                //user = session.Load<User2>(1L);
                user = session.CreateQuery("from User2 where id = 1").UniqueResult<User2>();
            }

            Assert.Equal("Zhu", user.Name);
            Assert.False(NHibernateUtil.IsPropertyInitialized(user, "Resume"));
            Assert.Throws<LazyInitializationException>(() => user.Resume);
        }

        [Fact]
        public void should_batch_delete_user()
        {
            var users = BatchSaveUsers(3);

            using (var session = GetSession())
            using (var tx = session.BeginTransaction())
            {
                var query = session.CreateQuery("delete User");
                query.ExecuteUpdate();

                tx.Commit();

                var user = session.Get<User>(users[0].Id);
                Assert.Null(user);
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

        private void PrepareUserAddress()
        {
            Console.WriteLine("=========================insert data start=========================");

            using (var session = GetSession())
            using (var tx = session.BeginTransaction())
            {
                var user = new User("Zhu") {Contact = {Telephone = "123"}};
                user.AddAddress(new Address { AddressDetail = "Shanghai", User = user });
                user.AddAddress(new Address { AddressDetail = "Beijing", User = user });
                user.AddAddress(new Address { AddressDetail = "GuangZhou", User = user });
                session.Save(user);
                tx.Commit();
            }

            Console.WriteLine("=========================insert data end===========================");
            Console.WriteLine();
        }

        private void PrepareUserWithResume()
        {
            var user = new User2 {Id = 1, Name = "Zhu", Resume = "I am Zhu."};

            using (var session = GetSession())
            using (var tx = session.BeginTransaction())
            {
                session.Save(user);
                tx.Commit();
            }
        }

        private List<User> BatchSaveUsers(int userCount)
        {
            var userList = new List<User>();
            for (var i = 0; i < userCount; i++)
            {
                var user = new User("Zhu" + i);
                //var passport = new Passport{Serial = "1111", User = user};
                //user.Passport = passport;
                userList.Add(user);
            }

            using (var session = GetSession())
            using (var tx = session.BeginTransaction())
            {
                for (var i = 0; i < userList.Count; i++)
                {
                    session.Save(userList[i]);

                    if (i % 25 == 0)
                    {
                        session.Flush();
                        session.Clear();
                    }
                }

                tx.Commit();
            }

            Console.WriteLine("Save Completed!");
            return userList;
        }
    }
}