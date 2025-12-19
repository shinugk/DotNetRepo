 - Factory Method is a creational design pattern that delegates object creation to subclasses or a dedicated method.
 - Instead of using new everywhere, you create objects using a factory method.
 - Acceptable when:
   - You want to centralize object creation
   - you want to create different types based on conditions
   - You want to avoid tightly coupling your code with concrete classes

 📌 EXAMPLE: FACTORY METHOD (SIMPLE FACTORY)
 ----------------------------------------------------------------
 Step 1: Create Interface / Base Class
 ```
 public interface IShape
 {
     void Draw();
 }

 public class Circle : IShape
 {
     public void Draw() => Console.WriteLine("Drawing Circle");
 }

 public class Square : IShape
 {
     public void Draw() => Console.WriteLine("Drawing Square");
 }
```
 -------------------------------------------------------------------
 Step 2: Factory Method
 ```
 public static class ShapeFactory
 {
     public static IShape GetShape(string type)
     {
         return type.ToLower() switch
         {
             "circle" => new Circle(),
             "square" => new Square(),
             _ => throw new Exception("Invalid shape")
         };
     }
 }
```
 ------------------------------------------------------------------
Usage:
```
 IShape shape = ShapeFactory.GetShape("circle");
 shape.Draw();
```
 -----------------------------------------------------------------
 - ✔ Centralized object creation
 - ✔ No need to modify multiple places if logic changes
