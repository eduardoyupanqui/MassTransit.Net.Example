using MassTransit.Net.Jobs.Client.Commands;
using MassTransit.Net.Jobs.Client.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Net.Jobs.Client
{
    public interface IExecutor
    {
        event EventHandler<ExecutorStartEventArgs> ProcessStarted;
        event EventHandler<ExecutorTaskEventArgs> StatusTarea;
        event EventHandler<ExecutorCompleteEventArgs> ProcessCompleted;
        event EventHandler<ExecutorFailEventArgs> ProcessFailed;
        void Execute(JobCommand command);
    }
}
