using System.Collections.Generic;
using System.Threading.Tasks;

namespace MelcoreFramework.PushNotification
{
    public interface IPushNotification
    {
        #region Public Methods

        Task PushNotification(string token, string title, string body, string data = "", string clickAction = "");

        Task PushNotification(List<string> tokens, string title, string body, string data = "", string clickAction = "");

        #endregion Public Methods
    }
}