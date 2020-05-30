using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MelcoreFramework.MessageBroker
{
    public interface IMessageBroker : IDisposable
    {
        #region Public Methods

        Task Close();

        Task CreateConnection(string hostName);

        Task CreateQueue(string name, bool durable, bool exclusive, bool autoDelete);

        Task Open();

        Task SendData<T>(string queue, T data, string exchange, string contentType, byte deliveryMode, string expiration, Dictionary<string, object> headers);

        Task SetReadData(Action<string, ulong, bool, string, string, string, ReadOnlyMemory<byte>> action);

        #endregion Public Methods
    }
}