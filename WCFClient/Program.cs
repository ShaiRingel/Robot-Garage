using GarageServiceProxy;
using WCFClient.Components;


internal class Program
{
    private static void Main(string[] args)
  {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddRazorComponents()
            .AddInteractiveServerComponents();

		builder.Services.AddSingleton(sp =>
        {
            return new GarageServiceClient(GarageServiceClient.EndpointConfiguration.WSHttpBinding_IGarageService,
			"https://localhost:5001/GarageService/WSHttps");
        });

		var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}