//M70 SERVICE BACKLOG BACKEND

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ServicesDB>(opt => opt.UseInMemoryDatabase("ServiceList")); //service db & schema
builder.Services.AddDbContext<SoftwareDB>(opt => opt.UseInMemoryDatabase("SoftwareList")); //software upgrades db & schema
builder.Services.AddDbContext<RepairDB>(opt => opt.UseInMemoryDatabase("Repairlist")); //software upgrades db & schema


//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDirectoryBrowser();
var app = builder.Build();

//app.MapGet("/", () => "Load Single Page Application");
app.UseDefaultFiles(); app.UseStaticFiles();




/* 
 SERVICES METHODS
*/
app.MapGet("/api/v1/services", async (ServicesDB db) =>
    await db.Service.ToListAsync());

app.MapPost("/api/v1/services", async (Record service, ServicesDB db) => {
    db.Service.Add(service);
    await db.SaveChangesAsync();
    return Results.Created($"/products/{service.Id}", service);
});

//GET COMPLETE
app.MapGet("/api/v1/services/completed", async (ServicesDB db) =>
    await db.Service.Where(t => t.Completed).ToListAsync());

//GET SPECIFIC SERVICE ITEM
app.MapGet("/api/v1/services/{id}", async (int id, ServicesDB db) =>
    await db.Service.FindAsync(id)
        is Record service
            ? Results.Ok(service)
            : Results.NotFound());

//UPDATE SPECIFIC SERVICE RECORD
app.MapPut("/api/v1/services/{id}", async (int id, Record inputService, ServicesDB db) => {
    var service = await db.Service.FindAsync(id);
    if (service is null) return Results.NotFound();
    service.Date = inputService.Date;
    service.Completed = inputService.Completed;
    service.Fee = inputService.Fee;
    service.Tel = inputService.Tel;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

//REMOVE SPECIFIC RECORD
app.MapDelete("/api/v1/services/{id}", async (int id, ServicesDB db) => {
    if (await db.Service.FindAsync(id) is Record service)
    {
        db.Service.Remove(service);
        await db.SaveChangesAsync();
        return Results.Ok(service);
    }
    return Results.NotFound();
});



/* 
 SOFTWARE-UPGRADES METHODS
*/
app.MapGet("/api/v1/software", async (SoftwareDB db) =>
    await db.Software.ToListAsync());

app.MapPost("/api/v1/software", async (Record software, SoftwareDB db) => {
    db.Software.Add(software);
    await db.SaveChangesAsync();
    return Results.Created($"/products/{software.Id}", software);
});

//GET COMPLETE
app.MapGet("/api/v1/software/completed", async (SoftwareDB db) =>
    await db.Software.Where(t => t.Completed).ToListAsync());

//GET SPECIFIC SERVICE ITEM
app.MapGet("/api/v1/software/{id}", async (int id, SoftwareDB db) =>
    await db.Software.FindAsync(id)
        is Record software
            ? Results.Ok(software)
            : Results.NotFound());

//UPDATE SPECIFIC SERVICE RECORD
app.MapPut("/api/v1/software/{id}", async (int id, Record inputSoftware, SoftwareDB db) => {
    var software = await db.Software.FindAsync(id);
    if (software is null) return Results.NotFound();
    software.Date = inputSoftware.Date;
    software.Completed = inputSoftware.Completed;
    software.Fee = inputSoftware.Fee;
    software.Tel = inputSoftware.Tel;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

//REMOVE SPECIFIC RECORD
app.MapDelete("/api/v1/software/{id}", async (int id, SoftwareDB db) => {
    if (await db.Software.FindAsync(id) is Record software)
    {
        db.Software.Remove(software);
        await db.SaveChangesAsync();
        return Results.Ok(software);
    }
    return Results.NotFound();
});














app.Run();


//RECORD SCHEMA
class Record
{
    public int Id { get; set; }
    public string? Date { get; set; }
    public bool Completed { get; set; }
    public float Fee { get; set; }
    public int Tel { get; set; }
}

//SERVICES IN MEM DB
class ServicesDB : DbContext
{
    public ServicesDB(DbContextOptions<ServicesDB> options)
        : base(options) { }
    public DbSet<Record> Service => Set<Record>();
}


//SOFTWARE UPGRADES IN MEM DB
class SoftwareDB : DbContext
{
    public SoftwareDB(DbContextOptions<SoftwareDB> options)
        : base(options) { }
    public DbSet<Record> Software => Set<Record>();
}


//REPAIR IN MEM DB
class RepairDB : DbContext
{
    public RepairDB(DbContextOptions<SoftwareDB> options)
        : base(options) { }
    public DbSet<Record> Repair => Set<Record>();
}