using Amazon;

namespace Aws.S3.Models;

public record BucketRequestModel
{
    public required string Name { get; set; }

}
