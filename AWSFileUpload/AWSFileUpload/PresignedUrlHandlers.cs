using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.S3;
using Amazon.S3.Model;
using System.IO;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWSFileUpload
{
    public class PresignedUrlHandlers
    {

        private IAmazonS3 s3Client;

        /// <summary>
        /// Default constructor that Lambda will invoke.
        /// </summary>
        public PresignedUrlHandlers()
        {
            s3Client = new AmazonS3Client();
        }


        /// <summary>
        /// A Lambda function to respond to HTTP Get methods from API Gateway
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The API Gateway response.</returns>
        public APIGatewayProxyResponse GetUploadPresignedUrl(APIGatewayProxyRequest request, ILambdaContext context)
        {
            context.Logger.LogLine("Get Request\n");

            string bucket = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("UploadBucket")) ? "default_bucket" : Environment.GetEnvironmentVariable("UploadBucket");

            string key = string.IsNullOrEmpty(request.QueryStringParameters["name"]) ? "unknown.jpg" : request.QueryStringParameters["name"];

            string presignedurl = GeneratePreSignedURL(120, bucket, key);
            context.Logger.LogLine(presignedurl);

            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = "{ \"uploadURL\": \"" + presignedurl + "\" }",
                Headers = new Dictionary<string, string> {
                    {
                        "Content-Type", "application/json"
                    },
                    {
                        "Access-Control-Allow-Methods", "GET"
                    },
                    {
                        "Access-Control-Allow-Origin", "*"
                    }
                }
            };

            return response;
        }

        private string GeneratePreSignedURL(double duration, string bucket, string key)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = bucket,
                Key = Path.Combine("upload", key),
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddSeconds(duration),
                ContentType = "image/jpeg"
            };

            string url = s3Client.GetPreSignedURL(request);
            return url;
        }


    }
}
