using ArticlApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArticlApp.Data.SqlServerEF
{
    public class AuthorEntity : IDataHelper<Author>
    {
        private ApplicationDbContext db;
        private Author _table;
        public AuthorEntity(ApplicationDbContext applicationDb)
        {
            db = applicationDb;
            _table = new Author();
        }
        public int Add(Author table)
        {
            if (db.Database.CanConnect())
            {
                db.Authors.Add(table);
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
                _table = Find(Id);
                db.Authors.Remove(_table);
                db.SaveChanges();
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int Edit(int Id, Author updatedAuthor)
        {
            if (!db.Database.CanConnect())
                return 0;

            var existingAuthor = db.Authors.Find(Id);
            if (existingAuthor == null)
                return 0;

            // Copy updated values into the tracked entity
            db.Entry(existingAuthor).CurrentValues.SetValues(updatedAuthor);

            db.SaveChanges();
            return 1;
        }


        public Author Find(int Id)
        {
            if (db.Database.CanConnect())
            {
                return db.Authors.Where(x => x.Id == Id).First();
            }
            else
            {
                 throw new Exception("Error database connection"); ;
            }
        }

        public List<Author> GetAllData()
        {
            if (db.Database.CanConnect())
            {
                return db.Authors.ToList();
            }
            else
            {
                throw new Exception("Error database connection");
            }
        }

        public List<Author> Search(string SerachItem)
        {
            if (db.Database.CanConnect())
            {
                return db.Authors.Where(
                   x => x.FullName.Contains(SerachItem)
                   || x.UserId.Contains(SerachItem)
                   || x.Bio.Contains(SerachItem)
                   || x.UserName.Contains(SerachItem)
                   || x.Facbook.Contains(SerachItem)
                   || x.Twitter.Contains(SerachItem)
                   || x.Instagram.Contains(SerachItem)
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
