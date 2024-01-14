using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;

var builder = WebApplication.CreateBuilder(args);
//loggin configuration
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// configure the ocelot json file 
builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json",true,true);

//Ocelot Api Gateway configuation   ocelot.Development.json

builder.Services.AddOcelot().AddCacheManager(settings=>settings.WithDictionaryHandle());

var app = builder.Build();

await app.UseOcelot();
//app.MapGet("/", () => "Hello API GetWay!");
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Hello World!");
    });
});


app.Run();
