- Definition:
  - High-level modules should not depend on low-level modules. Both should depend on abstractions (interfaces).
  - AND
  - Abstractions should not depend on details, Details should depend on abstractions.
- Use interfaces instead of concrete classes, and inject dependencies using DI.


🎯 WHY DIP IS IMPORTANT IN .NET?
----------------------------------------------------------
- Without DIP:
    - ❌ Your controllers tightly depend on concrete services
    - ❌ Hard to replace services (ex: PayPal → Stripe)
    - ❌ Difficult to unit test
    - ❌ Hard to scale
    - ❌ More bugs
- With DIP:
    - ✔ Loosely coupled code
    - ✔ Swappable implementations
    - ✔ Better testability
    - ✔ Clean architecture
    - ✔ Works perfectly with .NET Dependency Injection (DI)




❌ BAD EXAMPLE (VIOLATES DIP)
--------------------------------------------------      
Controller directly depends on a CONCRETE class:
```
public class PaymentController : ControllerBase
{
    private readonly PaypalPaymentService _payment;

    public PaymentController()
    {
        _payment = new PaypalPaymentService();
    }

    [HttpPost]
    public IActionResult Pay()
    {
        _payment.Pay();
        return Ok();
    }
}
```
❌ Problems:
- Controller is hardcoded to PayPal
- Can't change to Stripe or RazorPay without modifying controller
- Not testable
- Not extendable <br>
This violates DIP because high-level module (controller) depends on low-level module (PaypalPaymentService).


<br>

✅ GOOD EXAMPLE (FOLLOWS DIP)
--------------------------------------------------
STEP 1: CREATE AN ABSTRACTION (INTERFACE)
---------------------------------------------------
```
public interface IPaymentService
{
    void Pay();
}
```

STEP 2: CONCRETE IMPLEMENTATIONS DEPEND ON THE ABSTRACTION
---------------------------------------------------------
```
//PayPal implementation:
public class PaypalPaymentService : IPaymentService
{
    public void Pay()
    {
        // PayPal logic
    }
}

//Stripe implementation:
public class StripePaymentService : IPaymentService
{
    public void Pay()
    {
        // Stripe logic
    }
}
```
✔ Now concrete classes depend on interface, not controller.


STEP 3: CONTROLLER DEPENDS ON THE ABSTRACTION (NOT IMPLEMENTATION)
-------------------------------------------------------------------
```
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _payment;

    public PaymentController(IPaymentService payment)
    {
        _payment = payment;
    }

    [HttpPost]
    public IActionResult Pay()
    {
        _payment.Pay();
        return Ok("Payment Successful");
    }
}
```
- ✔ No direct dependency on PayPal or Stripe
- ✔ Controller is stable and clean
- ✔ Follows DIP perfectly


STEP 4: WIRE THE IMPLEMENTATION IN PROGRAM.CS
------------------------------------------------------
- Use PayPal:
    - `builder.Services.AddScoped<IPaymentService, PaypalPaymentService>();`
- Switch to Stripe (without editing controller!)
    - `builder.Services.AddScoped<IPaymentService, StripePaymentService>();`

Summary:
-----------------------
- ✔ No controller changes
- ✔ No code modifications
- ✔ Only configuration changes
- This is Dependency Inversion + Dependency Injection working together.
