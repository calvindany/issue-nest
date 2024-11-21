using backend_issue_nest_v2.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionStr = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CustDbContext>(
    options => options.UseLazyLoadingProxies()
    .UseSqlServer(
        connectionStr,
        b => b.MigrationsAssembly("backend-issue-nest-v2")
        )
    );


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
