using Microsoft.AspNetCore.Mvc;
using Aws.Ec2.Ec2Manager;
using Aws.Ec2.Models;

namespace Ec2.Controllers;

[ApiController]
[Route("[controller]")]
public class Ec2Controller : Controller
{

    Ec2Manager ec2Manager = new Ec2Manager();

    [HttpPost]
    [Route("/ec2/launchinstance")]
    public async Task<IResult> CreateEc2Instance([FromBody] InstaceRequestModel request)
    {

        var res = await ec2Manager.LaunchEc2Instance(request);

        if (res.HttpStatusCode != System.Net.HttpStatusCode.OK)
        {
            return Results.Json(res, statusCode: StatusCodes.Status500InternalServerError);
        }


        return Results.Created("Instance launched", res);
    }



}
