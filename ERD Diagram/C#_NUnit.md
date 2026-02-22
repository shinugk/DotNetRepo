Minimal Required Packages for NUnit + Moq
------------------------------------------
dotnet add package NUnit
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package Moq

Why Moq?
--------
Moq is used to isolate unit tests by mocking dependencies so that tests do not rely on external systems like databases or APIs.

Sample Example:
-----------------------------------
- We are testing a UserService that depends on a repository.
- Dependencies will be mocked using Moq.
```
using NUnit.Framework;
using Moq;
using System;
using System.Threading.Tasks;

namespace DemoTests
{
    // Dependency interface
    public interface IUserRepository
    {
        string GetUserName(int id);
        Task<string> GetUserNameAsync(int id);
    }

    // Service we want to test
    public class UserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public string GetUser(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid Id");

            return _repository.GetUserName(id);
        }

        public async Task<string> GetUserAsync(int id)
        {
            return await _repository.GetUserNameAsync(id);
        }
    }
```
```
    // ================= TEST CLASS =================

    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _mockRepo;
        private UserService _service;

        // Runs once before all tests
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Console.WriteLine("Starting Test Suite...");
        }

        // Runs before each test
        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IUserRepository>();
            _service = new UserService(_mockRepo.Object);
        }

        // Runs after each test
        [TearDown]
        public void Cleanup()
        {
            Console.WriteLine("Test Finished");
        }

        // Runs once after all tests
        [OneTimeTearDown]
        public void OneTimeCleanup()
        {
            Console.WriteLine("All Tests Completed");
        }

        // ================= BASIC TEST =================

        [Test]
        public void GetUser_ValidId_ReturnsUserName()
        {
            // Arrange
            _mockRepo.Setup(x => x.GetUserName(1))
                     .Returns("John");

            // Act
            var result = _service.GetUser(1);

            // Assert
            Assert.AreEqual("John", result);
        }

        // ================= EXCEPTION TEST =================

        [Test]
        public void GetUser_InvalidId_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => _service.GetUser(0));
        }

        // ================= VERIFY METHOD CALL =================

        [Test]
        public void GetUser_ShouldCallRepositoryOnce()
        {
            _mockRepo.Setup(x => x.GetUserName(1))
                     .Returns("John");

            var result = _service.GetUser(1);

            _mockRepo.Verify(x => x.GetUserName(1), Times.Once);
        }

        // ================= PARAMETERIZED TEST =================

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetUser_MultipleIds_ReturnsValue(int id)
        {
            _mockRepo.Setup(x => x.GetUserName(It.IsAny<int>()))
                     .Returns("TestUser");

            var result = _service.GetUser(id);

            Assert.AreEqual("TestUser", result);
        }

        // ================= ASYNC TEST =================

        [Test]
        public async Task GetUserAsync_ReturnsUserName()
        {
            _mockRepo.Setup(x => x.GetUserNameAsync(1))
                     .ReturnsAsync("AsyncUser");

            var result = await _service.GetUserAsync(1);

            Assert.AreEqual("AsyncUser", result);
        }

        // ================= MOCK THROW EXCEPTION =================

        [Test]
        public void Repository_ThrowsException_ShouldPropagate()
        {
            _mockRepo.Setup(x => x.GetUserName(1))
                     .Throws(new Exception("DB Error"));

            Assert.Throws<Exception>(() => _service.GetUser(1));
        }

        // ================= SETUP SEQUENCE =================

        [Test]
        public void GetUser_SetupSequence_Test()
        {
            _mockRepo.SetupSequence(x => x.GetUserName(1))
                     .Returns("First")
                     .Returns("Second");

            var result1 = _service.GetUser(1);
            var result2 = _service.GetUser(1);

            Assert.AreEqual("First", result1);
            Assert.AreEqual("Second", result2);
        }

        // ================= IGNORE TEST =================

        [Test]
        [Ignore("Not implemented yet")]
        public void FutureTest()
        {
        }
    }
```

<br>

Basics:
------------------
1) [TestFixture] : Marks this class as test class
2) Mock Object Creation : private Mock<IUserRepository> _mockRepo;
    - Moq creates a fake repository. Real database is NOT used.
3) Injecting Mock into Service : _service = new UserService(_mockRepo.Object);
    - .Object gives the fake implementation. This is called Dependency Injection in Unit Testing.
4) [SetUp] : Runs before every test.
    - Used for: ✔ Creating mocks ✔ Initializing objects
5) [OneTimeSetUp] : Runs once before all tests.
    - Used for: ✔ Expensive setup ✔ Logging
6) AAA Pattern : Always follow this.
    ```
   // Arrange
   // Act
   // Assert
    ```
7) Parameterized Test: 
```
  [TestCase(1)]
  [TestCase(2)]
  [TestCase(3)]
```
8) [TearDown] : Runs after each test. Used for cleanup.


<br>


How to test Repository using EF Core InMemory Database
----------------------------------------------------------
Normally repository uses real DB: SQL Server/ MySQL/ Oracle

In Unit Tests we use:
✅ EF Core InMemory Provider

Benefits:
✔ Fast
✔ No real DB needed
✔ Isolated tests
✔ Easy setup

Install Required Package: <dotnet add package Microsoft.EntityFrameworkCore.InMemory>

