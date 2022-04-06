//M70 SERVICE BACKLOG BACKEND

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;


string admin = "Basic YWRtaW46YWRtaW4xMjM=";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ServicesDB>(opt => opt.UseInMemoryDatabase("ServiceList")); //service db 
builder.Services.AddDbContext<SoftwareDB>(opt => opt.UseInMemoryDatabase("SoftwareList")); //software upgrades db
builder.Services.AddDbContext<RepairDB>(opt => opt.UseInMemoryDatabase("Repairlist")); //repair db 
builder.Services.AddDbContext<InspectionDB>(opt => opt.UseInMemoryDatabase("Inspectionlist")); //safety inspection db


//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDirectoryBrowser();
var app = builder.Build();

//METHODS FOR LOADING FRONTEND SPA
//app.MapGet("/", () => "Load Single Page Application");
//app.UseDefaultFiles(); app.UseStaticFiles();


app.UseFileServer(new FileServerOptions {
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles")),
    RequestPath = "",
    EnableDefaultFiles = true
});;


/* 
 SERVICES METHODS
*/
app.MapGet("/api/v1/services", async (HttpContext context, ServicesDB db) => {
    string auth = context.Request.Headers["Authorization"].ToString();
    if (auth != admin){ throw new Exception("USER NOT AUTHORIZED"); } 
    return await db.Service.ToListAsync();
});

app.MapPost("/api/v1/services", async (Record service, ServicesDB db, HttpContext context) => {
    string auth = context.Request.Headers["Authorization"].ToString();
    if (auth != admin) { throw new Exception("USER NOT AUTHORIZED"); }
    db.Service.Add(service);
    await db.SaveChangesAsync();
    return Results.Created($"/services/{service.Id}", service);
});

//GET COMPLETE SERVICE RECORDS
app.MapGet("/api/v1/services/completed", async (ServicesDB db, HttpContext context) => {
    string auth = context.Request.Headers["Authorization"].ToString();
    if (auth != admin) { throw new Exception("USER NOT AUTHORIZED"); }
    return await db.Service.Where(t => t.Completed).ToListAsync();
});


//GET SPECIFIC SERVICE RECORD
app.MapGet("/api/v1/services/{id}", async (int id, ServicesDB db, HttpContext context) =>
    await db.Service.FindAsync(id)
        is Record service
            ? Results.Ok(service)
            : Results.NotFound());


