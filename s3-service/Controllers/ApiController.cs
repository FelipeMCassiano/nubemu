using Microsoft.AspNetCore.Mvc;
using Aws.S3.Models;
using Aws.S3.BucketManager;
using Amazon.S3;
using Swashbuckle.AspNetCore.Annotations;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class ControllerApi : ControllerBase
{
    private static BucketManager bucketManager = new BucketManager();

    /// <summary>
    /// Creates a new S3 bucket.
    /// </summary>
    /// <param name="s3BucketRequest">The request model containing the bucket name and configuration.</param>
    /// <returns>A result indicating the success or failure of the bucket creation.</returns>
    [HttpPost]
    [Route("/s3/createbucket")]
    [SwaggerOperation(Summary = "Creates a new S3 bucket", Description = "Creates a new S3 bucket with the specified name and configuration.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Bucket created successfully")]
    [SwaggerResponse(StatusCodes.Status409Conflict, "Bucket already exists")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request data")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
    public async Task<IResult> CreateS3(BucketRequestModel s3BucketRequest)
    {
        try
        {
            var res = await bucketManager.CreateBucketAsync(s3BucketRequest);
            if (res.IsT1)
            {
                return Results.Conflict(res.AsT1);
            }

            if (res.AsT0.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                return Results.BadRequest(res);
            }

            return Results.Ok($"Bucket created: {s3BucketRequest.Name}");
        }
        catch (AmazonS3Exception ex)
        {
            return Results.Json(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Deletes an existing S3 bucket.
    /// </summary>
    /// <param name="name">The name of the bucket to delete.</param>
    /// <returns>A result indicating the success or failure of the bucket deletion.</returns>
    [HttpDelete]
    [Route("/s3/deletebucket/{name}")]
    [SwaggerOperation(Summary = "Deletes an existing S3 bucket", Description = "Deletes an S3 bucket with the specified name.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Bucket deleted successfully")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Bucket not found")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
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

    /// <summary>
    /// Lists all S3 buckets.
    /// </summary>
    /// <returns>A list of S3 buckets.</returns>
    [HttpGet]
    [Route("/s3/listbuckets")]
    [SwaggerOperation(Summary = "Lists all S3 buckets", Description = "Retrieves a list of all S3 buckets.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Buckets retrieved successfully")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
    public async Task<IResult> ListS3Bucket()
    {
        var res = await bucketManager.ListBuckets();
        if (res.IsT1)
        {
            return Results.Json(res.AsT1, statusCode: StatusCodes.Status500InternalServerError);
        }

        return Results.Ok(res.AsT0);
    }

    /// <summary>
    /// Creates a lifecycle rule for an S3 bucket.
    /// </summary>
    /// <param name="bucketLifecycleRuleRequest">The request model containing the lifecycle rule details.</param>
    /// <returns>A result indicating the success or failure of the lifecycle rule creation.</returns>
    [HttpPost]
    [Route("/s3/create/lifecyclerule")]
    [SwaggerOperation(Summary = "Creates a lifecycle rule for an S3 bucket", Description = "Adds a lifecycle rule to an existing S3 bucket.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Lifecycle rule created successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request data")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
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

    /// <summary>
    /// Creates a policy for an S3 bucket.
    /// </summary>
    /// <param name="bucketPolicyRequest">The request model containing the bucket policy details.</param>
    /// <returns>A result indicating the success or failure of the policy creation.</returns>
    [HttpPost]
    [Route("/s3/create/policy")]
    [SwaggerOperation(Summary = "Creates a policy for an S3 bucket", Description = "Adds a policy to an existing S3 bucket.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Policy created successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request data")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error")]
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

