The Unit of Work (UoW) pattern:
 - ✔ Groups multiple database operations into a single transaction
 - ✔ Ensures all operations succeed or none are saved
 - ✔ Keeps your repositories coordinated
 - ✔ Prevents partial updates if something fails
 - ✔ Centralizes SaveChanges() calls
  
In simple words:
 - Unit of Work = One transaction for all DB actions during a request.

THIS IS ESPECIALLY IMPORTANT FOR WEB API OPERATIONS THAT MODIFY MULTIPLE TABLES.



🎯 WHY USE UNIT OF WORK IN WEB API?
-----------------------------------------------------------------------------------
| Problem without UoW                              | How UoW solves it                      |
| ------------------------------------------------ | -------------------------------------- |
| Each repository calls `SaveChanges()` separately | One `SaveChanges()` for all operations |
| Partial updates if one operation fails           | Transaction rollback                   |
| Scattered DB logic                               | Centralized DB access                  |
| Hard to maintain / test                          | Clean abstraction layer                |



📦 TYPICAL WEB API SCENARIO
-----------------------------------------
Example: Creating an Order also creates OrderItems.
-------------------------------------------------------
- Without UoW:
```
    Repository1.SaveChanges()
    Repository2.SaveChanges()
    Repository3.SaveChanges()
```
→ If 2nd step fails, data becomes inconsistent.
- With UoW:
```
Do all operations
Call Save once
Transaction happens automatically
```




🧱 STEP-BY-STEP IMPLEMENTATION IN C# .NET WEB API
-------------------------------------------------------------
🧱 1. CREATE REPOSITORIES (SIMPLE EXAMPLE)
---------------------------------------------------------------------
- PRODUCT REPOSITORY:
```
public interface IProductRepository
{
    Task<Product> GetAsync(int id);
    void Add(Product product);
}

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;
    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Product> GetAsync(int id)
    {
        _context.Products.FindAsync(id).AsTask();
    } 

    public void Add(Product product)
    {
        _context.Products.Add(product);
    }
}
```
- ORDER REPOSITORY:
```
public interface IOrderRepository
{
    void Add(Order order);
}

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;
    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Add(Order order)
    {
        _context.Orders.Add(order);
    }
       
}
```
            
🧱 2. CREATE UNIT OF WORK INTERFACE
------------------------------------------------------------
```
public interface IUnitOfWork
{
    IProductRepository Products { get; }
    IOrderRepository Orders { get; }
    Task<int> CompleteAsync();
}
```
🧱 3. IMPLEMENT UNIT OF WORK
------------------------------------------------------------
```
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IProductRepository Products { get; }
    public IOrderRepository Orders { get; }

    public UnitOfWork(AppDbContext context,
                      IProductRepository productRepo,
                      IOrderRepository orderRepo)
    {
        _context = context;
        Products = productRepo;
        Orders = orderRepo;
    }

    public Task<int> CompleteAsync()
    {
        return _context.SaveChangesAsync();
    }
}
```
✔ CompleteAsync() is where all changes are committed.


🧱 4. REGISTER REPOSITORIES + UOW IN DI (PROGRAM.CS)
---------------------------------------------------------------------------
```
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
```


🧱 5. USE UNIT OF WORK IN A WEB API CONTROLLER
-----------------------------------------------------------------------------
```
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IUnitOfWork _uow;

    public OrdersController(IUnitOfWork uow)
    {
        _uow = uow;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
    {
        var product = await _uow.Products.GetAsync(dto.ProductId);

        if (product == null)
            return NotFound("Product not found");

        var order = new Order { ProductId = product.Id, Quantity = dto.Quantity };

        _uow.Orders.Add(order);

        // Only ONE call commits everything
        await _uow.CompleteAsync();                         <----- ONE CALL COMMITS EVERYTHING

        return Ok(order);
    }
}
```
----------------------------------------------------------------------------
- ✔ All DB writes are saved together
- ✔ If an error occurs, nothing is written
---------------------------------------------------------------------------




⭐ BENEFITS OF UNIT OF WORK IN WEB API
-----------------------------------------------------------------------------
| Benefit                           | Explanation                             |
| --------------------------------- | --------------------------------------- |
| **Atomic Transactions**           | All DB operations succeed or roll back  |
| **Cleaner Services**              | Only one call to save                   |
| **Decoupling**                    | Controller does not worry about EF Core |
| **Easier Testing**                | Mock repositories + UoW                 |
| **Reduces duplicate SaveChanges** | One commit at the end                   |



🧠 HOW UNIT OF WORK WORKS WITH EF CORE?
----------------------------------------------
- EF Core's DbContext already:
    - Tracks changes
    - Performs transactions around SaveChanges()
    - Allows multiple repositories to use the same context
- So UoW simply exposes those capabilities in a cleaner architecture.




⚠️ WHEN NOT TO USE UNIT OF WORK?
------------------------------------------------------------------------
- If you use only EF Core repositories & services → EF itself is UoW
- For extremely small applications
- When every API only performs a single DB operation
- But many teams still use UoW for cleaner, testable architecture.

