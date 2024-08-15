using Microsoft.AspNetCore.Mvc;
using Aws.S3.Models;
using Aws.S3.BucketManager;
using Amazon.S3;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class ControllerApi : ControllerBase
{
    private static BucketManager bucketManager = new BucketManager();

    [HttpPost]
    [Route("/s3/createbucket")]
    public async Task<IResult> CreateS3(BucketRequestModel s3BucketRequest)

    {
        try
        {
            var res = await bucketManager.CreateBucketAsync(s3BucketRequest);
            if (res.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                return Results.BadRequest(res);
            }


            return Results.Ok($"bucket created: {s3BucketRequest.Name}");

        }
        catch (AmazonS3Exception ex)
        {
            return Results.Json(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }



    }

    [HttpDelete]
    [Route("/s3/deletebucket/{name}")]
    public async Task<IResult> DeleteS3(string name)
    {
        try
        {
            var res = await bucketManager.DeleteBucketAsync(name);
            if (res.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                return Results.NotFound(res);
            }
        }
        catch (AmazonS3Exception ex)
        {
            return Results.Json(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }

        return Results.Ok();

    }

    [HttpGet]
    [Route("/s3/listbuckets")]
    public async Task<IResult> ListS3Bucket()
    {
        var res = await bucketManager.ListBuckets();
        if (res.HttpStatusCode != System.Net.HttpStatusCode.OK)
        {
            return Results.BadRequest(res);
        }

        return Results.Ok(res.Buckets);
    }


    [HttpPost]
    [Route("/s3/create/lifecyclerule")]
    public async Task<IResult> CreateS3Lifecycle([FromBody] BucketLifecycleRuleRequestModel bucketLifecycleRuleRequest)
    {

        try
        {
            var res = await bucketManager.CreateLifeclyeBucket(bucketLifecycleRuleRequest);
            if (res.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                return Results.BadRequest(res);
            }


            return Results.Ok();
        }
        catch (AmazonS3Exception ex)
        {
            return Results.Json(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
        catch (Exception ex)
        {
            return Results.Json(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    [Route("/s3/create/policy")]
    public async Task<IResult> CreateS3Policy(BucketPolicyRequestModel bucketPolicyRequest)
    {
        try
        {
            var res = await bucketManager.CreateBucketPolicy(bucketPolicyRequest);
            if (res.HttpStatusCode != System.Net.HttpStatusCode.NoContent)
            {
                return Results.BadRequest(res);
            }
            return Results.Ok();

        }
        catch (AmazonS3Exception ex)
        {
            return Results.Json(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
