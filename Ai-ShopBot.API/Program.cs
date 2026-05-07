using Ai_shopBot.Infrastructure.Extensions;
using Ai_ShopBot.API;
using Ai_ShopBot.API.Hubs;
using Ai_ShopBot.Application.Behaviors;
using Ai_ShopBot.Application.Extensions;
using Ai_ShopBot.Core.Constants;
using Ai_ShopBot.Core.Models;
using Ai_ShopBot.Presistance.Context;
using Ai_ShopBot.Presistance.Extensions;
using Carter;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplication(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddPersistance(builder.Configuration);

builder.Services.AddAPIDependencies(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddCarter();

var app = builder.Build();


//if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();


app.UseCors(x =>
{
    x.AllowAnyHeader()
     .AllowAnyMethod()
     .AllowAnyOrigin();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await InitiateDatabase(app);

app.MapHub<ClientHub>("/clientChatHub");
app.MapCarter();

app.Run();

static async Task InitiateDatabase(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ShopDbContext>();
        var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
        var totalMigrations = context.Database.GetMigrations();
        if (pendingMigrations.Count() == totalMigrations.Count())
        {
            await context.Database.MigrateAsync();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            await roleManager.CreateAsync(new IdentityRole(Roles.Client));

            var userManager = services.GetRequiredService<UserManager<User>>();
            await userManager.CreateAsync(new User
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                FullName = "Admin"
            }, "123@Abc");
        }
        else if (pendingMigrations.Any())
        {
            await context.Database.MigrateAsync();
        }
    }
}