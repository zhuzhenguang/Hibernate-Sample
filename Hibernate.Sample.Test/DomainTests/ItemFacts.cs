using System.Linq;
using Hibernate.Sample.Test.Common;
using Hibernate.Sample.Test.Domain;
using NHibernate.Linq;
using Xunit;

namespace Hibernate.Sample.Test.DomainTests
{
    public class ItemFacts : TestBase
    {
        [Fact]
        public void should_query_book1_dvd1()
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

            Assert.True(book.Id > 0);
            Assert.True(dvd.Id > 0);

            var items = session.Query<Item1>().ToList();
            Assert.Equal(2, items.Count);
            Assert.Contains(book, items);
            Assert.Contains(dvd, items);
        }

        [Fact]
        public void should_query_book2_dvd2()
        {
            DeleteAllTalbes();

            var book = new Book2 { Manufacturer = "a press", Name = "ring load", PageCount = 200 };
            var dvd = new Dvd2 { Manufacturer = "b press", Name = "sonata", RegionCode = "10220303" };

            var session = GetSession();
            using (var tranaction = session.BeginTransaction())
            {
                session.Save(book);
                session.Save(dvd);
                tranaction.Commit();
            }

            Assert.True(book.Id > 0);
            Assert.True(dvd.Id > 0);

            var items = session.Query<Item2>().ToList();
            Assert.Equal(2, items.Count);
            Assert.Contains(book, items);
            Assert.Contains(dvd, items);
        }

        [Fact]
        public void should_query_book3_dvd3()
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

            Assert.True(book.Id > 0);
            Assert.True(dvd.Id > 0);

            var items = session.Query<Item3>().ToList();
            Assert.Equal(2, items.Count);
            Assert.Contains(book, items);
            Assert.Contains(dvd, items);
        }
    }
}