using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MercoreFramework.Loger
{
   public interface IMelcoreLog
    {

        Task AddLog(LogType type,string title, string log);
        Task ClearLog(DateTime startDate, DateTime endDate, LogType type);
        Task<string> GetLog(DateTime startDate, DateTime endDate, LogType type);

    }
}
