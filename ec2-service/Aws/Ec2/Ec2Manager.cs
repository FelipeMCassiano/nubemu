
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

    public async Task<RunInstancesResponse> LaunchEc2Instance(InstanceRequestModel instaceRequestModel)
    {
        var imgId = _imagesIds[instaceRequestModel.ec2Image.ToLower()];


        var request = new RunInstancesRequest
        {
            ImageId = imgId,
            InstanceType = InstanceType.FindValue(instaceRequestModel.instanceType),
            MinCount = instaceRequestModel.minCount,
            MaxCount = instaceRequestModel.maxCount,
            TagSpecifications = new List<TagSpecification>{
                new TagSpecification{
                    ResourceType = "instance",
                    Tags = new List<Tag>{
                        new Tag {Key = "Name", Value = instaceRequestModel.name }
                    }
                }
            }

        };

        var response = await _ec2Client.RunInstancesAsync(request);

        return response;

    }

    public async Task<List<InstanceResponseModel>> ListEc2Instances()
    {
        var nameTag = new Tag { Key = "Name" };

        var response = await _ec2Client.DescribeInstancesAsync();

        var instances = new List<InstanceResponseModel>();

        foreach (var reservations in response.Reservations)
        {
            foreach (var i in reservations.Instances)
            {
                var name = i.Tags.Find(x => x.Key == nameTag.Key);
                if (name is null)
                {
                    continue;
                }

                var instance = new InstanceResponseModel(name.Value, i.State.Name);

                instances.Add(instance);

            }
        }

        return instances;

    }


}

