namespace AIExamples.Shared.Extensions;

public static class HostBuilderExtensions
{
    extension(IHostBuilder builder)
    {
        public IHostBuilder ConfigureAzureAIFoundry() => builder.BindSettings<AzureAIFoundrySettings>("AzureAIFoundry");

        public IHostBuilder ConfigureGitHubAI() => builder.BindSettings<GitHubAISettings>("GitHubAI");

        public IHostBuilder ConfigureOllamaAI() => builder.BindSettings<OllamaAISettings>("OllamaAI");

        public IHostBuilder ConfigureLMStudioAI() => builder.BindSettings<LMStudioAISettings>("LMStudioAI");

        public IHostBuilder ConfigureCosmosDB() => builder.BindSettings<CosmosDBSettings>("CosmosDB");

        public IHostBuilder ConfigureOpenApi() => builder.BindSettings<OpenApiSettings>("OpenApi");

        public IHostBuilder ConfigureQdrant() => builder.BindSettings<QdrantSettings>("Qdrant");

        public IHostBuilder RegisterExamples()
        {
            builder.ConfigureServices(services =>
            {
                services.Scan(scan => scan.FromApplicationDependencies()
                                          .AddClasses(classes => classes.AssignableTo<IExample>())
                                          .AsSelf()
                                          .WithTransientLifetime());
            });

            return builder;
        }

        private IHostBuilder BindSettings<T>(string sectionName) where T : class, new()
        {
            builder.ConfigureServices((context, services) =>
            {
                var settings = new T();
                context.Configuration.GetSection(sectionName).Bind(settings);

                services.AddSingleton(settings);
            });

            return builder;
        }
    }
}
