using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace Aws.S3.Models;

public record BucketRequestModel
{
    public required string Name { get; set; }

}

public record BucketLifecycleRuleRequestModel
{
    public required string bucketName { get; set; }
    public required List<LifecycleRuleModel> lifecycleRules { get; set; }
}
public record LifecycleRuleModel
{
    public required string id { get; set; }
    public required string prefix { get; set; }
    public required int expiration { get; set; }
    public required LifecycleRulesStatusModel status { get; set; }
}

public enum LifecycleRulesStatusModel
{
    Enabled,
    Disabled

}
public record BucketPolicyRequestModel
{
    public required string bucketName { get; set; }
    public required string effect { get; set; }
    public string? principal { get; set; }
    public required string action { get; set; }
    public required string resource { get; set; }
}
