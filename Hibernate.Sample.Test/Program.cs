using System;
using System.Linq;
using Hibernate.Sample.Test.Common;
using Hibernate.Sample.Test.Domain;
using log4net;
using NHibernate.Linq;
using Xunit;

namespace Hibernate.Sample.Test
{
    public class Program: TestBase
    {
        private static ILog logger = LogManager.GetLogger(typeof (Program));

        public static void Main()
        {
            new Program()
                //.TestInsertUser();
                //.TestInsertUserPassport();
                //.TestInsertUserAddress();
                //.TestInsertUserAddressTwoWays();
                .TestItem3();

            Console.ReadLine();
        }

        public void TestInsertUser()
        {
            DeleteAllTalbes();

            var user = new User { Name = "Zhu" };

            var session = GetSession();
            var transaction = session.BeginTransaction();

            using (session)
            using (transaction)
            {
                session.Save(user);
                session.Flush();
                transaction.Commit();
            }
        }

        public void TestInsertUserPassport()
        {
            DeleteAllTalbes();

            var user = new User { Name = "Zhu" };
            var passport = new Passport { Serial = "df890890", Expiry = "20190101" };

            user.Passport = passport;
            passport.User = user;

            var session = GetSession();
            using (var transaction = session.BeginTransaction())
            {
                session.Save(user);
                transaction.Commit();
            }

            var user2 = GetSession().Get<User>(user.Id);
            logger.Info(user2.Passport.Serial);
            logger.Info(user2.Passport.Expiry);
        }

        public void TestInsertUserGroup()
        {
            DeleteAllTalbes();

            var user = new User { Name = "Zhu" };
            var group = new Group { Name = "Admin Group" };

            user.Group = group;

            var session = GetSession();
            using (var transaction = session.BeginTransaction())
            {
                session.Save(user);
                transaction.Commit();
            }

            GetSession().Get<User>(user.Id);
        }

        public void TestInsertUserAddress()
        {
            DeleteAllTalbes();

            var user = new User { Name = "Zhu" };
            var address = new Address { ZipCode = "100101", AddressDetail = "Beijing Dongzhimen" };
            user.Addresses.Add(address);

            var session = GetSession();
            using (var transaction = session.BeginTransaction())
            {
                session.Save(user);
                transaction.Commit();
            }

            //GetSession().Get<User>(user.Id);
        }

        public void TestInsertUserAddressTwoWays()
        {
            DeleteAllTalbes();

            var user = new User { Name = "Zhu" };
            var address = new Address { ZipCode = "100101", AddressDetail = "Beijing Dongzhimen", User = user};
            user.Addresses.Add(address);

            var session = GetSession();
            using (var transaction = session.BeginTransaction())
            {
                session.Save(user);

                //address.User = null;
                //user.Addresses.Remove(address);

                //session.Save(user);
                transaction.Commit();
            }
        }

        private void TestItem1()
        {
            DeleteAllTalbes();

            var book = new Book1 { Manufacturer = "a press", Name = "ring load", PageCount = 200 };
            var dvd = new Dvd1 { Manufacturer = "b press", Name = "sonata", RegionCode = "10220303" };

            var session = GetSession();
            using (var tranaction = session.BeginTransaction())
            {
                session.Save(book);
                session.Save(dvd);
                tranaction.Commit();
            }

            session.Query<Item1>().ToList();
        }

        private void TestItem2()
        {
            DeleteAllTalbes();

            var book = new Book2 { Manufacturer = "a press", Name = "ring load", PageCount = 200 };
            var dvd = new Dvd3 { Manufacturer = "b press", Name = "sonata", RegionCode = "10220303" };

            var session = GetSession();
            using (var tranaction = session.BeginTransaction())
            {
                session.Save(book);
                session.Save(dvd);
                tranaction.Commit();
            }

            session.Query<Item2>().ToList();
        }

        private void TestItem3()
        {
            DeleteAllTalbes();

            var book = new Book3 { Manufacturer = "a press", Name = "ring load", PageCount = 200 };
            var dvd = new Dvd3 { Manufacturer = "b press", Name = "sonata", RegionCode = "10220303" };

            var session = GetSession();
            using (var tranaction = session.BeginTransaction())
            {
                session.Save(book);
                session.Save(dvd);
                tranaction.Commit();
            }

            session.Query<Book3>().ToList();
            session.Query<Dvd3>().ToList();
        }
    }
}