Example:
--------------------
Repository
```
public interface IUserRepository
{
    Task<User> GetByIdAsync(int id);
    Task AddAsync(User user);
    Task<List<User>> GetAllAsync();
}
```
Implementation
```
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }
}
```
Repository Test Using InMemory DB (IMPORTANT)
```
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

[TestFixture]
public class UserRepositoryTests
{
    private AppDbContext _context;
    private UserRepository _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        _context = new AppDbContext(options);
        _repository = new UserRepository(_context);

        // Seed data
        _context.Users.Add(new User { Id = 1, Name = "John" });
        _context.Users.Add(new User { Id = 2, Name = "Jane" });
        _context.SaveChanges();
    }

    [TearDown]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    // ================= GET BY ID =================

    [Test]
    public async Task GetByIdAsync_ShouldReturnUser()
    {
        var user = await _repository.GetByIdAsync(1);

        Assert.IsNotNull(user);
        Assert.AreEqual("John", user.Name);
    }

    // ================= ADD USER =================

    [Test]
    public async Task AddAsync_ShouldAddUser()
    {
        var newUser = new User { Id = 3, Name = "Bob" };

        await _repository.AddAsync(newUser);

        var user = await _context.Users.FindAsync(3);

        Assert.IsNotNull(user);
        Assert.AreEqual("Bob", user.Name);
    }

    // ================= GET ALL =================

    [Test]
    public async Task GetAllAsync_ShouldReturnAllUsers()
    {
        var users = await _repository.GetAllAsync();

        Assert.AreEqual(2, users.Count);
    }
}
```

<br>


Interview Questions:
----------------------------
1) Why use InMemory instead of Moq for Repository?
    - Answer: Repository contains EF Core logic. Mocking DbContext is complex and unreliable. InMemory provider allows testing actual EF behavior without real database.
  
2) How do you unit test a service with dependencies?
    - Answer: Use Moq to mock dependencies and NUnit for assertions.

3) When to Use Moq vs InMemory
| Scenario            | Use                     |
| ------------------- | ----------------------- |
| Service testing     | Moq                     |
| Repository testing  | InMemory                |
| Controller testing  | Moq                     |
| Integration testing | Real DB / TestContainer |

4) Which assertion style do you prefer?
    - Answer: I prefer Assert.That() because it is more readable and flexible than traditional assertions.
  
5) All Nunit Assert statements
```
Assert.AreEqual(expected, actual);              // Checks if expected value equals actual value
Assert.AreNotEqual(notExpected, actual);        // Checks if value is NOT equal
Assert.IsTrue(condition);                       // Passes if condition is true
Assert.IsFalse(condition);                      // Passes if condition is false
Assert.IsNull(objectValue);                     // Passes if object is null
Assert.IsNotNull(objectValue);                  // Passes if object is NOT null
Assert.AreSame(obj1, obj2);                     // Passes if both references point to same object in memory
Assert.AreNotSame(obj1, obj2);                  // Passes if objects are different references
Assert.IsInstanceOf<Type>(objectValue);         // Checks object is exact type
Assert.IsAssignableFrom<Type>(objectValue);     // Checks object can be assigned to type (inheritance supported)
Assert.Contains(item, collection);              // Checks collection contains item (simple contains)

CollectionAssert.AreEqual(expectedList, actualList);  // Checks two collections are equal in order and values
CollectionAssert.Contains(collection, item);   // Checks collection contains item
CollectionAssert.IsEmpty(collection);          // Passes if collection is empty
CollectionAssert.IsNotEmpty(collection);       // Passes if collection is NOT empty

Assert.Greater(actual, threshold);             // Checks actual > threshold
Assert.Less(actual, threshold);                // Checks actual < threshold
Assert.GreaterOrEqual(actual, value);          // Checks actual >= value
Assert.LessOrEqual(actual, value);             // Checks actual <= value
Assert.Throws<ExceptionType>(() => method());  // Passes if method throws expected exception
Assert.DoesNotThrow(() => method());           // Passes if method does NOT throw exception
Assert.ThrowsAsync<ExceptionType>(async () => await method()); // Checks async method throws exception
Assert.Pass("message");                        // Forces test to pass immediately
Assert.Fail("message");                        // Forces test to fail immediately
Assert.Warn("message");                        // Generates warning but does not fail test

// ================= MODERN ASSERT.THAT STYLE (RECOMMENDED) =================
Assert.That(actual, Is.EqualTo(expected));     // Modern equality check (recommended style)
Assert.That(actual, Is.Not.EqualTo(value));    // Modern inequality check
Assert.That(condition, Is.True);               // Modern true check
Assert.That(condition, Is.False);              // Modern false check
Assert.That(objectValue, Is.Null);             // Modern null check
Assert.That(objectValue, Is.Not.Null);         // Modern not-null check
Assert.That(objectValue, Is.InstanceOf<Type>()); // Checks object type
Assert.That(number, Is.InRange(min, max));     // Checks number is inside range
Assert.That(collection, Has.Count.EqualTo(n)); // Checks collection count
Assert.That(collection, Does.Contain(item));   // Checks collection contains item
Assert.That(stringValue, Does.StartWith(text)); // Checks string starts with text
Assert.That(stringValue, Does.EndWith(text));   // Checks string ends with text
Assert.That(stringValue, Does.Contain(text));   // Checks substring exists
Assert.That(stringValue, Is.EqualTo(text).IgnoreCase); // Case-insensitive string comparison
Assert.That(() => method(), Throws.TypeOf<ExceptionType>()); // Modern exception check

// ================= MULTIPLE ASSERTIONS =================
Assert.Multiple(() =>
{
Assert.AreEqual(expected1, actual1);       // First assertion inside multiple block
Assert.AreEqual(expected2, actual2);       // Second assertion (all failures reported together)
});
```

