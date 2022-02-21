using HatcheryFinal_Web_API.Data;
using HatcheryFinal_Web_API.Data.Repositories;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BankDbContext>();
builder.Services.AddScoped<IBankRepository, BankRepository>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddControllers();


var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
