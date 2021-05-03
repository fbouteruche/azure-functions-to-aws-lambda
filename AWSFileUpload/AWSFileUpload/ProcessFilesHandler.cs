using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWSFileUpload
{

    public class ProcessFilesHandler
    {
        public string ProcessFileCreatedEvent(S3Event request, ILambdaContext context)
        {
            foreach (S3Event.S3EventNotificationRecord file in request.Records)
            {
                context.Logger.LogLine($"Process file {file.S3.Object.Key}");
            }
            return "ok";
        }
    }
}
