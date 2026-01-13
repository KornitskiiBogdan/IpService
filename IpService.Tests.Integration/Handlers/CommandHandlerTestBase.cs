namespace IpService.Tests.Integration.Handlers;

public class CommandHandlerTestBase
{
    protected CustomWebApplicationFactory Factory;
    protected IServiceProvider ServiceProvider;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        Factory = new CustomWebApplicationFactory();
        await Factory.InitializeAsync();

        ServiceProvider = Factory.Services;
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await Factory.DisposeAsync();
    }
}