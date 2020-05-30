using System.Collections.Generic;
using System.Threading.Tasks;

namespace MelcoreFramework.Search
{
    public interface ISearch
    {
        #region Public Methods

        Task<T> Get<T>(string key);

        Task Register<T>(string key, List<T> value);

        #endregion Public Methods
    }
}