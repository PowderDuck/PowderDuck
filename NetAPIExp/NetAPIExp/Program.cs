using Microsoft.EntityFrameworkCore;
using NetAPIExp;
using Microsoft.Data.SqlClient;
using NetAPIExp.Database;
using Azure.Core;
using Azure;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddDbContext<UserContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostConnection"))
    //options => options.UseSqlServer(@"Server=(localdb)\MSSQLLOCALDB;Database=Users;Trusted_Connection=True")
    );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