//UPDATE SPECIFIC SERVICE RECORD
app.MapPut("/api/v1/services/{id}", async (int id, Record inputService, ServicesDB db, HttpContext context) => {
    if (context.Request.Headers["Authorization"].ToString() != admin) { throw new Exception("USER NOT AUTHORIZED"); } 
    var service = await db.Service.FindAsync(id);
    if (service is null) return Results.NotFound();
    service.Date = inputService.Date;
    service.Completed = inputService.Completed;
    service.Fee = inputService.Fee;
    service.Tel = inputService.Tel;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

//REMOVE SPECIFIC SERVICE RECORD
app.MapDelete("/api/v1/services/{id}", async (int id, ServicesDB db, HttpContext context) => {
    if (context.Request.Headers["Authorization"].ToString() != admin) { throw new Exception("USER NOT AUTHORIZED"); }
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
app.MapGet("/api/v1/software", async (SoftwareDB db, HttpContext context) => {
    if (context.Request.Headers["Authorization"].ToString() != admin) { throw new Exception("USER NOT AUTHORIZED"); }
    return await db.Software.ToListAsync();
});
    

app.MapPost("/api/v1/software", async (Record software, SoftwareDB db, HttpContext context) => {
    if (context.Request.Headers["Authorization"].ToString() != admin) { throw new Exception("USER NOT AUTHORIZED"); }
    db.Software.Add(software);
    await db.SaveChangesAsync();
    return Results.Created($"/software/{software.Id}", software);
});

//GET COMPLETE SOFTWARE-UPGRADES
app.MapGet("/api/v1/software/completed", async (SoftwareDB db, HttpContext context) => {
    if (context.Request.Headers["Authorization"].ToString() != admin) { throw new Exception("USER NOT AUTHORIZED"); }
    return await db.Software.Where(t => t.Completed).ToListAsync();
});
   

//GET SPECIFIC SOFTWARE-UPGRADE RECORDS
app.MapGet("/api/v1/software/{id}", async (int id, SoftwareDB db) =>
    await db.Software.FindAsync(id)
        is Record software
            ? Results.Ok(software)
            : Results.NotFound());

//UPDATE SPECIFIC SOFTWARE-UPGRADE RECORD
app.MapPut("/api/v1/software/{id}", async (int id, Record inputSoftware, SoftwareDB db, HttpContext context) => {
    if (context.Request.Headers["Authorization"].ToString() != admin) { throw new Exception("USER NOT AUTHORIZED"); }
    var software = await db.Software.FindAsync(id);
    if (software is null) return Results.NotFound();
    software.Date = inputSoftware.Date;
    software.Completed = inputSoftware.Completed;
    software.Fee = inputSoftware.Fee;
    software.Tel = inputSoftware.Tel;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

//REMOVE SPECIFIC SOFTWARE-UPGRADE RECORD
app.MapDelete("/api/v1/software/{id}", async (int id, SoftwareDB db, HttpContext context) => {
    if (context.Request.Headers["Authorization"].ToString() != admin) { throw new Exception("USER NOT AUTHORIZED"); }
    if (await db.Software.FindAsync(id) is Record software)
    {
        db.Software.Remove(software);
        await db.SaveChangesAsync();
        return Results.Ok(software);
    }
    return Results.NotFound();
});



/* 
 REPAIR METHODS
*/
app.MapGet("/api/v1/repair", async (RepairDB db, HttpContext context) => {
    if (context.Request.Headers["Authorization"].ToString() != admin) { throw new Exception("USER NOT AUTHORIZED"); }
    return await db.Repair.ToListAsync();
});
    

app.MapPost("/api/v1/repair", async (Record repair, RepairDB db, HttpContext context) => {
    if (context.Request.Headers["Authorization"].ToString() != admin) { throw new Exception("USER NOT AUTHORIZED"); }
    db.Repair.Add(repair);
    await db.SaveChangesAsync();
    return Results.Created($"/products/{repair.Id}", repair);
});

//GET COMPLETED REPAIRS
app.MapGet("/api/v1/repair/completed", async (RepairDB db, HttpContext context) => {
    if (context.Request.Headers["Authorization"].ToString() != admin) { throw new Exception("USER NOT AUTHORIZED"); }
    return await db.Repair.Where(t => t.Completed).ToListAsync();
});
    

//GET SPECIFIC REPAIR RECOR
app.MapGet("/api/v1/repair/{id}", async (int id, RepairDB db) =>
    await db.Repair.FindAsync(id)
        is Record repair
            ? Results.Ok(repair)
            : Results.NotFound());

//UPDATE SPECIFIC REPAIR RECORD
app.MapPut("/api/v1/repair/{id}", async (int id, Record inputRepair, RepairDB db, HttpContext context) => {
    if (context.Request.Headers["Authorization"].ToString() != admin) { throw new Exception("USER NOT AUTHORIZED"); }
    var repair = await db.Repair.FindAsync(id);
    if (repair is null) return Results.NotFound();
    repair.Date = inputRepair.Date;
    repair.Completed = inputRepair.Completed;
    repair.Fee = inputRepair.Fee;
    repair.Tel = inputRepair.Tel;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

//REMOVE SPECIFIC REPAIR RECORD
app.MapDelete("/api/v1/repair/{id}", async (int id, RepairDB db, HttpContext context) => {
    if (context.Request.Headers["Authorization"].ToString() != admin) { throw new Exception("USER NOT AUTHORIZED"); }
    if (await db.Repair.FindAsync(id) is Record repair)
    {
        db.Repair.Remove(repair);
        await db.SaveChangesAsync();
        return Results.Ok(repair);
    }
    return Results.NotFound();
});



/* 
 SAFTEY INSPECTION METHODS
*/
app.MapGet("/api/v1/inspection", async (InspectionDB db, HttpContext context) => {
    if (context.Request.Headers["Authorization"].ToString() != admin) { throw new Exception("USER NOT AUTHORIZED"); }
    return await db.Inspection.ToListAsync();
});

app.MapPost("/api/v1/inspection", async (Record inspection, InspectionDB db, HttpContext context) => {
    if (context.Request.Headers["Authorization"].ToString() != admin) { throw new Exception("USER NOT AUTHORIZED"); }
    db.Inspection.Add(inspection);
    await db.SaveChangesAsync();
    return Results.Created($"/inspection/{inspection.Id}", inspection);
});

//GET COMPLETE INSPECTIONS
app.MapGet("/api/v1/inspection/completed", async (InspectionDB db, HttpContext context) => {
    if (context.Request.Headers["Authorization"].ToString() != admin) { throw new Exception("USER NOT AUTHORIZED"); }
    return await db.Inspection.Where(t => t.Completed).ToListAsync();
});

//GET SPECIFIC INSPECTION RECORD
app.MapGet("/api/v1/inspection/{id}", async (int id, InspectionDB db) =>
    await db.Inspection.FindAsync(id)
        is Record inspection
            ? Results.Ok(inspection)
            : Results.NotFound());

//UPDATE SPECIFIC INSPECTION RECORD
app.MapPut("/api/v1/inspection/{id}", async (int id, Record inputInspection, InspectionDB db, HttpContext context) => {
    if (context.Request.Headers["Authorization"].ToString() != admin) { throw new Exception("USER NOT AUTHORIZED"); }
    var inspection = await db.Inspection.FindAsync(id);
    if (inspection is null) return Results.NotFound();
    inspection.Date = inputInspection.Date;
    inspection.Completed = inputInspection.Completed;
    inspection.Fee = inputInspection.Fee;
    inspection.Tel = inputInspection.Tel;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

//REMOVE SPECIFIC INSPECTION RECORD
app.MapDelete("/api/v1/inspection/{id}", async (int id, InspectionDB db, HttpContext context) => {
    if (context.Request.Headers["Authorization"].ToString() != admin) { throw new Exception("USER NOT AUTHORIZED"); }
    if (await db.Inspection.FindAsync(id) is Record inspection)
    {
        db.Inspection.Remove(inspection);
        await db.SaveChangesAsync();
        return Results.Ok(inspection);
    }
    return Results.NotFound();
});





//AUTHENTICATE METHOD
app.MapGet("/api/v1/accounts", async (InspectionDB db, HttpContext context) => {
    if (context.Request.Headers["Authorization"].ToString() != admin) { return Results.Unauthorized(); }
    return Results.Ok("Authorized");
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
    public RepairDB(DbContextOptions<RepairDB> options)
        : base(options) { }
    public DbSet<Record> Repair => Set<Record>();
}


//INSPECTION IN MEM DB
class InspectionDB : DbContext
{
    public InspectionDB(DbContextOptions<InspectionDB> options)
        : base(options) { }
    public DbSet<Record> Inspection => Set<Record>();
}