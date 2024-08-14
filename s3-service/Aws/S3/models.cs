
namespace Aws.S3.Models;

public record BucketRequestModel
{
    public required string Name { get; set; }

}

public record BucketLifecycleRuleRequestModel(string bucketName, List<LifecycleRuleModel> lifecycleRules) { }

public record LifecycleRuleModel(string id, string prefix, int expiration, LifecycleRulesStatusModel status) { }

// TODO: remove this enum
public enum LifecycleRulesStatusModel
{
    Enabled,
    Disabled

}
public record BucketPolicyRequestModel(string bucketName, string effect, string principal, string action, string resource)
{
    public string principal { get; init; } = principal ?? "*";
}
