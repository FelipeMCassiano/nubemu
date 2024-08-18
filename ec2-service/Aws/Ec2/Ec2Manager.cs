
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

    private static Dictionary<string, string> _imagesIds = new Dictionary<string, string>(){ {"ubuntu", "ami-785db401"},
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
        _ec2Client.runI

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

                var instance = new InstanceResponseModel(i.InstanceId, name.Value, i.State.Name);

                instances.Add(instance);

            }
        }

        return instances;

    }
    public async Task<TerminateInstancesResponse> TerminateInstance(string instanceName)
    {
        var instaces = await GetInstancesByName(instanceName);

        var instancesIds = new List<string>();
        foreach (var i in instaces)
        {
            instancesIds.Add(i.instanceId);

        }
        var request = new TerminateInstancesRequest
        {
            InstanceIds = instancesIds
        };

        var response = await _ec2Client.TerminateInstancesAsync(request);
        return response;
    }

    public async Task<List<InstanceResponseModel>> GetInstancesByName(string instanceName)
    {
        var describeInstancesRequest = new DescribeInstancesRequest
        {
            Filters = new List<Filter>
        {
            new Filter
            {
                Name = "tag:Name",
                Values = new List<string> { instanceName }
            }
        }
        };

        var describeInstancesResponse = await _ec2Client.DescribeInstancesAsync(describeInstancesRequest);

        var instanceStatusList = new List<InstanceResponseModel>();

        foreach (var reservation in describeInstancesResponse.Reservations)
        {
            foreach (var instance in reservation.Instances)
            {
                var name = instance.Tags.Find(x => x.Key == "Name");
                if (name is null) continue;

                instanceStatusList.Add(new InstanceResponseModel(
                    instance.InstanceId,
                    name.Value,
                    instance.State.Name.Value));
            }
        }

        return instanceStatusList;

    }


}

