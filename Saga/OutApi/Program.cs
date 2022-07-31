using Common;
using Dtmcli;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDtmcli(x =>
{
    x.DtmUrl = "http://localhost:36789";
});

var app = builder.Build();

app.MapPost("/tasks/get", (HttpContext httpContext, TransRequest req) => 
{
    Console.WriteLine($"TransOut, QueryString={httpContext.Request.QueryString}");
    Console.WriteLine($"User: {req.TaskId}, transfer out {req.Description} --- forward");

    return Results.Ok(TransResponse.BuildSucceedResponse());
});


app.Run("http://*:10000");
