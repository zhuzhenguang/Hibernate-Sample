using Hibernate.Sample.Test.Common;
using Hibernate.Sample.Test.Domain;
using Xunit;

namespace Hibernate.Sample.Test.DomainTests
{
    public class GroupRoleFacts: TestBase
    {
        [Fact]
        public void should_save_group_role()
        {
            DeleteAllTalbes();

            var role1 = new Role {Name = "Role1"};
            var role2 = new Role {Name = "Role2"};
            var role3 = new Role {Name = "Role3"};

            var group1 = new Group{Name = "Group1"};
            var group2 = new Group{Name = "Group2"};
            var group3 = new Group{Name = "Group3"};

            group1.Roles.Add(role1);
            group1.Roles.Add(role2);

            group2.Roles.Add(role2);
            group2.Roles.Add(role3);

            group3.Roles.Add(role1);
            group3.Roles.Add(role3);

            role1.Groups.Add(group1);
            role1.Groups.Add(group3);

            role2.Groups.Add(group1);
            role2.Groups.Add(group2);

            role3.Groups.Add(group2);
            role3.Groups.Add(group3);

            var session = GetSession();
            using (var transaction = session.BeginTransaction())
            {
                session.Save(role1);
                session.Save(role2);
                session.Save(role3);
                
                session.Save(group1);
                session.Save(group2);
                session.Save(group3);

                transaction.Commit();
            }

            Assert.True(role1.Id > 0);
            Assert.True(role2.Id > 0);
            Assert.True(role3.Id > 0);

            Assert.True(group1.Id > 0);
            Assert.True(group2.Id > 0);
            Assert.True(group3.Id > 0);
        }

        [Fact]
        public void should_delete()
        {
            DeleteAllTalbes();
        }
    }
}