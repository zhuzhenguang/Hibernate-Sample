using Hibernate.Sample.Test.Common;
using Hibernate.Sample.Test.Domain;
using Xunit;

namespace Hibernate.Sample.Test.DomainTests
{
    public class UserFacts : TestBase
    {
        [Fact]
        public void should_save_user()
        {
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

            Assert.True(user.Id > 0);
        }
    }
}