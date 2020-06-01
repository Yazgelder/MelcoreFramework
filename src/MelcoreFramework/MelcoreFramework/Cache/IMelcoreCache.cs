using System.Threading;
using System.Threading.Tasks;

namespace MelcoreFramework.Cache
{
    public interface IMelcoreCache
    {
        #region Public Methods

        /// <summary>
        /// Cache e Veri eklemek için kullanılır
        /// </summary>
        /// <typeparam name="T">Eklenecek objenin generik tipi </typeparam>
        /// <param name="key">Sistemde kayıtlanacak takma adı </param>
        /// <param name="data">Sistemde tutulacak veri</param>
        /// <param name="timeOut">Sistemde tutulacak dakika süresi</param>
        /// <returns>task</returns>
        Task Add<T>(string key, T data, int timeOut, CancellationToken token = default(CancellationToken));
 

        /// <summary>
        /// verilen tip ve keye göre datayı getirir
        /// </summary>
        /// <typeparam name="T">Almak istenilen verinin tipi</typeparam>
        /// <param name="key">Almak istenen verinin takma adı</param>
        /// <returns>İstenen veri objesi</returns>
        Task<T> Get<T>(string key, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Cache içersinde verilen takma adına göre kaydı siler
        /// </summary>
        /// <param name="key">Silinmesi istenen verinin takma adı</param>
        /// <returns>task</returns>
        Task Remove(string key, CancellationToken token = default(CancellationToken));
 

        #endregion Public Methods
    }
}