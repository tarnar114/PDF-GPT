using Microsoft.SemanticKernel;


using Microsoft.Extensions.Azure;
using Microsoft.AspNetCore.Http;
using JavaScriptEngineSwitcher.V8;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using React.AspNet;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddReact();
builder.Services.AddJsEngineSwitcher(options => options.DefaultEngineName = V8JsEngine.EngineName)
  .AddV8();
builder.Services.AddControllersWithViews();
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


builder.Services.AddMemoryCache();
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
var appLifetime = app.Services.GetService<IHostApplicationLifetime>();


app.UseHttpsRedirection();
app.UseReact(config =>
{
  // If you want to use server-side rendering of React components,
  // add all the necessary JavaScript files here. This includes
  // your components as well as all of their dependencies.
  // See http://reactjs.net/ for more information. Example:
  //config
  //  .AddScript("~/js/First.jsx")
  //  .AddScript("~/js/Second.jsx");

  // If you use an external build too (for example, Babel, Webpack,
  // Browserify or Gulp), you can improve performance by disabling
  // ReactJS.NET's version of Babel and loading the pre-transpiled
  // scripts. Example:
  //config
  //  .SetLoadBabel(false)
  //  .AddScriptWithoutTransform("~/js/bundle.server.js");
});
app.UseStaticFiles();
app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);

app.MapControllerRoute(name: "default", pattern: "{controller}/{action=Index}/{id?}");

// app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
