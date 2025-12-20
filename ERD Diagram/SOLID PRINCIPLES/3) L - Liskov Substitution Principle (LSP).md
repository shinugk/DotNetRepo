 - Definition : 
   - A child class should be usable in place of its parent class without breaking the system.
 - Meaning: <br>
   ✔ If class B inherits A,  <br>
   ✔ You should be able to use B anywhere A is expected, <br>
   ❌ without errors, exceptions, or changed behavior.

 🔥 WHY LSP IS IMPORTANT?
------------------------------------
 - Because violating LSP leads to:
     - ❌ Unexpected exceptions
     - ❌ Broken polymorphism
     - ❌ Confusing behaviors
     - ❌ Fragile API design
 - LSP ensures:
     - ✔ Reliable inheritance
     - ✔ Clean architecture
     - ✔ Predictable behavior
     - ✔ Safely usable base classes


 ❌ BAD EXAMPLE (VIOLATES LSP):
 -------------------------------------------
 ```
--------WE HAVE A BASE REPOSITORY--------
 public class UserRepository                              
 {
     public virtual void Add(User user)
     {
         // Save to DB
     }
 }

------NOW SOMEONE CREATES A READ-ONLY REPOSITORY BY INHERITING THIS-------------
 public class ReadOnlyUserRepository : UserRepository          
 {
     public override void Add(User user)
     {
         throw new NotImplementedException("Cannot add user");
     }
 }
```
 
 ❌ PROBLEM:
 -------------------------------------
 ANYWHERE YOU USE:
 ```
     UserRepository repo = new ReadOnlyUserRepository();
     repo.Add(user); // CRASHES!
```
 This violates LSP because child class behaves differently from parent.


<br>

 ✅ CORRECT DESIGN (FOLLOWS LSP) (Split responsibilities using interfaces)
 ----------------------------------------------------

 STEP 1: SEPARATE READ AND WRITE CONTRACTS
 -------------------------------------------------
 ```
 public interface IReadRepository<T>
 {
     T Get(int id);
 }

 public interface IWriteRepository<T>
 {
     void Add(T entity);
 }
```

 STEP 2: IMPLEMENT THEM PROPERLY
 -----------------------------------------------             
**Read-only repository (implements only read)**
```
 public class ReadOnlyUserRepository : IReadRepository<User>       
 {
     public User Get(int id)
     {
         return new User { Id = id };
     }
 }
```
 **Full repository (implements read + write)**
```
 public class UserRepository  : IReadRepository<User>, IWriteRepository<User>   
 {
     public User Get(int id)
     {
         // return from DB
         return new User { Id = id };
     }

     public void Add(User entity)
     {
         // save to DB
     }
 }
```

 🧠 WHY DOES THIS FOLLOW LSP?
 ----------------------------------------------
 Because now:
 - ReadOnlyUserRepository is never forced to implement Add()
 - No overridden method throws unexpected exceptions
 - No child class breaks behavior of parent
 - Calling code is predictable and safe
