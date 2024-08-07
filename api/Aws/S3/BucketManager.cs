using Aws.S3.Models;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace Aws.S3.BucketManager;

public class BucketManager
{
    private static IAmazonS3 s3Client;

    public BucketManager()
    {
        var config = new AmazonS3Config { ServiceURL = "http://s3.localhost.localstack.cloud:4566", ForcePathStyle = true };
        s3Client = new AmazonS3Client(config);
    }

    public async Task<bool> CreateBucketAsync(BucketRequestModel bucketRequest)
    {
        try
        {
            var request = new PutBucketRequest
            {
                BucketName = bucketRequest.Name,
                UseClientRegion = true,

            };

            var response = await s3Client.PutBucketAsync(request);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;

        }
        catch (AmazonS3Exception ex)
        {
            Console.WriteLine($"Error creating bucket: {ex.Message}");
            return false;
        }
    }


    public async Task<bool> DeleteBucketAsync(string bucketName)
    {
        var request = new DeleteBucketRequest
        {
            BucketName = bucketName,
            UseClientRegion = true,

        };

        var response = await s3Client.DeleteBucketAsync(request);
        Console.WriteLine(response.ToString());
        return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
    }


}
