using TuberTreats.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
List<Customer> customers = new List<Customer>
{
    new Customer { Id = 1, Name = "Alice Smith", Address = "123 Maple Street, Springfield" },
    new Customer { Id = 2, Name = "Bob Johnson", Address = "456 Oak Avenue, Metropolis" },
    new Customer { Id = 3, Name = "Catherine Brown", Address = "789 Pine Road, Gotham" },
    new Customer { Id = 4, Name = "David Wilson", Address = "101 Birch Boulevard, Star City" },
    new Customer { Id = 5, Name = "Evelyn Turner", Address = "202 Cedar Lane, Central City" }
};
List<TuberDriver> tuberDrivers = new List<TuberDriver>
{
    new TuberDriver { Id = 1, Name = "John Doe" },
    new TuberDriver { Id = 2, Name = "Jane Smith" },
    new TuberDriver { Id = 3, Name = "Michael Johnson" },

};

List<Topping> toppings = new  List<Topping>
{
    new Topping { Id = 1, Name = "Pepperoni" },
    new Topping { Id = 2, Name = "Mushrooms" },
    new Topping { Id = 3, Name = "Onions" },
    new Topping { Id = 4, Name = "Green Peppers" },
    new Topping { Id = 5, Name = "Sausage" }
};

List<TuberOrder> orders = new List<TuberOrder>
{
    new TuberOrder
    {
        Id = 1,
        OrderPlacedOnDate = new DateTime(2024, 11, 1, 14, 30, 0),
        CustomerId = 1,
        TuberDriverId = 1,
        DeliveredOnDate = new DateTime(2024, 11, 1, 15, 0, 0)
    },
    new TuberOrder
    {
        Id = 2,
        OrderPlacedOnDate = new DateTime(2024, 11, 2, 10, 0, 0),
        CustomerId = 2,
        TuberDriverId = 2,
        DeliveredOnDate = new DateTime(2024, 11, 2, 10, 45, 0)
    },
    new TuberOrder
    {
        Id = 3,
        OrderPlacedOnDate = new DateTime(2024, 11, 3, 16, 15, 0),
        CustomerId = 3,
        TuberDriverId = 3,
        DeliveredOnDate = new DateTime(2024, 11, 3, 17, 0, 0)
    }
};

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//add endpoints here

app.Run();
//don't touch or move this!
public partial class Program { }