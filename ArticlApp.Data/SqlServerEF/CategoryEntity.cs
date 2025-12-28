using ArticlApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArticlApp.Data.SqlServerEF
{
    public class CategoryEntity : IDataHelper<Category>
    {
        private ApplicationDbContext db;
        private Category _table;
        public CategoryEntity(ApplicationDbContext applicationDb)
        {
            db = applicationDb;
            _table = new Category();
        }
        public int Add(Category table)
        {
            if (db.Database.CanConnect())
            {
                db.Categories.Add(table);
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
                db.Categories.Remove(_table);
                db.SaveChanges();
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int Edit(int Id, Category updatedCategory)
        {
            if (!db.Database.CanConnect())
                return 0;

            var existingCategory = db.Categories.Find(Id);
            if (existingCategory == null)
                return 0;

            // Copy updated values to tracked entity
            db.Entry(existingCategory).CurrentValues.SetValues(updatedCategory);

            db.SaveChanges();
            return 1;
        }

        public Category Find(int Id)
        {
            if (db.Database.CanConnect())
            {
                return db.Categories.Where(x => x.Id == Id).First();
            }
            else
            {
                throw new Exception("Error database connection");
            }
        }

        public List<Category> GetAllData()
        {
            if (db.Database.CanConnect())
            {
                return db.Categories.ToList();
            }
            else
            {
                throw new Exception("Error database connection");
            }
        }

        public List<Category> GetDataByUser(string UserId)
        {
            throw new NotImplementedException();
        }

        public List<Category> Search(string SerachItem)
        {
            if (db.Database.CanConnect())
            {
                return db.Categories.Where(x => x.Name.Contains(SerachItem)
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
