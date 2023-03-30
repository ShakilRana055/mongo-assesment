using FluentValidation;
using MongoDB.Driver;
using MongoDBAssesmentDataAccess.IService;
using MongoDBAssesmentDataAccess.Service;
using MongoDBAssesmentDomain.Entity;
using System;

var builder = WebApplication.CreateBuilder(args);

#region MognoRegion
var mongoClient = new MongoClient(builder.Configuration.GetConnectionString("DbConnection"));
var databaseName = builder.Configuration.GetConnectionString("DatabaseName");
builder.Services.AddSingleton<IMongoClient>(mongoClient);
builder.Services.AddSingleton(mongoClient.GetDatabase(databaseName));
builder.Services.AddTransient<IValidator<Users>, UsersValidator>();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ITodoApplicationService, TodoApplicationService>();
#endregion

#region PostGreSQL
var postgreConnection = builder.Configuration.GetConnectionString("PostGreSQLConnection");
builder.Services.AddTransient<IPGUserService, PGUserService>();
builder.Services.AddTransient<IPGTodoApplicationService, PGTodoApplicationService>();
#endregion


// Register generic repository


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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
