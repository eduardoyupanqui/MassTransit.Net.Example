using MassTransit.Net.Jobs.Client.Commands;
using MassTransit.Net.Jobs.Client.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MassTransit.Net.Jobs.Client
{
    public delegate Task AsyncEventHandler<in TEvent>(object sender, TEvent @event) where TEvent : System.EventArgs;
    public interface IExecutor
    {
        event AsyncEventHandler<ExecutorStartEventArgs> ProcessStarted;
        event AsyncEventHandler<ExecutorTaskEventArgs> StatusTarea;
        event AsyncEventHandler<ExecutorCompleteEventArgs> ProcessCompleted;
        event AsyncEventHandler<ExecutorFailEventArgs> ProcessFailed;
        Task Execute(JobCommand command);
    }
}
