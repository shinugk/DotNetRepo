- The Abstract Factory pattern creates related objects without specifying their concrete classes.
- It is a factory that creates other factories.
- Used when:
  - You want to create families of related objects
  - You want each family to follow the same theme or rules

 
📦 REAL-WORLD EXAMPLE: UI THEMES (LIGHT THEME, DARK THEME)
---------------------------------------------------------
- Imagine you want two UI themes:
    - Light Theme: Light Button, Light Checkbox
    - Dark Theme: Dark Button, Dark Checkbox
- The client should NOT know which concrete classes are being used.

----------------------------------------------------------------
STEP 1: PRODUCT INTERFACES
```
public interface IButton
{
    void Render();
}

public interface ICheckbox
{
    void Check();
}
```
-----------------------------------------------------------------
STEP 2: CONCRETE PRODUCTS
```
public class LightButton : IButton
{
    public void Render() => Console.WriteLine("Light Button");
}

public class DarkButton : IButton
{
    public void Render() => Console.WriteLine("Dark Button");
}

public class LightCheckbox : ICheckbox
{
    public void Check() => Console.WriteLine("Light Checkbox");
}

public class DarkCheckbox : ICheckbox
{
    public void Check() => Console.WriteLine("Dark Checkbox");
}
```
-------------------------------------------------------------
STEP 3: ABSTRACT FACTORY
```
public interface IUIFactory
{
    IButton CreateButton();
    ICheckbox CreateCheckbox();
}
```
-----------------------------------------------------------------
STEP 4: CONCRETE FACTORIES
```
public class LightThemeFactory : IUIFactory
{
    public IButton CreateButton() => new LightButton();
    public ICheckbox CreateCheckbox() => new LightCheckbox();
}

public class DarkThemeFactory : IUIFactory
{
    public IButton CreateButton() => new DarkButton();
    public ICheckbox CreateCheckbox() => new DarkCheckbox();
}
```
------------------------------------------------------------------
STEP 5: CLIENT CODE (USES FACTORY WITHOUT KNOWING WHICH OBJECTS IT CREATES)
```
public class Application
{
    private readonly IButton _button;
    private readonly ICheckbox _checkbox;

    public Application(IUIFactory factory)
    {
        _button = factory.CreateButton();
        _checkbox = factory.CreateCheckbox();
    }

    public void Render()
    {
        _button.Render();
        _checkbox.Check();
    }
}
```
-------------------------------------------------------------------
Usage:
```
IUIFactory factory = new DarkThemeFactory();
var app = new Application(factory);
app.Render();
```
----------------------------------------------------------------------

NOTE:
---------------------------------------------
- ✔ Creates consistent families of objects
- ✔ Client does NOT know the concrete classes
- ✔ Easy to switch themes (just change factory)


🎯 WHEN TO USE ABSTRACT FACTORY?
-------------------------------------------
- Use it when:
  - You have multiple related products
  - You want to ensure compatibility
      (e.g., LightButton must match LightCheckbox)
  - You want to switch families easily (Light → Dark)
 
 FACTORY METHOD VS ABSTRACT FACTORY
 ----------------------------------------------------------------------------------
 | Feature    | Factory Method                | Abstract Factory                |
 | ---------- | ----------------------------- | ------------------------------- |
 | Creates    | One object                    | Many related objects            |
 | Structure  | One factory method            | Factory of factories            |
 | Use Case   | Decide which object to create | Create related object families  |
 | Example    | ShapeFactory → Circle/Square  | ThemeFactory → Button, Checkbox |
 | Complexity | Simple                        | More complex                    |
