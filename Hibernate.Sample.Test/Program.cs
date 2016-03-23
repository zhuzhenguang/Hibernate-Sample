using System;
using System.Linq;
using Hibernate.Sample.Test.Common;
using Hibernate.Sample.Test.Domain;
using log4net;
using NHibernate.Linq;

namespace Hibernate.Sample.Test
{
    public class Program : TestBase
    {
        private static ILog logger = LogManager.GetLogger(typeof (Program));

        public static void Main()
        {
            new Program()
                //.TestInsertUser();
                //.TestInsertUserPassport();
                //.TestInsertUserAddress();
                //.TestInsertUserAddressTwoWays();
                //.TestItem3();
                //.TestUserWithResume();
                //.TestUserWithDifferentFetchMethod();
                //.TesteUserWithBatchLoading();
                //.TestPersistent();
                //.TestDetached();
                //.TestPersistenceException();
                //.TestInnerJoinWithHql();
                //.TestInnerJoinWithIQueryable();
                //.TestInnerJoinWithLinq();
                //.TestLeftJoinWithHql();
                //.TestLeftJoinWithIQueryable();
                .TestRightJoinWithHql();

            Console.ReadLine();
        }

        private void TestInsertUser()
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
        }

        private void TestInsertUserPassport()
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
            logger.Info(user2.Passport.Serial);
            logger.Info(user2.Passport.Expiry);
        }

        private void TestInsertUserGroup()
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

            GetSession().Get<User>(user.Id);
        }

        private void TestInsertUserAddress()
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
        }

        private void TestInsertUserAddressTwoWays()
        {
            DeleteAllTalbes();

            var user = new User("Zhu");
            var address = new Address {ZipCode = "100101", AddressDetail = "Beijing Dongzhimen", User = user};
            user.AddAddress(address);

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

            var book = new Book1 {Manufacturer = "a press", Name = "ring load", PageCount = 200};
            var dvd = new Dvd1 {Manufacturer = "b press", Name = "sonata", RegionCode = "10220303"};

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

            var book = new Book2 {Manufacturer = "a press", Name = "ring load", PageCount = 200};
            var dvd = new Dvd3 {Manufacturer = "b press", Name = "sonata", RegionCode = "10220303"};

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

            var book = new Book3 {Manufacturer = "a press", Name = "ring load", PageCount = 200};
            var dvd = new Dvd3 {Manufacturer = "b press", Name = "sonata", RegionCode = "10220303"};

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

        private void TestUserWithResume()
        {
            DeleteAllTalbes();

            var user = new UserWithResume("Zhu") {Resume = "My name is Zhu."};

            var session = GetSession();
            using (var transaction = session.BeginTransaction())
            {
                session.Save(user);
                transaction.Commit();
            }

            session.Query<UserWithResume>().ToList();
            session.Query<object>().ToList();
        }

        // Immediate Load
        // Lazy Load
        // Eager Load
        private void TestUserWithDifferentFetchMethod()
        {
            DeleteAllTalbes();

            var user = new User("Zhu");
            var address = new Address {ZipCode = "100101", AddressDetail = "Beijing Dongzhimen", User = user};
            user.AddAddress(address);

            var session = GetSession();
            using (var transaction = session.BeginTransaction())
            {
                session.Save(user);
                transaction.Commit();
            }

            var searchedUser = GetSession().Get<User>(user.Id);
            Console.WriteLine("Query finished..........");
            Console.WriteLine("Last name: {0}", searchedUser.Name.LastName);
            var addresses = searchedUser.Contact.Addresses;
            foreach (var address1 in addresses)
            {
                Console.WriteLine("Address: {0}", address1.AddressDetail);
            }
        }

        // Batch Load
        private void TesteUserWithBatchLoading()
        {
            DeleteAllTalbes();

            var user1 = new User("Zhu");
            var user2 = new User("ZhuZhu");
            var address1 = new Address {ZipCode = "100101", AddressDetail = "Beijing Dongzhimen", User = user1};
            var address2 = new Address {ZipCode = "100102", AddressDetail = "Beijing Xizhimen", User = user1};
            user1.AddAddress(address1);
            user2.AddAddress(address2);

            var session = GetSession();
            using (var transaction = session.BeginTransaction())
            {
                session.Save(user1);
                session.Save(user2);
                transaction.Commit();
            }

            var searchedUsers = GetSession().Query<User>().ToList();
            foreach (var searchedUser in searchedUsers)
            {
                var addresses = searchedUser.Contact.Addresses;
                foreach (var address in addresses)
                {
                    Console.WriteLine("Address: {0}", address.AddressDetail);
                }
            }
        }

        private void TestTransient()
        {
            var user = new User("Zhu");
        }

        private void TestPersistent()
        {
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
        }

        // if id is null
        // if id is unsaved-value
        // if version is null
        // if version is unsaved-value
        // interceptor.isUnsaved
        private void TestDetached()
        {
            var user = new User("Zhu");

            var session1 = GetSession();
            using (var tx1 = session1.BeginTransaction())
            {
                session1.Save(user);
                tx1.Commit();
            }

            session1.Close();

            var session2 = GetSession();
            using (var tx2 = session2.BeginTransaction())
            {
                session2.Save(user);
                tx2.Commit();
            }
        }

        private void TestPersistenceException()
        {
            DeleteAllTalbes();

            var user2 = new User2 {Id = 1, Name = "Zhu"};

            var session = GetSession();
            using (var tx = session.BeginTransaction())
            {
                session.Save(user2);
                tx.Commit();
            }

            using (var tx = session.BeginTransaction())
            {
                session.Update(new User2 {Id = user2.Id, Name = user2.Name});
                tx.Commit();
            }
        }

        private void TestInnerJoinWithHql()
        {
            DeleteAllTalbes();

            PrepareUserAddressData();

            var session = GetSession();
            var users =
                session.CreateQuery("from User user inner join fetch user.Contact.Addresses")
                    .List<User>()
                    .Distinct()
                    .ToList();
        }

        // WorkBalanceController.QueryTaskAssignmentsByCommonRequest()
        private void TestInnerJoinWithIQueryable()
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
                    .Distinct()
                    .ToList();
        }

        private void TestInnerJoinWithLinq()
        {
            DeleteAllTalbes();

            PrepareUserAddressData();

            var session = GetSession();
            var users = session.Query<User>().Where(user => user.Contact.Addresses.Count > 0).ToList();
        }

        private void TestLeftJoinWithHql()
        {
            DeleteAllTalbes();

            PrepareUserAddressData();

            var session = GetSession();
            var users = session.CreateQuery("from User user left join fetch user.Contact.Addresses").List<User>();
        }

        private void TestLeftJoinWithIQueryable()
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
                    (user, addressList) => new { User = user, Addresses = addressList })
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
        }

        private void TestRightJoinWithHql()
        {
            DeleteAllTalbes();

            PrepareUserAddressData();

            var session = GetSession();
            var users = session.CreateQuery("from User user right join fetch user.Contact.Addresses").List<User>();
        }

        private void PrepareUserAddressData()
        {
            Console.WriteLine("=========================insert data start=========================");
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
            Console.WriteLine("=========================insert data end=========================");
            Console.WriteLine();
        }
    }
}