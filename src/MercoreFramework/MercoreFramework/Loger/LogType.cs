using System;
using System.Collections.Generic;
using System.Text;

namespace MercoreFramework.Loger
{
    public enum LogType:byte
    {
        None=0,
        AuditLog=1,
        ErrorLog=2,
        Validation=3,
        LoginLog=4

    }
}
