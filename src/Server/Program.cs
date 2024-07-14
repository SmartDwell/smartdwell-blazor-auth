using Blazored.LocalStorage;
using Server.ApiGroups;
using Shared;

const string corsPolicyName = "AllowAll";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, builder =>
    {
        builder.AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .SetIsOriginAllowed(_ => true);
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<TokenRepository>();

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

app.UseCors(corsPolicyName);

app.MapConfigurationGroup();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
