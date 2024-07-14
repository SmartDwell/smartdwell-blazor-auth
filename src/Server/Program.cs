using Server.ApiGroups;
using Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configurationOptionsSection = builder.Configuration.GetSection(nameof(ConfigurationOptions));
_ = configurationOptionsSection.Get<ConfigurationOptions>() 
    ?? throw new Exception("ConfigurationOptions is null.");

builder.Services.AddOptions<ConfigurationOptions>().Bind(configurationOptionsSection);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();

app.MapConfigurationGroup();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
