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
    public class AuthorEntityTests
    {
        [Fact]
        public void Add_ShouldAddAuthor()
        {
            // Arrange
            var db = DbContextMock.GetInMemoryDB("AddAuthorDB");
            var entity = new AuthorEntity(db);

            var author = new Author { FullName = "Mohamed", UserId = "U1", UserName = "MohamedA" };

            // Act
            var result = entity.Add(author);

            // Assert
            Assert.Equal(1, result);
            Assert.Single(db.Authors);
            Assert.Equal("Mohamed", db.Authors.First().FullName);
        }

        [Fact]
        public void Edit_ShouldUpdateAuthor()
        {
            var db = DbContextMock.GetInMemoryDB("EditAuthorDB");
            var entity = new AuthorEntity(db);

            db.Authors.Add(new Author { Id = 1, FullName = "Old Name", UserId = "U1", UserName = "MohamedA" });
            db.SaveChanges();

            var updatedAuthor = new Author { Id = 1, FullName = "New Name" };

            var result = entity.Edit(1, updatedAuthor);

            Assert.Equal(1, result);
            Assert.Equal("New Name", db.Authors.First().FullName);
        }

        [Fact]
        public void Delete_ShouldRemoveAuthor()
        {
            var db = DbContextMock.GetInMemoryDB("DeleteAuthorDB");
            var entity = new AuthorEntity(db);

            db.Authors.Add(new Author { Id = 1, FullName = "To Remove",UserName = "MohamedA", UserId = "U1" });
            db.SaveChanges();

            var result = entity.Delete(1);

            Assert.Equal(1, result);
            Assert.Empty(db.Authors);
        }

        [Fact]
        public void Search_ShouldReturnMatches()
        {
            var db = DbContextMock.GetInMemoryDB("SearchAuthorDB");
            var entity = new AuthorEntity(db);

            db.Authors.Add(new Author { Id = 1, FullName = "Mohamed Ali", UserId = "U1", UserName = "MohamedA" });
            db.Authors.Add(new Author { Id = 2, FullName = "John", UserId = "U1", UserName = "Mohamedb" });
            db.SaveChanges();

            var results = entity.Search("Ali");

            Assert.Single(results);
            Assert.Equal("Mohamed Ali", results.First().FullName);
        }
    }
}
