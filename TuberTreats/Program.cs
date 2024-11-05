using System.Reflection.Metadata.Ecma335;
using TuberTreats.Models;
using TuberTreats.Models.DTOs;

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
        TuberDriverId = 1
        
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

List<TuberTopping> tuberToppings = new List<TuberTopping>
{
    new TuberTopping { Id = 1, TuberOrderId = 1, ToppingId = 1 }, 
    new TuberTopping { Id = 2, TuberOrderId = 1, ToppingId = 3 }, 
    new TuberTopping { Id = 3, TuberOrderId = 2, ToppingId = 2 }, 
    new TuberTopping { Id = 4, TuberOrderId = 2, ToppingId = 4 }, 
    new TuberTopping { Id = 5, TuberOrderId = 3, ToppingId = 1 }, 
    new TuberTopping { Id = 6, TuberOrderId = 3, ToppingId = 5 }
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
app.MapGet("/tuberorders", () => {

   return orders.Select(order => {
        Customer  customer = customers.FirstOrDefault(customer => customer.Id == order.CustomerId);
        TuberDriver tuberDriver = tuberDrivers.FirstOrDefault(td => td.Id == order.TuberDriverId);
        List<Topping> ordertoppings = tuberToppings
        .Where(tt => tt.TuberOrderId == order.Id)
        .Select(t => toppings.FirstOrDefault(topping => topping.Id == t.ToppingId)).ToList();
        order.DeliveredOnDate = order.DeliveredOnDate;
      return order;
    });
});

app.MapGet("/tuberorders/{id}", (int id) => {

    TuberOrder order = orders.FirstOrDefault(o => o.Id == id);
    Customer  customer = customers.FirstOrDefault(customer => customer.Id == order.CustomerId);
    TuberDriver tuberDriver = tuberDrivers.FirstOrDefault(td => td.Id == order.TuberDriverId);
    List<Topping> ordertoppings = tuberToppings
    .Where(tt => tt.TuberOrderId == order.Id)
    .Select(t => toppings.FirstOrDefault(topping => topping.Id == t.ToppingId)).ToList();
    
    return new TuberOrderDTO
    {
        Id = order.Id,
        OrderPlacedOnDate = (DateTime)order.DeliveredOnDate,
        CustomerId = order.CustomerId,
        Customer = new CustomerDTO {Id = customer.Id, Name = customer.Name, Address = customer.Address},
        TuberDriverId = order.TuberDriverId,
        TuberDriver = new TuberDriverDTO {Id = tuberDriver.Id, Name = tuberDriver.Name},
        DeliveredOnDate = order.DeliveredOnDate,
        Toppings =  ordertoppings.Select(ot => new ToppingDTO {Id=ot.Id, Name = ot.Name}).ToList()
    }; 
    
});

app.MapPost("/tuberorders", (TuberOrder tuberOrder) => 
{
    tuberOrder.Id = orders.Any() ? orders.Max(o => o.Id) + 1 : 1;
    tuberOrder.OrderPlacedOnDate = DateTime.Now;
    orders.Add(tuberOrder);
    return Results.Ok(new TuberOrderDTO
    {
        Id = tuberOrder.Id,
        OrderPlacedOnDate = tuberOrder.OrderPlacedOnDate,
        CustomerId = tuberOrder.CustomerId
        
    });
});

app.MapPut("/tuberorders/{id}", (int id, int driverId) => 
{   
    TuberOrder tuberOrder = orders.FirstOrDefault(o => o.Id == id);
    tuberOrder.TuberDriverId = driverId;
    return Results.Ok(tuberOrder);

});

app.MapPost("/tuberorders/{id}/complete", (int id) => 
{
    TuberOrder tuberOrder = orders.FirstOrDefault(o => o.Id == id);
    if (tuberOrder.DeliveredOnDate == null)
    {
        tuberOrder.DeliveredOnDate = DateTime.Now;
        return Results.Ok();
        
    }else
    {
        return Results.BadRequest();
    }
    

});

app.MapGet("/toppings", () => 
{
    return toppings.Select(t => new Topping 
    {
        Id = t.Id,
        Name = t.Name
    });
});

app.Run();
//don't touch or move this!
public partial class Program { }