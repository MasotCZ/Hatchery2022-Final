using HatcheryFinal_Web_API.Data;
using HatcheryFinal_Web_API.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BankDbContext>();

builder.Services.AddScoped<IBankDbContext, BankDbContext>();
builder.Services.AddScoped<IProfitabilityRepository, ProfitabilityRepository>();
builder.Services.AddScoped<ICreditRequestRepository, CreditRequestRepository>();
builder.Services.AddScoped<ICreditPartnerRepository, CreditPartnerRepository>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddControllers();

//swag
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();

//swag
app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
