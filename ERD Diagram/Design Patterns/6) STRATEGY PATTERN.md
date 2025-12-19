 - Category: Behavioral
 - Description: Defines a family of algorithms, encapsulates each one, and makes them interchangeable at runtime.
 - Why Important: Enables runtime selection of behaviors, which is useful for implementing features like payment processing, sorting algorithms, or validation rules.
 - Use Cases: 
    - Selecting payment providers(e.g., PayPal, Stripe).
    - Dynamic validation or business rule application.

 - In simple words:
   - Strategy Pattern lets you switch algorithms (behavior) at runtime.


 🧠 WHY USE STRATEGY PATTERN?
 ------------------------------------
 Use it when:
 - You have multiple ways of doing the same task
 - You want to avoid long if/else or switch statements
 - You want to follow the Open-Closed Principle (OCP) — add new strategies without modifying existing code


 📦 REAL-LIFE ANALOGY
 ----------------------------------------------
 - Think of payment options at checkout:
     - Pay by UPI
     - Pay by Credit Card
     - Pay by Cash
 - The checkout process is the same, only the payment method changes.
 - This is exactly how Strategy Pattern works.



 --------------------------------------------------------------
 🧱 1. Define the Strategy Interface
 ```
 public interface IPaymentStrategy
 {
     void Pay(double amount);
 }
```
 ----------------------------------------------------------------
 🛠 2. Concrete Strategy Classes 
 ```
 ------Payment by UPI:----------
 public class UpiPayment : IPaymentStrategy
 {
     public void Pay(double amount)
     {
         Console.WriteLine($"Paid {amount} using UPI");
     }
 }

 -------Payment by Credit Card:-------
 public class CreditCardPayment : IPaymentStrategy
 {
     public void Pay(double amount)
     {
         Console.WriteLine($"Paid {amount} using Credit Card");
     }
 }

 ---------Payment by Cash:---------
 public class CashPayment : IPaymentStrategy
 {
     public void Pay(double amount)
     {
         Console.WriteLine($"Paid {amount} in Cash");
     }
 }
```
 -------------------------------------------------------------------
 🧩 3. Context Class (uses a strategy)
 ```
 public class PaymentService
 {
     private IPaymentStrategy _paymentStrategy;

     public void SetPaymentStrategy(IPaymentStrategy strategy)
     {
         _paymentStrategy = strategy;
     }

     public void MakePayment(double amount)
     {
         _paymentStrategy.Pay(amount);
     }
 }
```
 ---------------------------------------------------------------------
 🚀 4. Usage (Switch strategy at runtime)
 ```
 var paymentService = new PaymentService();

 paymentService.SetPaymentStrategy(new UpiPayment());
 paymentService.MakePayment(500);

 paymentService.SetPaymentStrategy(new CreditCardPayment());
 paymentService.MakePayment(1000);
```
 ----------------------------------------------------------------------
 Output:
 - Paid 500 using UPI
 - Paid 1000 using Credit Card
---------------------------------------------------------------------------



 ⭐ BENEFITS OF STRATEGY PATTERN
 ------------------------------------------------------------------------------------
 | Benefit                             | Explanation                                  |
 | ----------------------------------- | -------------------------------------------- |
 | **Removes if-else complexity**      | No long conditional blocks                   |
 | **Open-Closed Principle**           | Add new strategies without touching old code |
 | **Easily interchangeable behavior** | Change algorithms at runtime                 |
 | **Better Testing**                  | Each strategy can be unit-tested separately  |
 | **Cleaner and maintainable code**   | Behavior is separated into small classes     |



 ⚠️ WHEN NOT TO USE STRATEGY PATTERN
 -------------------------------------------------------------------------
 Avoid this pattern when:
 - You only have one behavior (no variations)
 - Variations are very small and do not require full separate classes
 - Too many small strategy classes make the design over-complicated


 📌 Real .NET Examples of Strategy Pattern
 -----------------------------------------------------------------------
 - ASP.NET Core Authentication
    - Different authentication schemes (JWT, Cookies, OAuth) are strategies.
 - Sorting algorithms
    - You can switch between QuickSort, MergeSort, BubbleSort strategies.
 - Compression algorithms
    - Zip vs Rar vs Tar compression strategies.
