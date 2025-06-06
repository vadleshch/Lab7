using Amazon.S3;
using Amazon.S3.Transfer;

namespace Uploader;
class Program
{
    static async Task Main(string[] args)
    {
        const string endpoint = "http://localhost:4566";
        const string bucket = "s3-start";
        string filePath = args.Length > 0 ? args[0] : "sample.txt";

        if (!File.Exists(filePath))
            await File.WriteAllTextAsync(filePath, "Hello!");

        var config = new AmazonS3Config
        {
            ServiceURL = endpoint,
            ForcePathStyle = true
        };

        using var s3 = new AmazonS3Client("test", "test", config);
        var transfer = new TransferUtility(s3);

        await transfer.UploadAsync(filePath, bucket);
        Console.WriteLine($"Uploaded {filePath} to {bucket}");
    }
}
