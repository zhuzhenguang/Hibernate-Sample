﻿using System;
using System.Collections.Generic;
using System.Linq;
using Hibernate.Sample.Test.Common;
using Hibernate.Sample.Test.Domain;
using NHibernate;
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
                user = session.CreateQuery("from User where Name.LastName = 'Zhu'").UniqueResult<User>();
                Console.WriteLine(user.Name);
                NHibernateUtil.Initialize(user.Contact.Addresses);

                /*user = session.Query<User>()
                    .Where(u => u.Name.LastName == "Zhu")
                    .Fetch(u => u.Contact.Addresses)
                    .Single();*/
                Console.WriteLine(user.Name);
            }

            var addresses = user.Contact.Addresses;
            Assert.Equal("Shanghai", addresses.First().AddressDetail);
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
                var user = new User("Zhu");
                user.AddAddress(new Address { AddressDetail = "Shanghai", User = user });
                user.AddAddress(new Address { AddressDetail = "Beijing", User = user });
                user.AddAddress(new Address { AddressDetail = "GuangZhou", User = user });
                session.Save(user);
                tx.Commit();
            }

            Console.WriteLine("=========================insert data end===========================");
            Console.WriteLine();
        }
    }
}