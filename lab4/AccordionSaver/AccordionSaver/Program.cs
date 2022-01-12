using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
Console.WriteLine("Hello, World!");
var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.Use(async (context, next) =>
{

    context.Response.OnStarting(() =>
    {
        context.Request.Headers.ContentType = "application/json";
        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        context.Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET");
        return Task.FromResult(0);
    });

    await next();
});

app.Use(async (context, next) =>
{

    context.Response.OnCompleted(() =>
    {
        context.Request.Headers.ContentType = "application/json";
        return Task.FromResult(0);
    });

    await next();
}); 
                    

app.MapPost("/postAccordions", ([FromBody(EmptyBodyBehavior = Microsoft.AspNetCore.Mvc.ModelBinding.EmptyBodyBehavior.Allow)]Accordion[] accordions,
                     [FromHeader(Name = "Content-Type")] string contentType)
                     => {
                         File.WriteAllText("File.json", JsonSerializer.Serialize(accordions));
                         return Results.Ok(accordions);
                     });

app.MapGet("/getAccordions", () =>
{
    var objString = File.ReadAllText("File.json");
    var obj = JsonSerializer.Deserialize<Accordion[]>(objString);
    return Results.Ok(obj);
});
app.Run();

class Accordion
{
    public string Title { get; set; }
    public string Content { get; set; }
}
// See https://aka.ms/new-console-template for more information

