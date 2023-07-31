using Microsoft.SemanticKernel;

using Npgsql;
using Pgvector.Npgsql;
using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel.Connectors.Memory.Postgres;
using Microsoft.SemanticKernel.Skills.Core;
using Microsoft.SemanticKernel.TemplateEngine;
using Microsoft.SemanticKernel.AI.ChatCompletion;

var builder = WebApplication.CreateBuilder(args);
var kernelBuilder = new KernelBuilder();

// Add services to the container.


builder.Services.AddControllersWithViews();
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var apiKey = builder.Configuration["OPENAI-KEY"];
//semantic kernel config

kernelBuilder.WithOpenAITextEmbeddingGenerationService("text-embedding-ada-002", apiKey);
kernelBuilder.WithOpenAIChatCompletionService("gpt-3.5-turbo", apiKey);

kernelBuilder.WithMemoryStorage(new VolatileMemoryStore());

IKernel kernel=kernelBuilder.Build();
//cors
var context=kernel.CreateNewContext();
var promptRenderer=new PromptTemplateEngine();
var chatGPT=kernel.GetService<IChatCompletion>();

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
// builder.Services.AddSingleton(memStore);
builder.Services.AddSingleton(context);
builder.Services.AddSingleton(promptRenderer);
builder.Services.AddSingleton(chatGPT);
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
// app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
