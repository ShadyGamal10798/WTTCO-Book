using Serilog.Sinks.Elasticsearch;
using Serilog;
using OpenTelemetry;
using OpenTelemetry.Trace;
using Google.Protobuf.WellKnownTypes;
using Elastic.Apm.NetCoreAll;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
        .Build();


//Elasticsearch

//http://elastic:qmVRdqI*zCMGkUU72P5n@10.50.50.30:9200
//http://elastic:qmVRdqI*zCMGkUU72P5n@elastic.wttco.com:9200







#region Elastic Search

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

#endregion




//ConfigureLogging();
builder.Host.UseSerilog();

var app = builder.Build();

app.UseAllElasticApm(builder.Configuration);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDeveloperExceptionPage();


app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();




app.Run();
