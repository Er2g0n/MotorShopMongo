
using Structure_Context;
using Structure_Core.TransactionManagement;
using Structure_Core.User;
using Structure_Interface.IBaseServices;
using Structure_Interface.IUserManagement;
using Structure_Service.TransactionManagement;
using Structure_Service.UserManagement;

namespace ApplicationAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // MongoDBContext
        builder.Services.AddSingleton<MongoDBContext>(provider =>
            new MongoDBContext(builder.Configuration));

        // OrderProvider
        builder.Services.AddTransient<ICRUD_Service<Order, string>, OrderProvider>();
        //builder.Services.AddTransient<IOrderProvider, OrderProvider>();

        // UserProvider
        builder.Services.AddTransient<ICRUD_Service<User, string>, UserProvider>();
        builder.Services.AddTransient<IUserProvider, UserProvider>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
