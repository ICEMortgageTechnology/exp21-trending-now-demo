using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMT.DeveloperConnect.API.Client.Http
{
    public enum RequestStatus
    {
        None = 0,
        Completed = 1,
        Failed = 3,
        Error = 4,
        Aborted = 5
    }
}
