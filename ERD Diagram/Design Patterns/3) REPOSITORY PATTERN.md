- **Category**: Modern/Architectural
- **Description**: Encapsulates data access logic, providing a collection-like interface for accessing domain objects, abstracting the data layer.
  - ✔ Separates data-access logic from business logic
  - ✔ Provides a clean abstraction over data operations
  - ✔ Makes code easier to maintain, test, and mock
  - ✔ Avoids repeating CRUD logic everywhere
- **Why Important**: Widely used with Entity Framework Core to simplify database operations, improve testability, and separate business logic from data access.
- **Use Cases**: 
  - CRUD operations in WebAPIs.
  - Abstracting database interactions in microservices.
- In simple words:
  - Repository acts as a middle layer between your application and the database.
  - Your services/controllers talk to repository instead of directly talking to EF Core or SQL.



🧱 WITHOUT REPOSITORY (TIGHTLY COUPLED CODE)
---------------------------------------------------------------------
```
public class ProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public List<Product> GetProducts()
    {
        return _context.Products.ToList();     // <------------ DIRECT EF CORE CALL
    }
}
```
**Problems:**
- Business layer knows EF Core details
- Hard to unit test (DbContext is not mock-friendly)
- No central place for queries


🧱 WITH REPOSITORY (LOOSE COUPLING)
-------------------------------------------------------------------------
- You create a repository interface + class that handles all DB operations.
- Service layer uses repository instead of DbContext.
------------------------------------------------------------------------
✔ STEP 1: CREATE MODEL
```
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
}
```

------------------------------------------------------------------------
✔ STEP 2: CREATE REPOSITORY INTERFACE
```
public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAll();
    Task<Product> GetById(int id);
    Task Add(Product product);
    Task Update(Product product);
    Task Delete(int id);
}
```
-----------------------------------------------------------------------
✔ STEP 3: IMPLEMENT REPOSITORY
```
public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAll()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product> GetById(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task Add(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}
```
-------------------------------------------------------------------------
✔ STEP 4: REGISTER IN DI (PROGRAM.CS)
```
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddDbContext<AppDbContext>();
```
-------------------------------------------------------------------------
✔ STEP 5: USE REPOSITORY IN CONTROLLER / SERVICES
```
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repo;

    public ProductsController(IProductRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _repo.GetAll());
    }
}
```

-----------------------------------------------------------------------
Now your controller is clean and loosely coupled.
------------------------------------------------------------------------



🎯 WHY USE THE REPOSITORY PATTERN?
------------------------------------------------------------------------------------
| Benefit                         | Explanation                                        |
| ------------------------------  | -------------------------------------------------- |
| ✔ Loosely Coupled              | Business layer does not depend on EF Core          |
| ✔ Easy Unit Testing            | You can mock the repository                        |
| ✔ Clean Separation of Concerns | DB code and business logic are separated           |
| ✔ Reusable                     | CRUD logic reused across controllers               |
| ✔ Helps follow SOLID           | Especially the **Single Responsibility Principle** |



❌ WHEN NOT TO USE REPOSITORY PATTERN?
---------------------------------------------------------------------------------
- When using EF Core, which is already a repository/unit-of-work implementation
- When abstraction becomes unnecessary and adds extra layers
- For very complex queries where EF DbContext is easier to use directly



🔚 SUMMARY
✔ Repository is a middle layer between your app and DB
✔ Improves testability, clean architecture, loose coupling
✔ Uses interfaces and DI
✔ Keeps controllers/service layers clean
✔ Optional: use generic repositories
