
using Amazon.EC2;

namespace Aws.Ec2.Models;


public record InstanceRequestModel(string name, string ec2Image, string instanceType, int minCount, int maxCount) { }

public record InstanceResponseModel(string instanceId, string name, InstanceStateName status) { }
