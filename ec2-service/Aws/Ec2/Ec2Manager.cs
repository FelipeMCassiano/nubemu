
using Amazon.EC2;
using Amazon.EC2.Model;
using Amazon.Runtime;
using Aws.Ec2.Models;

namespace Aws.Ec2.Ec2Manager;

public class Ec2Manager
{

    private static AmazonEC2Client _ec2Client = new AmazonEC2Client(
   new BasicAWSCredentials("dummy-access-key", "dummy-secret-key"), // localstack doens't need a real credential
   new AmazonEC2Config
   {
       ServiceURL = "http://localhost:4566",
       UseHttp = true
   }

);

    private static Dictionary<string, string> _imagesIds = new Dictionary<string, string>(){
        {"ubuntu", "ami-785db401"},
            {"amazon-linux", "ami-760aaa0f"},
            {"windows", "ami-03cf127a"},
    };


    public async Task<RunInstancesResponse> LaunchEc2Instance(InstaceRequestModel instaceRequestModel)
    {
        var imgId = _imagesIds[instaceRequestModel.ec2Image.ToLower()];


        var request = new RunInstancesRequest
        {
            ImageId = imgId,
            InstanceType = InstanceType.FindValue(instaceRequestModel.instanceType),
            MinCount = instaceRequestModel.minCount,
            MaxCount = instaceRequestModel.maxCount,
        };

        var response = await _ec2Client.RunInstancesAsync(request);

        return response;

    }



}

