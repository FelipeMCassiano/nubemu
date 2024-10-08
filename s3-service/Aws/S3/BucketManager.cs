using Aws.S3.Models;
using Amazon.S3;
using Amazon.S3.Model;
using System.Text.Json;
using Errors;
using OneOf;

namespace Aws.S3.BucketManager;

public class BucketManager
{
    private static IAmazonS3 s3Client = new AmazonS3Client(new AmazonS3Config { ServiceURL = "http://s3.localhost.localstack.cloud:4566", ForcePathStyle = true });

    public async Task<OneOf<PutBucketResponse, BucketNameConflictError>> CreateBucketAsync(BucketRequestModel bucketRequest)
    {
        var request = new PutBucketRequest
        {
            BucketName = bucketRequest.Name,
            UseClientRegion = true,

        };

        var buckets = await s3Client.ListBucketsAsync();


        if (buckets.Buckets.Exists(x => x.BucketName == bucketRequest.Name))
        {
            return new BucketNameConflictError();
        }

        var response = await s3Client.PutBucketAsync(request);
        return response;

    }

    public async Task<DeleteBucketResponse> DeleteBucketAsync(string bucketName)
    {
        var request = new DeleteBucketRequest
        {
            BucketName = bucketName,
            UseClientRegion = true,

        };

        var response = await s3Client.DeleteBucketAsync(request);
        return response;
    }
    public async Task<PutLifecycleConfigurationResponse> CreateLifeclyeBucket(BucketLifecycleRuleRequestModel lifecycleRuleRequestModel)
    {

        var newRules = new List<LifecycleRule>();

        foreach (var r in lifecycleRuleRequestModel.lifecycleRules)
        {

            LifecycleRuleStatus status;
            status = r.status switch
            {
                LifecycleRulesStatusModel.Enabled => status = LifecycleRuleStatus.Enabled,
                LifecycleRulesStatusModel.Disabled => status = LifecycleRuleStatus.Disabled,
                _ => throw new ArgumentOutOfRangeException(nameof(r.status), "Invalid status value.")
            };

            var rule = new LifecycleRule
            {
                Filter = new LifecycleFilter
                {
                    LifecycleFilterPredicate = new LifecyclePrefixPredicate
                    {
                        Prefix = r.prefix
                    }
                },
                Id = r.id,
                Expiration = new LifecycleRuleExpiration { Days = r.expiration },
                Status = status
            };
            newRules.Add(rule);

        };
        var request = new PutLifecycleConfigurationRequest
        {
            BucketName = lifecycleRuleRequestModel.bucketName,
            Configuration = new LifecycleConfiguration
            {
                Rules = newRules
            }
        };

        var response = await s3Client.PutLifecycleConfigurationAsync(request);

        return response;
    }
    public async Task<OneOf<List<S3Bucket>, ListBucketInternalServerError>> ListBuckets()
    {
        var res = await s3Client.ListBucketsAsync();
        if (res.HttpStatusCode != System.Net.HttpStatusCode.OK)
        {
            return new ListBucketInternalServerError();
        }

        return res.Buckets;
    }
    public async Task<PutBucketPolicyResponse> CreateBucketPolicy(BucketPolicyRequestModel bucketPolicy)
    {

        var policyJson = JsonSerializer.Serialize(new
        {
            Version = "2012-10-17",
            Statement = new[]{
            new {
            Effect = bucketPolicy.effect,
            Principal = bucketPolicy.principal,
            Action = bucketPolicy.action,
            Resource = $"arn:aws:s3:::{bucketPolicy.resource}"

            }
            }
        });

        var request = new PutBucketPolicyRequest
        {
            BucketName = bucketPolicy.bucketName,
            Policy = policyJson
        };

        var response = await s3Client.PutBucketPolicyAsync(request);
        return response;

    }




}
