using MassTransit.Courier;
using MassTransit.Net.Courier.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.Courier.Activities
{
    public class ValidateImageActivity : IExecuteActivity<ValidateImageArguments>
    {
        public ValidateImageActivity()
        {

        }
        public async Task<ExecutionResult> Execute(ExecuteContext<ValidateImageArguments> execution)
        {

            if (string.IsNullOrEmpty(execution.Arguments.ImagePath))
            {
                // regular termination
                //return execution.Terminate();

                // terminate and include additional variables in the event
                return execution.Terminate(new { Reason = "Not a good time, dude." });
            }

            return execution.Completed();
            
        }
    }
}
