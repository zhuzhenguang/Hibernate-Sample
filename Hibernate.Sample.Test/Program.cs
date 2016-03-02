using System;
using Hibernate.Sample.Test.Common;
using Hibernate.Sample.Test.Domain;
using log4net;
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
                .TestInsertUserPassport();

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
    }
}