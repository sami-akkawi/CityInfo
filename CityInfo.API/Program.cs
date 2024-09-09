using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options => options.ReturnHttpNotAcceptable = true)
    .AddXmlDataContractSerializerFormatters();
builder.Services.AddProblemDetails(
    options => 
        options.CustomizeProblemDetails = ctx =>
        {
            ctx.ProblemDetails.Extensions.Add("additionalInfo", "Additional info example");
            ctx.ProblemDetails.Extensions.Add("Server", Environment.MachineName);
        }
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

// app.UseEndpoints(endpoints => endpoints.MapControllers());

app.MapControllers();

app.Run();