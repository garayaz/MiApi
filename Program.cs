using MiApi.Data;
using MiApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectioString = builder.Configuration.GetConnectionString("PostGreSQLConnection");
builder.Services.AddDbContext<PostsDB>(options =>
options.UseNpgsql(connectioString));

var app = builder.Build();

// Configure the HTTP request.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

    app.UseHttpsRedirection();

    app.MapPost("/Posts/", async (Post e, PostsDB db) =>
            {
                db.Posts.Add(e);
                await db.SaveChangesAsync();

                return Results.Created($"post/{e.id}", e);
            });

    app.MapGet("/Posts/", async (PostsDB db) =>
    {
        var posts = await db.Posts.ToListAsync();
        return Results.Ok(posts);
    });

    app.MapGet("/Posts/{id:int}", async (int id, PostsDB db) =>
            {
                var post = await db.Posts.FindAsync(id);
                return post != null ? Results.Ok(post) : Results.NotFound();
            });

    app.MapDelete("/Posts/{id:int}", async (int id, PostsDB db) =>
        {
            var post = await db.Posts.FindAsync(id);
            if (post == null)
            {
                return Results.NotFound();
            }

            db.Posts.Remove(post);
            await db.SaveChangesAsync();

            return Results.Ok(post);

        });


app.Run();
