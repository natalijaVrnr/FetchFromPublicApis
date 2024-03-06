using FetchFromPublicApis_FunctionApp.Services.BlobService;
using FetchFromPublicApis_FunctionApp.Services.TableService;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( sw =>
{
    var apiInfo = new OpenApiInfo { Title = "FetchFromPublicApis_WebApi", Version = "v1" };
    sw.SwaggerDoc("v1", apiInfo);

    var filePath = Path.Combine(System.AppContext.BaseDirectory, $"{apiInfo.Title}.xml");
    sw.IncludeXmlComments(filePath);
});
builder.Services.AddSingleton<IBlobService, BlobService>();
builder.Services.AddSingleton<ITableService, TableService>();
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
