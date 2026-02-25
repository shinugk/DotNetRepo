- Definition:
  - High-level modules should not depend on low-level modules. Both should depend on abstractions (interfaces).
  - Use interfaces instead of concrete classes, and inject dependencies using DI.


❌ BAD EXAMPLE (VIOLATES DIP)
--------------------------------------------------      
Controller directly depends on a CONCRETE class:
```
public class PaymentController : ControllerBase
{
    private readonly PaypalPaymentService _payment;   //depends on concrete class 

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
Problems: This violates DIP because high-level module (controller) depends on low-level module (PaypalPaymentService).


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
    private readonly IPaymentService _payment;        //having abstraction here so controller does not care which service is served

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
- ✔ No direct dependency on PayPal or Stripe. It depends on Abstraction. whichever is registered in DI that is served to controller.
- ✔ No changes to controller needed


STEP 4: WIRE THE IMPLEMENTATION IN PROGRAM.CS
------------------------------------------------------
- Use PayPal:
    - `builder.Services.AddScoped<IPaymentService, PaypalPaymentService>();`
- Switch to Stripe (without editing controller!)
    - `builder.Services.AddScoped<IPaymentService, StripePaymentService>();`

