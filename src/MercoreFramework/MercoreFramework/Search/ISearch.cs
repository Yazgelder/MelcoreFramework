using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MercoreFramework.Search
{
   public interface ISearch
    {
        Task Register<T>(string key, List<T> value);
        Task<T> Get<T>(string key);
    }
}
