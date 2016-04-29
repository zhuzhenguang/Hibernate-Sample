using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hibernate.Sample.Test.Common;
using Hibernate.Sample.Test.Domain;
using log4net;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace Hibernate.Sample.Test
{
    public class Program : TestBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (Program));

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
                //.TestUserWithBatchLoading();
                //.TestPersistent();
                //.TestDetached();
                //.TestPersistenceException();
                //.TestInnerJoinWithHql();
                //.TestInnerJoinWithIQueryable();
                //.TestInnerJoinWithLinq();
                //.TestLeftJoinWithHql();
                //.TestLeftJoinWithIQueryable();
                //.TestRightJoinWithHql();
                //.TestFirstLevelSessionCache();
                //.TestSecondLevelCache();
                //.TestReadCommitted();
                //.TestRepeatableRead();
                //.TestSerializable();
                //.TestPessimisticLock();
                //.TestOptimisticLock();
                //.TestListUser();
                //.TestEnumerableUser();
                //.TestEnumeralbeUserAfterList();
                //.TestQueryUserByLinq();
                //.TestQueryCache();
                //.TestLazyLoadForEntity();
                //.TestThrowExceptionWhenLazyLoad();
                //.TestLazyLoadForCollection();
                //.TestLazyLoadForCache();
                //.TestLazyLoadForFetch();
                //.TestLazyLoadForProperty();
                //.TestSave();
                //.TestUpdate();
                //.TestMerge();
                //.TestPersist();
                //.TestLock();
                //.TestBatchCreate();
                .TestBatchDelete();

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
            Logger.Info(user2.Passport.Serial);
            Logger.Info(user2.Passport.Expiry);
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
        private void TestUserWithBatchLoading()
        {
            DeleteAllTalbes();

            var user1 = new User("Zhu");
            var user2 = new User("ZhuZhu");
            var address1 = new Address {ZipCode = "100101", AddressDetail = "Beijing Dongzhimen", User = user1};
            var address2 = new Address {ZipCode = "100102", AddressDetail = "Beijing Xizhimen", User = user2};
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

            PrepareUserAddressDataForJoin();

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

            PrepareUserAddressDataForJoin();

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

            PrepareUserAddressDataForJoin();

            var session = GetSession();
            var users = session.Query<User>().Where(user => user.Contact.Addresses.Count > 0).ToList();
        }

        private void TestLeftJoinWithHql()
        {
            DeleteAllTalbes();

            PrepareUserAddressDataForJoin();

            var session = GetSession();
            var users = session.CreateQuery("from User user left join fetch user.Contact.Addresses").List<User>();
        }

        private void TestLeftJoinWithIQueryable()
        {
            DeleteAllTalbes();

            PrepareUserAddressDataForJoin();

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
        }

        private void TestRightJoinWithHql()
        {
            DeleteAllTalbes();

            PrepareUserAddressDataForJoin();

            var session = GetSession();
            var users = session.CreateQuery("from User user right join fetch user.Contact.Addresses").List<User>();
        }

        private void TestFirstLevelSessionCache()
        {
            DeleteAllTalbes();
            PrepareUser2();

            using (var session = GetSession())
            {
                var user1 = session.Get<User2>(1L);
                var user2 = session.Get<User2>(1L);
                session.CreateCriteria<User2>().Add(new InExpression("Id", new object[] {1L})).List<User2>();

                Console.WriteLine("===================================");

                var users1 = session.CreateQuery("from User2").List<User2>();
                var users2 = session.CreateQuery("from User2").List<User2>();
            }

            using (var session = GetSession())
            {
                var user1 = session.Get<User2>(1L);
                session.Evict(user1);
                var user2 = session.Get<User2>(1L);
            }
        }

        private void TestSecondLevelCache()
        {
            DeleteAllTalbes();
            PrepareUser2();

            using (var session = GetSession())
            {
                var user1 = session.Get<User2>(1L);
            }

            using (var session = GetSession())
            {
                var user1 = session.Get<User2>(1L);
            }
        }

        // ReadUncommitted -> ReadCommitted(sqlserver default) RepeatableRead Serializable
        private void TestReadCommitted()
        {
            DeleteAllTalbes();

            new Task(() =>
            {
                Thread.Sleep(1000);
                var session2 = GetSession();
                var tx2 = session2.BeginTransaction();
                var user = session2.Query<User2>().ToList();
                tx2.Commit();
                Console.WriteLine("user's count is: " + user.Count);
            }).Start();

            new Task(() =>
            {
                var session1 = GetSession();
                var tx1 = session1.BeginTransaction();

                session1.Save(new User2 {Id = 1, Name = "Zhu"});
                session1.Flush();

                Thread.Sleep(2000);
                Console.WriteLine("another transaction save completed");
                tx1.Rollback();
            }).Start();
        }

        private void TestRepeatableRead()
        {
            DeleteAllTalbes();
            PrepareUser2();

            new Task(() =>
            {
                Thread.Sleep(1000);
                var session2 = GetSession();
                using (var tx2 = session2.BeginTransaction())
                {
                    var user2 = session2.Get<User2>(1L);
                    user2.Name = "Jiao";
                    session2.Update(user2);
                    tx2.Commit();
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId + ": " + user2.Name);
                }
            }).Start();

            new Task(() =>
            {
                var session1 = GetSession();
                var tx1 = session1.BeginTransaction();

                var user1 = session1.Get<User2>(1L);
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId + ": " + user1.Name);
                session1.Clear();

                Thread.Sleep(3000);

                var user3 = session1.Get<User2>(1L);
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId + ": " + user3.Name);

                tx1.Commit();
            }).Start();
        }

        private void TestSerializable()
        {
            DeleteAllTalbes();

            new Task(() =>
            {
                Thread.Sleep(1000);
                var session2 = GetSession();
                using (var tx2 = session2.BeginTransaction())
                {
                    session2.Save(new User2 {Id = 1, Name = "Zhu"});
                    tx2.Commit();
                }
            }).Start();

            new Task(() =>
            {
                var session1 = GetSession();
                var tx1 = session1.BeginTransaction();

                var users1 = session1.Query<User2>().ToList();
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId + ": " + users1.Count);
                session1.Clear();

                Thread.Sleep(3000);

                var users3 = session1.Query<User2>().ToList();
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId + ": " + users3.Count);

                tx1.Commit();
            }).Start();
        }

        private void TestPessimisticLock()
        {
            DeleteAllTalbes();
            PrepareUser2();

            new Task(() =>
            {
                var session1 = GetSession();
                var tx1 = session1.BeginTransaction();

                var user1 = session1.CreateQuery("from User2 user where user.Id = :id")
                    .SetParameter("id", 1L)
                    .SetLockMode("user", LockMode.Upgrade)
                    .UniqueResult<User2>();

                Thread.Sleep(3000);

                Console.WriteLine(Thread.CurrentThread.ManagedThreadId + ": " + user1.Name);
                tx1.Commit();
            }).Start();

            new Task(() =>
            {
                Thread.Sleep(1000);
                var session1 = GetSession();
                using (var tx1 = session1.BeginTransaction())
                {
                    var user2 = session1.Get<User2>(1L);
                    user2.Name = "Jiao";
                    session1.Update(user2);
                    tx1.Commit();
                }
                Console.WriteLine("update completed!");
            }).Start();
        }

        private void TestOptimisticLock()
        {
            DeleteAllTalbes();
            PrepareUser2();

            new Task(() =>
            {
                var session1 = GetSession();
                using (var tx = session1.BeginTransaction())
                {
                    var user1 = session1.Get<User2>(1L);
                    Thread.Sleep(1000);
                    user1.Name = "Jiao";
                    tx.Commit();
                }
                
            }).Start();

            new Task(() =>
            {
                var session1 = GetSession();
                using (var tx = session1.BeginTransaction())
                {
                    var user1 = session1.Get<User2>(1L);
                    Thread.Sleep(1000);
                    user1.Name = "ZhuZhu";
                    tx.Commit();
                }
            }).Start();
        }

        private void TestListUser()
        {
            DeleteAllTalbes();
            PrepareUser2S();

            using (var session = GetSession())
            {
                var users = session.CreateQuery("from User2 where Name like 'Z%'").List<User2>();
            }    
        }

        private void TestEnumerableUser()
        {
            DeleteAllTalbes();
            PrepareUser2S();

            using (var session = GetSession())
            {
                var users = session.CreateQuery("from User2 where Name like 'Z%'").Enumerable<User2>();
                foreach (var user2 in users)
                {
                    Console.WriteLine(user2.Name);
                }
            }
        }

        private void TestEnumeralbeUserAfterList()
        {
            DeleteAllTalbes();
            PrepareUser2S();

            using (var session = GetSession())
            {
                var users1 = session.CreateQuery("from User2 where Name like 'Z%'").List<User2>();
                var users2 = session.CreateQuery("from User2 where Name like 'Z%'").Enumerable<User2>();

                foreach (var user2 in users2)
                {
                    Console.WriteLine(user2.Name);
                }
            }
        }

        private void TestQueryUserByLinq()
        {
            DeleteAllTalbes();
            PrepareUser2S();

            using (var session = GetSession())
            {
                var user = session.Query<User2>().Where(u => u.Name.StartsWith("Z")).ToList();
                var user2 = session.Query<User2>().Where(u => u.Name.StartsWith("Z")).ToList();
            }
        }

        private void ResolvePerformaceProblemUsingEnumerable()
        {
            DeleteAllTalbes();
            PrepareUser2S();

            using (var session = GetSession())
            {
                var users2 = session.CreateQuery("from User2 where Name like 'Z%'").Enumerable<User2>();

                foreach (var user2 in users2)
                {
                    session.Evict(user2);
                    GetSessionFactory().Evict(typeof(User2), user2.Id);

                    Console.WriteLine(user2.Name);
                }
            }
        }

        private void TestQueryCache()
        {
            DeleteAllTalbes();
            PrepareUser2();

            using (var session = GetSession())
            {
                var query = session.CreateQuery("from User2 where Name like 'Z%'");
                query.SetCacheable(true);

                var users1 = query.List<User2>();
                foreach (var user in users1)
                {
                    Console.WriteLine(user.Name);
                }

                Console.WriteLine("Second Query...");

                var query2 = session.CreateQuery("from User2 where Name like 'Z%'");
                query2.SetCacheable(true);

                var users2 = query2.List<User2>();
                foreach (var user in users2)
                {
                    Console.WriteLine(user.Name);
                }
            }
        }

        private void TestLazyLoadForEntity()
        {
            DeleteAllTalbes();
            PrepareUser2S();

            using (var session = GetSession())
            {
                var user2 = session.Load<User2>(1L);
                Console.WriteLine("load completed");
                Console.WriteLine(user2.Name);
            }
        }

        private void TestThrowExceptionWhenLazyLoad()
        {
            DeleteAllTalbes();
            PrepareUser2S();

            using (var session = GetSession())
            {
                var user2 = session.Load<User2>(1L);

                new Task(() =>
                {
                    Console.WriteLine(user2.Name);
                }).Start();
            }
        }

        private void TestLazyLoadForCollection()
        {
            DeleteAllTalbes();
            PrepareUserAddress();

            ISet<Address> addresses;
            using (var session = GetSession())
            {
                var user = session.Query<User>().Single(u => u.Name.LastName == "Zhu");
                Console.WriteLine(user.Name);
                addresses = user.Contact.Addresses;
                Console.WriteLine(addresses.GetType().FullName);
            }

            var addressArray = addresses.ToArray();
        }

        private void TestLazyLoadForFetch()
        {
            DeleteAllTalbes();
            PrepareUserAddress();

            User user;
            using (var session = GetSession())
            {
                /*user = session.CreateQuery("from User where Name.LastName = 'Zhu'").UniqueResult<User>();
                Console.WriteLine(user.Name);
                NHibernateUtil.Initialize(user.Contact.Addresses);*/

                /*user = session.Query<User>()
                    .Where(u => u.Name.LastName == "Zhu")
                    .Fetch(u => u.Contact).ThenFetchMany(c => c.Addresses)
                    .Single();*/

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

            var addresses = user.Contact.Addresses.ToArray();
        }

        private void TestLazyLoadForCache()
        {
            DeleteAllTalbes();
            var userId = PrepareUserAddress();

            using (var session = GetSession())
            {
                var user = session.Load<User>(userId);
                var addresses = user.Contact.Addresses;
                foreach (var address in addresses)
                {
                    Console.WriteLine(address.AddressDetail);
                }
            }

            Console.WriteLine("---------Second Query---------");

            using (var session2 = GetSession())
            {
                var user = session2.Load<User>(userId);
                var addresses = user.Contact.Addresses;
                foreach (var address in addresses)
                {
                    Console.WriteLine(address.AddressDetail);
                }
            }
        }

        private void TestLazyLoadForProperty()
        {
            DeleteAllTalbes();
            PrepareUser2WithResume();

            using (var session = GetSession())
            {
                var user = session.Load<User2>(1L);
                //var user = session.CreateQuery("from User2").UniqueResult<User2>();
                var name = user.Name;
                var resume = user.Resume;
            }
        }

        // #1 find saved object in session
        // #2 liftcycle onSave/validation validate()/interceptor onSave()
        // #3 insert sql
        // #4 set user id
        // #5 set user to session
        // #6 cascade
        private void TestSave()
        {
            DeleteAllTalbes();

            var user = new User2 {Id = 1, Name = "Zhu"};

            using (var session = GetSession())
            using (var tx = session.BeginTransaction())
            {
                session.Save(user);

                Console.WriteLine("save completed {0}", user.Id);
                tx.Commit();
            }

            /*using (var session = GetSession())
            {
                session.Save(user);
                session.Flush();
            }*/
        }

        // #1 find updated object in session
        // #2 initialize object status info, save it in session, detached -> persistent
        private void TestUpdate()
        {
            DeleteAllTalbes();

            var user = new User2 { Id = 1, Name = "Zhu" };

            using (var session = GetSession())
            using (var tx = session.BeginTransaction())
            {
                session.Save(user);

                Console.WriteLine("save completed");
                tx.Commit();
            }

            using (var session = GetSession())
            using (var tx = session.BeginTransaction())
            {
                session.Update(user);
                user.Resume = "I am Zhu";
                tx.Commit();
            }
        }

        private void TestSaveOrUpdate()
        {
            DeleteAllTalbes();

            var user = new User2 {Id = 1, Name = "Zhu"};

            using (var session = GetSession())
            using (var tx = session.BeginTransaction())
            {
                session.SaveOrUpdate(user);

                Console.WriteLine("save completed");
                tx.Commit();
            }
        }

        private void TestMerge()
        {
            DeleteAllTalbes();

            var user = new User2 { Id = 1, Name = "Zhu" };

            using (var session = GetSession())
            using (var tx = session.BeginTransaction())
            {
                session.Save(user);

                Console.WriteLine("save completed");

                //session.Update(new User2 {Id = 1, Name = "Jiao"});
                session.Merge(new User2 {Id = 1, Name = "Jiao"});
                tx.Commit();
            }
        }

        // ?
        private void TestPersist()
        {
            DeleteAllTalbes();

            using (var session = GetSession())
            {
                var user = new User("Zhu");
                session.Persist(user);

                Console.WriteLine("saved. {0}", user.Id);
                
                session.Flush();
            }

            using (var session = GetSession())
            {
                var user = new User("Zhu");
                session.Save(user);

                Console.WriteLine("saved. {0}", user.Id);

                session.Flush();
            }
        }

        private void TestLock()
        {
            DeleteAllTalbes();

            var user = new User2 { Id = 1, Name = "Zhu" };
            using (var session = GetSession())
            {
                session.Save(user);
                session.Flush();
            }

            using (var session = GetSession())
            {
                session.Lock(user, LockMode.None);
                user.Name = "Jiao";
                session.Flush();
            }
        }

        private void TestBatchCreate()
        {
            DeleteAllTalbes();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            BatchSaveUsers(10000);

            var elapsedMilliseconds = stopWatch.ElapsedMilliseconds;
            Console.WriteLine("elapse: {0}", elapsedMilliseconds);

            Console.ReadLine();
        }

        private void BatchSaveUsers(int userCount)
        {
            var userList = new List<User>();
            for (var i = 0; i < userCount; i++)
            {
                var user = new User("Zhu" + i);
                userList.Add(user);
            }

            using (var session = GetSession())
            using (var tx = session.BeginTransaction())
            {
                for (var i = 0; i < userList.Count; i++)
                {
                    session.Save(userList[i]);

                    if (i%25 == 0)
                    {
                        session.Flush();
                        session.Clear();
                    }
                }

                tx.Commit();
            }

            Console.WriteLine("Save Completed!");
        }

        private void TestBatchDelete()
        {
            DeleteAllTalbes();

            BatchSaveUsers(10000);

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            /*using (var session = GetSession())
            using (var tx = session.BeginTransaction())
            {
                session.Delete("from User");
                tx.Commit();
            }*/

            using (var session = GetSession())
            using (var tx = session.BeginTransaction())
            {
                var users = session.CreateQuery("from User").Enumerable<User>();
                foreach (var user in users)
                {
                    session.Delete(user);
                }
                //session.Delete("from User");
                tx.Commit();
            }
            var elapsedMilliseconds = stopWatch.ElapsedMilliseconds;

            Console.WriteLine("elapse: {0}", elapsedMilliseconds);

            Console.ReadLine();
        }

        private void PrepareUser2WithResume()
        {
            var user = new User2 { Id = 1, Name = "Zhu", Resume = "I am Zhu." };

            using (var session = GetSession())
            using (var tx = session.BeginTransaction())
            {
                session.Save(user);
                tx.Commit();
            }
        }

        private void PrepareUser2()
        {
            Console.WriteLine("=========================insert data start=========================");
            var user2 = new User2 {Id = 1, Name = "Zhu"};

            using (var session = GetSession())
            using (var tx = session.BeginTransaction())
            {
                session.Save(user2);
                tx.Commit();
            }

            Console.WriteLine("=========================insert data end===========================");
            Console.WriteLine();
        }

        private void PrepareUser2S()
        {
            var session = GetSession();
            using (var tx = session.BeginTransaction())
            {
                session.Save(new User2 { Id = 1, Name = "Zhu" });
                session.Save(new User2 { Id = 2, Name = "ZhuZhu" });
                session.Save(new User2 { Id = 3, Name = "Jiao" });
                tx.Commit();
            }
        }

        private void PrepareUserAddressDataForJoin()
        {
            Console.WriteLine("=========================insert data start=========================");
            var user1 = new User("Zhu");
            user1.AddAddress(new Address {AddressDetail = "Shanghai", User = user1});
            user1.AddAddress(new Address {AddressDetail = "Beijing", User = user1});

            var user2 = new User("Jiao");
            user2.AddAddress(new Address {AddressDetail = "GuangZhou", User = user2});

            var user3 = new User("ZhuZhu");
            var user4 = new User("JiaoJiao");
            var address = new Address {AddressDetail = "Hongkongs"};

            using (var session = GetSession())
            using (var tx = session.BeginTransaction())
            {
                session.Save(user1);
                session.Save(user2);
                session.Save(user3);
                session.Save(user4);
                session.Save(address);
                tx.Commit();
            }
            Console.WriteLine("=========================insert data end===========================");
            Console.WriteLine();
        }

        private long PrepareUserAddress()
        {
            Console.WriteLine("=========================insert data start=========================");

            var user = new User("Zhu");
            user.AddAddress(new Address { AddressDetail = "Shanghai", User = user });
            user.AddAddress(new Address { AddressDetail = "Beijing", User = user });
            user.AddAddress(new Address { AddressDetail = "GuangZhou", User = user });

            using (var session = GetSession())
            using (var tx = session.BeginTransaction())
            {
                
                session.Save(user);
                tx.Commit();
            }

            Console.WriteLine("=========================insert data end===========================");
            Console.WriteLine();
            return user.Id;
        }
    }
}