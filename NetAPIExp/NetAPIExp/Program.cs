using Microsoft.EntityFrameworkCore;
using NetAPIExp;
using Microsoft.Data.SqlClient;
using NetAPIExp.Database;

var builder = WebApplication.CreateBuilder(args);

//DatabaseHandler handler = new DatabaseHandler(builder.Configuration.GetConnectionString("PostConnection"));

// Add services to the container.
//builder.Configuration.GetConnectionString("");
builder.Services.AddControllers();
builder.Services.AddDbContext<UserContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostConnection"))
    //options => options.UseSqlServer(@"Server=(localdb)\MSSQLLOCALDB;Database=Users;Trusted_Connection=True")
    );
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
