using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticlApp.Data.Interfaces
{
    public interface IDataByUserHelper<Table> : IDataHelper<Table>
    {
        List<Table> GetDataByUser(string UserId);
    }
}
