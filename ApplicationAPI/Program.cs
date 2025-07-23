
using Structure_Context;
using Structure_Core.ProductManagement;
using Structure_Core.TransactionManagement;
using Structure_Core.UserManagement;
using Structure_Interface.IBaseServices;
using Structure_Interface.ITransactionService;
using Structure_Interface.IUserService;
using Structure_Service.ProductService;
using Structure_Service.TransactionService;
using Structure_Service.UserService;

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

        //ProductProvider
        builder.Services.AddTransient<ICRUD_Service<Product, string>, ProductProvider>();

        // TransactionProvider
        // OrderProvider
        builder.Services.AddTransient<ICRUD_Service<Order, string>, OrderProvider>();
        builder.Services.AddTransient<IOrderProvider, OrderProvider>();

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
