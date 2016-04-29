using System;
using System.IO;
using Hibernate.Sample.Test.Domain;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Util;

namespace Hibernate.Sample.Test.Common
{
    public class TestBase
    {
        private readonly ISessionFactory _sessionFactory;

        public TestBase()
        {
            var configuration =
                new Configuration()
                    .SetDefaultAssembly(typeof (User).Assembly.FullName)
                    .SetDefaultNamespace(typeof (User).Namespace)
                    .AddDirectory(new DirectoryInfo("./Domain/"));

            _sessionFactory = configuration.BuildSessionFactory();
        }

        protected void DeleteAllTalbes()
        {
            var tables = new[]
            {
                "Passport", 
                "Address", 
                "Address2", 
                "User", 
                "Hibernate.Sample.Test.Domain.Group", 
                "Hibernate.Sample.Test.Domain.Role",
                "Book1",
                "Dvd1",
                "Item2",
                "Item3",
                "User2",
                "User3"
            };

            var session = GetSession();
            using (var transaction = session.BeginTransaction())
            {
                DeleteGroup(session);
                foreach (var table in tables)
                {
                    var query = session.CreateQuery("Delete from " + table);
                    query.ExecuteUpdate();    
                }

                transaction.Commit();
            }
            Console.WriteLine("====================");
        }

        private static void DeleteGroup(ISession session)
        {
            var queryGroup = session.CreateQuery("from Hibernate.Sample.Test.Domain.Group");
            var groups = queryGroup.List<Group>();
            if (!groups.Any())
            {
                return;
            }
            foreach (var @group in groups)
            {
                @group.Roles = null;
                session.Save(@group);
                //session.Delete(@group);
            }
            session.Flush();
        }

        protected ISession GetSession()
        {
            return _sessionFactory.OpenSession();
        }

        protected IStatelessSession GetStatelessSession()
        {
            return _sessionFactory.OpenStatelessSession();
        }

        protected ISessionFactory GetSessionFactory()
        {
            return _sessionFactory;
        }
    }
}