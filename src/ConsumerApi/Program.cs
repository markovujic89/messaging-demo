using ConsumerApi;
using ConsumerApi.Models;
using ConsumerApi.Repository;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<LeadCreatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        // configure receive endpoint and bind message type
        cfg.ReceiveEndpoint("lead-created-queue", e =>
        {
            // durable, prefetch, retry can be tuned
            e.Durable = true;
            e.PrefetchCount = 16;
            e.ConcurrentMessageLimit = 10;
            e.ConfigureConsumer<LeadCreatedConsumer>(context);
        });
    });
});

builder.Services.AddScoped<IMessageRepository, MessageRepository>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();