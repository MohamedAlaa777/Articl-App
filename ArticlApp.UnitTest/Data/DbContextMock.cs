using ArticlApp.Data.SqlServerEF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticlApp.UnitTest.Data
{
    public static class DbContextMock
    {
        public static ApplicationDbContext GetInMemoryDB(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}
