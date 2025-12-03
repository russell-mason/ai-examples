var builder = WebApplication.CreateBuilder(args);

builder.Services
       .AddOpenApi(options => options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0) // V3 for Semantic Kernel compatibility
       .AddValidation()
       .AddSingleton<IUserService, UserService>();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference(options => options.WithTitle("OpenAPI Test API"));

app.UseHttpsRedirection();

app.RegisterGetUserExistsEndpoint();
app.RegisterGetUserEndpoint();
app.RegisterPostUserEndpoint();

app.Run();
