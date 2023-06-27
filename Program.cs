using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.SemanticFunctions;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.Memory;

var builder = WebApplication.CreateBuilder(args);
var kernelBuilder = new KernelBuilder();

// Add services to the container.


builder.Services.AddControllersWithViews();
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var apiKey=builder.Configuration["api-key"];
//semantic kernel config
kernelBuilder.WithOpenAITextEmbeddingGenerationService("text-embedding-ada-002",apiKey);
kernelBuilder.WithOpenAITextCompletionService("davinci",apiKey);
kernelBuilder.WithMemoryStorage(new VolatileMemoryStore());
var kernel=kernelBuilder.Build();
//cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: MyAllowSpecificOrigins,
        policy =>
        {
            policy
                .WithOrigins(
                    "http://example.com",
                    "http://www.contoso.com",
                    "https://localhost:44434/"
                )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
        }
    );
});
builder.Services.AddSingleton(kernel);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);

app.MapControllerRoute(name: "default", pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
