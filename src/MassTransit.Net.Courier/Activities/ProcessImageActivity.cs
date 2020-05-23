using MassTransit.Courier;
using MassTransit.Net.Courier.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.Courier.Activities
{
    public class ProcessImageActivity : IExecuteActivity<ProcessImageArguments>
    {
        public async Task<ExecutionResult> Execute(ExecuteContext<ProcessImageArguments> context)
        {
            var path = context.Arguments.ImagePath;
            
            //Process something

            return context.Completed();
        }
    }
}
