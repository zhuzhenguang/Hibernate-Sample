using System.IO;
using Hibernate.Sample.Test.Domain;
using NHibernate;
using NHibernate.Cfg;

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
            var tables = new[] {"Passport", "Address", "User"};

            var session = GetSession();
            using (var transaction = session.BeginTransaction())
            {
                foreach (var table in tables)
                {
                    var query = session.CreateQuery("Delete from " + table);
                    query.ExecuteUpdate();    
                }
                transaction.Commit();
            }
        }

        protected ISession GetSession()
        {
            return _sessionFactory.OpenSession();
        }
    }
}