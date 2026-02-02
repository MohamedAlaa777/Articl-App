using ArticlApp.Core;
using ArticlApp.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArticlApp.Data.SqlServerEF
{
    public class AuthorPostEntity : IDataByUserHelper<AuthorPost>
    {
        private ApplicationDbContext db;
        public AuthorPostEntity(ApplicationDbContext applicationDb)
        {
            db = applicationDb;
        }
        public int Add(AuthorPost createdPost)
        {
            if (db.Database.CanConnect())
            {
                db.AuthorPosts.Add(createdPost);
                db.SaveChanges();
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int Delete(int Id)
        {
            if (db.Database.CanConnect())
            {
                var _table = Find(Id);
                db.AuthorPosts.Remove(_table);
                db.SaveChanges();
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int Edit(int Id, AuthorPost updatedPost)
        {
            if (!db.Database.CanConnect())
                return 0;

            var existingPost = db.AuthorPosts.Find(Id);
            if (existingPost == null)
                return 0;

            // Copy updated values to tracked entity
            db.Entry(existingPost).CurrentValues.SetValues(updatedPost);

            db.SaveChanges();
            return 1;
        }

        public AuthorPost Find(int Id)
        {
            if (db.Database.CanConnect())
            {
                return db.AuthorPosts.Where(x => x.Id == Id).First();
            }
            else
            {
                throw new Exception("Error database connection");
            }
        }

        public List<AuthorPost> GetAllData()
        {
            if (db.Database.CanConnect())
            {
                return db.AuthorPosts.ToList();
            }
            else
            {
                throw new Exception("Error database connection");
            }
        }

        public List<AuthorPost> GetDataByUser(string UserId)
        {
            if (db.Database.CanConnect())
            {
                return db.AuthorPosts.Where(x=>x.UserId==UserId).ToList();
            }
            else
            {
                throw new Exception("Error database connection");
            }
        }

        public List<AuthorPost> Search(string SerachItem)
        {
            if (db.Database.CanConnect())
            {
                return db.AuthorPosts.Where(x =>
                x.FullName.Contains(SerachItem)
                ||x.UserId.Contains(SerachItem)
                ||x.UserName.Contains(SerachItem)
                ||x.PostTitle.Contains(SerachItem)
                ||x.PostDescription.Contains(SerachItem)
                ||x.PostImageUrl.Contains(SerachItem)
                ||x.AuthorId.ToString().Contains(SerachItem)
                ||x.CategoryId.ToString().Contains(SerachItem)
                ||x.AddedDate.ToString().Contains(SerachItem)
                || x.Id.ToString().Contains(SerachItem))
                .ToList();
            }
            else
            {
                throw new Exception("Error database connection");
            }
        }
    }
}
