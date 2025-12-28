using ArticlApp.Core;
using ArticlApp.Data.SqlServerEF;
using ArticlApp.UnitTest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticlApp.UnitTest.Services
{
    public class CategoryEntityTests
    {
        [Fact]
        public void AddCategory_ShouldAdd()
        {
            var db = DbContextMock.GetInMemoryDB("AddCategoryDB");
            var entity = new CategoryEntity(db);

            entity.Add(new Category { Name = "Tech" });

            Assert.Single(db.Categories);
        }

        [Fact]
        public void EditCategory_ShouldUpdate()
        {
            // we change the database name in each method to be indpendent and isolated of each other
            var db = DbContextMock.GetInMemoryDB("EditCategoryDB");
            var entity = new CategoryEntity(db);

            db.Categories.Add(new Category { Id = 1, Name = "Old" });
            db.SaveChanges();

            entity.Edit(1, new Category { Id = 1, Name = "New" });

            Assert.Equal("New", db.Categories.First().Name);
        }
    }
}
