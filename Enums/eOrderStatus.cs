using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FieldOrdersAPI.Enums
{
    public enum eOrderStatus
    {
        eInProgress = 1,
        eOnHold,
        eCancelled,
        eClosed,
        eReplaced,
    }
}
