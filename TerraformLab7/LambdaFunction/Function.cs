using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.S3;
using Amazon.S3.Model;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LambdaFunction;

public class Function
{
    private static readonly string TargetBucket = "s3-finish";
    private static readonly AmazonS3Client S3 = new(
        "test", "test",
        new AmazonS3Config
        {
            ServiceURL = "http://localhost:4566",
            ForcePathStyle = true
        });

    public async Task FunctionHandler(S3Event evnt, ILambdaContext context)
    {
        var record = evnt.Records![0];
        string srcBucket = record.S3.Bucket.Name;
        string key = record.S3.Object.Key;

        var copyRequest = new CopyObjectRequest
        {
            SourceBucket = srcBucket,
            SourceKey = key,
            DestinationBucket = TargetBucket,
            DestinationKey = key
        };

        await S3.CopyObjectAsync(copyRequest);
        context.Logger.LogLine($"Copied {key} -> {TargetBucket}");
    }
}
