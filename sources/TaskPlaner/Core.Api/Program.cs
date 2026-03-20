using Core.Api.Bundels;

string corsPolicy = "ApiCorsPolicy";

var builder = WebApplication.CreateBuilder(args);

ServiceRegistration.RegisterServices(builder.Services, builder.Configuration, corsPolicy);

var app = builder.Build();

app.UseCors(corsPolicy);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

Database.Migrate(app);
Database.SeedDefaultAdminUser(app);

app.Run();
