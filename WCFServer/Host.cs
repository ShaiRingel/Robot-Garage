using CoreWCF;
using CoreWCF.Configuration;
using CoreWCF.Description;
using WCFServer;

public class Host
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddServiceModelServices()
            .AddServiceModelMetadata();

        builder.Services.AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

        var app = builder.Build();

        var myWSHttpBinding = new WSHttpBinding(SecurityMode.Transport);
        myWSHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;

        app.UseServiceModel(builder =>
        {
            builder.AddService<GarageService>((serviceOptions) => { })
            .AddServiceEndpoint<GarageService,
                IGarageService>(new BasicHttpBinding(),
                "/GarageService/basichttp")
            .AddServiceEndpoint<GarageService,
                IGarageService>(myWSHttpBinding,
                "/GarageService/WSHttps");
        });

        var serviceMetadataBehavior = app.Services.GetRequiredService<CoreWCF.Description.ServiceMetadataBehavior>();
        serviceMetadataBehavior.HttpGetEnabled = true;

        app.Run();
    }
}
