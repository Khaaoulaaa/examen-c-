using BookApi.Data;
using BookApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<BookDb>(opt => opt.UseInMemoryDatabase("BookList"));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.MapGet("/", () => "Hello World!");

// GET /books 
app.MapGet("/books", async (BookDb db) =>
{
    var books = await db.Books.ToListAsync();
    return Results.Ok(books);
});

// POST /books 
app.MapPost("/books", async (Book book, BookDb db) =>
{
    db.Books.Add(book);
    await db.SaveChangesAsync();
    return Results.Created($"/books/{book.Id}", book);
});


app.MapPut("/books/{id}", async (int id, BookDb db) =>
{
    var book = await db.Books.FindAsync(id);
    if (book is null)
    {
        return Results.NotFound();
    }
    book.IsRead = true;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
