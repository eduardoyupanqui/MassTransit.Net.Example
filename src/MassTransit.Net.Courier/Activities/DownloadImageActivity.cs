using MassTransit.Courier;
using MassTransit.Net.Courier.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.Courier.Activities
{
    public class DownloadImageActivity : IActivity<DownloadImageArguments, DownloadImageLog>
    {
        public DownloadImageActivity()
        {

        }

        public async Task<ExecutionResult> Execute(ExecuteContext<DownloadImageArguments> execution)
        {
            DownloadImageArguments args = execution.Arguments;
            string imageSavePath = Path.Combine(args.WorkPath,
                execution.TrackingNumber.ToString());

            //await _httpClient.GetAndSave(args.ImageUri, imageSavePath);

            //1
            //return execution.Completed<DownloadImageLog>(new { ImageSavePath = imageSavePath });
            //2
            return execution.CompletedWithVariables(new { ImagePath = imageSavePath });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<DownloadImageLog> compensation)
        {
            DownloadImageLog log = compensation.Log;
            File.Delete(log.ImageSavePath);

            return compensation.Compensated();
        }
    }
}
