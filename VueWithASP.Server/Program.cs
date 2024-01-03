using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Data.SqlClient;
using VueWithASP.Server.MyCode.Core.ErrorHandling;
using VueWithASP.Server.MyCode.Core.Settings;

try
{
  var builder = WebApplication.CreateBuilder(args);

  // Add services to the container.

  builder.Services.AddControllers();
  // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
  builder.Services.AddEndpointsApiExplorer();
  builder.Services.AddSwaggerGen();

  var app = builder.Build();

  app.Use(async (context, next) =>
  {
    context.Request.EnableBuffering();
    await next();
  });

  app.UseDefaultFiles();
  app.UseStaticFiles();

  // Configure the HTTP request pipeline.
  if (app.Environment.IsDevelopment())
  {
    app.UseSwagger();
    app.UseSwaggerUI();
  }

  app.UseHttpsRedirection();

  //string connectionString = app.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")!;
  //connectionString = connectionString.Replace("{your_password}"
  //  , VueWithASP.Server.MyCode.Core.Settings.cSettings.getSetting(cSettings.eSetting.DbPass)
  //  );
  //try
  //{
  //  using var conn = new SqlConnection(connectionString);
  //  conn.Open();

  //  var command = new SqlCommand(
  //      "SELECT * FROM [dbo].[APP_OBJECTS];",
  //      conn);
  //  using SqlDataReader reader = command.ExecuteReader();
  //}
  //catch (Exception e)
  //{
  //  // Table may already exist
  //  Console.WriteLine(e.Message);
  //}
  app.UseRouting();

  app.UseAuthorization();

  app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

  //app.MapControllers();

  app.MapFallbackToFile("/index.html");
  app.Run();

}
catch (Exception oException)
{
  VueWithASP.Server.MyCode.Core.ErrorHandling.cErrorHandling.LogError(oException);
}
