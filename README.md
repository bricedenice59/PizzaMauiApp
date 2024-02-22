# The Application

The objective of this application is to create the back-end and front-end code for a small demo application of a online pizza ordering using latest .NET technologies, i.e .NET MAUI with .NET 8 thats runs on both iPhone and Android OS.

All resources (images, icons and text descriptions) were generated with ChatGPT + DALL-E 3.

![](https://github.com/bricedenice59/PizzaMauiApp/blob/master/2024-01-04_10-30-27.gif)

## External APIs

This is the list of microservices API/libraries required for the application to be running.

- Shared library available as nugget package ([https://github.com/bricedenice59/PizzaMauiAppAPI](https://github.com/bricedenice59/PizzaMauiApp.API.Shared))

External services (API) for processing orders:
- Store and Identity management API (https://github.com/bricedenice59/PizzaMauiAppAPI)
- Order processing microservice API (https://github.com/bricedenice59/PizzaMauiApp.API.Orders)
- Kitchen microservice order state API (https://github.com/bricedenice59/PizzaMauiApp.API.Orders.Kitchen)

DTOs libraries (nugget packages available):
- DTOs for API ([https://github.com/bricedenice59/PizzaMauiAppAPI](https://github.com/bricedenice59/PizzaMauiApp.API.Dtos))
- RabbitMQ common interfaces ([https://github.com/bricedenice59/PizzaMauiApp.API.Orders](https://github.com/bricedenice59/PizzaMauiApp.RabbitMQ.Messages))


## TODOs
- [ ] Process Orders (coming soon...)
- [ ] Showing order processing steps (coming soon...)
- [ ] Fix some UI issues
- [ ] Android: Call API on dev machine is still impossible, have spent days to try fixing that but no solution at the moment.
- [ ] Upload some images to a server instead of using embedeed pictures.
