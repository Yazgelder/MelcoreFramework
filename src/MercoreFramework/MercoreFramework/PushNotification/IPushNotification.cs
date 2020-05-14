using System.Collections.Generic;
using System.Threading.Tasks;

namespace MercoreFramework.PushNotification
{
    public interface IPushNotification
    {
        Task PushNotification(string tooken, string title, string body, string data = "", string clickAction = "");

        Task PushNotification(List<string> tookens, string title, string body, string data = "", string clickAction = "");
    }
}