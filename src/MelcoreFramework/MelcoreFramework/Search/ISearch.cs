using System.Collections.Generic;
using System.Threading.Tasks;

namespace MelcoreFramework.Search
{
    public interface ISearch
    {
        Task Register<T>(string key, List<T> value);

        Task<T> Get<T>(string key);
    }
}