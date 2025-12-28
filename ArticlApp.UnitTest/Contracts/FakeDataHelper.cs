using ArticlApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticlApp.UnitTest.Contracts
{
    public class FakeDataHelper<T> : IDataHelper<T> where T : class
    {
        public List<T> data = new List<T>();

        public int Add(T table) { data.Add(table); return 1; }
        public int Delete(int Id)
        {
            var item = Find(Id);
            data.Remove(item);
            return 1;
        }
        public int Edit(int id, T table)
        {
            Delete(id);
            Add(table);
            return 1;
        }
        public T Find(int id)
        {
            return data.FirstOrDefault(x => (int)x.GetType().GetProperty("Id").GetValue(x) == id);
        }
        public List<T> GetAllData() => data;
        public List<T> GetDataByUser(string UserId) => data;
        public List<T> Search(string searchItem)
        {
            return data.Where(x => x.ToString().Contains(searchItem)).ToList();
        }
    }
}
