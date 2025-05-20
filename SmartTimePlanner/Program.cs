using Umbraco.Cms.Web.Common.ApplicationBuilder;
using Umbraco.Cms.Core.Notifications;


var builder = WebApplication.CreateBuilder(args);

// Lägg till vanliga controllers
builder.Services.AddControllers();

// Här börjar Umbraco-bygget
builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddComposers()
    .Build();

var app = builder.Build();

await app.BootUmbracoAsync();

app.UseHttpsRedirection();

app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

app.MapControllers();

app.Run();
