
SOLID represents five design principles that help create maintainable, scalable, and testable software systems.


| Principle | Meaning | WebAPI Example |
| --------- | -------------------------------------------- | -------------------------------------------------------- |
| SRP | One responsibility per class                 | Controller → Service → Repository separation             |
| OCP       | Extend without modifying                     | New loggers without editing existing code                |
| LSP       | Child class should not break parent behavior | Separate read/write repositories                         |
| ISP       | Small, focused interfaces                    | EmailService + SmsService instead of large INotification |
| DIP       | Depend on abstractions                       | Use interfaces + dependency injection                    |
