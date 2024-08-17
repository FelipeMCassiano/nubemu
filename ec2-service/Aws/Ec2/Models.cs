
namespace Aws.Ec2.Models;


public record InstaceRequestModel(string ec2Image, string instanceType, int minCount, int maxCount) { }
