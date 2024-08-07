using Microsoft.AspNetCore.Mvc;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Aws.S3.Models;
using Aws.S3.BucketManager;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class ControllerApi : ControllerBase
{
    private static BucketManager bucketManager = new BucketManager();

    [HttpPost]
    [Route("/s3/create")]
    public async Task<IResult> CreateS3(BucketRequestModel s3BucketRequest)
    {
        var res = await bucketManager.CreateBucketAsync(s3BucketRequest);
        if (!res)
        {
            return Results.BadRequest();
        }

        return Results.Ok(s3BucketRequest.Name);

    }

    [HttpDelete]
    [Route("/s3/delete/{name}")]
    public async Task<IResult> DeleteS3(string name)
    {
        var res = await bucketManager.DeleteBucketAsync(name);
        if (!res)
        {
            return Results.NotFound();
        }

        return Results.Ok();

    }

}
