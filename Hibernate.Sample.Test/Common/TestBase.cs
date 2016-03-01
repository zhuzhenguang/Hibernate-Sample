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
                    .SetDefaultNamespace(typeof (User).Namespace);

            _sessionFactory = configuration.BuildSessionFactory();
        }

        protected ISession GetSession()
        {
            return _sessionFactory.OpenSession();
        }
    }
}