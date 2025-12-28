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
    public class AuthorPostEntityTests
    {
        [Fact]
        public void AddPost_ShouldAdd()
        {
            var db = DbContextMock.GetInMemoryDB("AddPostDB");
            var entity = new AuthorPostEntity(db);

            var post = new AuthorPost
            {
                Id = 1,
                UserId = "UID1",
                UserName = "User1",
                FullName = "User One",
                PostTitle = "Test Post",
                PostCategory = "C#",
                PostDescription = "Test Desc",
                AddedDate = DateTime.Now,
                PostImageUrl = "img.png"
            };

            entity.Add(post);

            Assert.Single(db.AuthorPosts);
        }

        [Fact]
        public void SearchPost_ShouldReturnFiltered()
        {
            var db = DbContextMock.GetInMemoryDB("SearchPostDB");
            var entity = new AuthorPostEntity(db);

            db.AuthorPosts.Add(new AuthorPost { 
                Id = 1,
                PostTitle = "Learn ASP.NET Core",
                UserId = "UID1",
                UserName = "User1",
                FullName = "User One",
                PostCategory = "C#",
                PostDescription = "Test Desc",
                AddedDate = DateTime.Now,
                PostImageUrl = "img.png"
            });
            db.AuthorPosts.Add(new AuthorPost { 
                Id = 2,
                PostTitle = "Java tutorial",
                UserId = "UID1",
                UserName = "User1",
                FullName = "User One",
                PostCategory = "Java",
                PostDescription = "Test Desc",
                AddedDate = DateTime.Now,
                PostImageUrl = "img.png"
            });
            db.SaveChanges();

            var results = entity.Search("ASP.NET");

            Assert.Single(results);
            Assert.Equal("Learn ASP.NET Core", results.First().PostTitle);
        }
    }
}
