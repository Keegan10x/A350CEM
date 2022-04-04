//SHOPPING LIST BACKEND

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ProductDb>(opt => opt.UseInMemoryDatabase("ProductList"));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDirectoryBrowser();
var app = builder.Build();

//app.MapGet("/", () => "Load Single Page Application");
app.UseDefaultFiles(); app.UseStaticFiles();


//ALL GET METHODS HERE___________________________________________________
//GET ALL PRODUCTS
app.MapGet("/api/v1/products", async (ProductDb db) =>
    await db.Product.ToListAsync());

//SHOW PURCHASED PRODUCTS
app.MapGet("/api/v1/products/purchased", async (ProductDb db) =>
    await db.Product.Where(t => t.IsPurchased).ToListAsync());

//SHOW SPECIFIC PRODUCT
app.MapGet("/api/v1/todoitems/{id}", async (int id, ProductDb db) =>
    await db.Product.FindAsync(id)
        is Product product
            ? Results.Ok(product)
            : Results.NotFound());



//ALL POST,PUT,DELETE METHODS HERE_______________________________________
//ADD PRODUCT
app.MapPost("/api/v1/products", async (Product product, ProductDb db) => {
    db.Product.Add(product);
    await db.SaveChangesAsync();
    return Results.Created($"/products/{product.Id}", product);
});

//UPDATE CART
app.MapPut("/api/v1/products/{id}", async (int id, Product inputProduct, ProductDb db) => {
    var product = await db.Product.FindAsync(id);
    if (product is null) return Results.NotFound();
    product.Name = inputProduct.Name;
    product.IsPurchased = inputProduct.IsPurchased;
    product.Price = inputProduct.Price;
    product.Qty = inputProduct.Qty;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

//REMOVE PRODUCT
app.MapDelete("/api/v1/products/{id}", async (int id, ProductDb db) => {
    if (await db.Product.FindAsync(id) is Product product)
    {
        db.Product.Remove(product);
        await db.SaveChangesAsync();
        return Results.Ok(product);
    }
    return Results.NotFound();
});




app.Run();

class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsPurchased { get; set; }
    public float Price { get; set; }
    public int Qty { get; set; }
}

class ProductDb : DbContext
{
    public ProductDb(DbContextOptions<ProductDb> options)
        : base(options) { }
    public DbSet<Product> Product => Set<Product>();
}