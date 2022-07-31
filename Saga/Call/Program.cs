using Dtmcli;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, World!");

var services = new ServiceCollection();

services.AddDtmcli(x =>
{
    x.DtmUrl = "http://localhost:36789";
});

var provider = services.BuildServiceProvider();
var dtmClient = provider.GetRequiredService<IDtmClient>();

var outApi = "http://localhost:5000/tasks/get";
var inApi = "http://localhost:10001/tasks/post";

var userOutReq = new Common.TransRequest() { TaskId = "1", Description = "Task1" };
var userInReq = new Common.TransRequest() { TaskId = "2", Description = "Task2" };


await Case3(dtmClient);


Console.ReadKey();



async Task Case3(IDtmClient dtmClient)
{
   var ct = new CancellationToken();

   var gid = await dtmClient.GenGid(ct);
   var saga = new Saga(dtmClient, gid)
       .Add(outApi + "/TransOut", outApi + "/TransOutCompensate", userOutReq)
       .Add(inApi + "/TransInError", inApi + "/BarrierTransInCompensate", userInReq)
       .EnableWaitResult()
       ;

   await saga.Submit(ct);

   Console.WriteLine($"Submit SAGA transaction, gid is {gid}.");
}