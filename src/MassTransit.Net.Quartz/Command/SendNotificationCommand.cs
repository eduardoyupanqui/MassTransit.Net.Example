using MassTransit.Net.Quartz.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.Quartz.Command
{
    public class SendNotificationCommand : SendNotification
    {
        public string EmailAddress { get; set; }
        public string Body { get; set; }
    }
}
