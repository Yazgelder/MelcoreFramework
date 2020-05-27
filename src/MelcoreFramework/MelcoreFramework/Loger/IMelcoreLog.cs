using System;
using System.Threading.Tasks;

namespace MelcoreFramework.Loger
{
    public interface IMelcoreLog
    {
        Task AddLog(LogType type, string title, string log);

        Task ClearLog(DateTime startDate, DateTime endDate, LogType type);

        Task<string> GetLog(DateTime startDate, DateTime endDate, LogType type);
    }
}