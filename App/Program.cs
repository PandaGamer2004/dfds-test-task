using DfdsTestTask.Exceptions;
using DfdsTestTask.Features.Encryption.Implementations;
using DfdsTestTask.Features.Encryption.Interfaces;
using DfdsTestTask.Features.Encryption.Models;
using DfdsTestTask.Features.UserManagement.BusinessLogic.Implementations;
using DfdsTestTask.Features.UserManagement.BusinessLogic.Interfaces;
using DfdsTestTask.Features.UserManagement.Persistence.Implementations;
using DfdsTestTask.Features.UserManagement.Persistence.Interfaces;
using DfdsTestTask.PersistenceShared;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => { options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Booking management API",
        Description = "An ASP.NET core Web API fro managing ToDo items",
        Contact = new OpenApiContact
        {
            Name = "Daniil Bazhanov",
            Email = "daniil.bazhanov.dk@gmail.com"
        }
    });
});



builder.Services.AddDbContext<BookingManagementDbContext>(options =>
{
    const string sqlServerConnectionStringPath = "ConnectionStrings:mssql";
    string? sqlConnectionString = builder.Configuration[sqlServerConnectionStringPath];

    if (sqlConnectionString is null)
    {
        throw new IncompleteAppConfigurationException(
            configurationSection: sqlServerConnectionStringPath
            );
    }
    
    options.UseSqlServer(sqlConnectionString);
});
builder.Services.AddSingleton<
    IEncryptionConfigurationLoader<SymmetricEncryptionContext>,
    FromConfigurationSymmetricContextEncryptionLoader
>();


builder.Services.AddSingleton<
    ISymmetricStringDataEncryptor,
    AesStringDataEncryptor
>();

builder.Services.AddScoped<
    IUserRepository, 
    UserRepository
>();


builder.Services.AddScoped<
    IUserService, 
    UserService
>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

