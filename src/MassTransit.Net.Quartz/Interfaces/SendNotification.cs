using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.Quartz.Interfaces
{
    public interface SendNotification
    {
        string EmailAddress { get; }
        string Body { get; }
    }
}
