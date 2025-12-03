namespace AIExamples.Shared.Extensions;

public static class HostExtensions
{
    extension(IHost host)
    {
        public T CreateExample<T>() where T : IExample => host.Services.GetRequiredService<T>();

        public async Task ExecuteExampleAsync<T>() where T : IExample
        {
            Console.WriteExampleSeparator<T>();

            try
            {
                await host.CreateExample<T>().ExecuteAsync();
            }
            catch (Exception ex)
            {
                Console.WriteError(ex);
            }

            Console.WriteSeparator();
        }
    }
}
