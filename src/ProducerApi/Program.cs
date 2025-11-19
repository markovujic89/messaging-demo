using Lead.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProducerApi.BackgroundTasks;
using ProducerApi.Models;
using ProducerApi.Repository;
using ProducerApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // ← ADD THIS LINE
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ProducerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost", host =>
        {
            host.Username("guest");
            host.Password("guest");
        });
        cfg.Publish<LeadCreated>(rabbitMqMessagePublishTopologyConfigurator =>
        {
            rabbitMqMessagePublishTopologyConfigurator.Durable = true;  // ensures persistent messages
        });
    });
});

builder.Services.AddScoped<ILeadService, LeadService>();
builder.Services.AddScoped<ILeadRepository, LeadRepository>();

builder.Services.AddScoped<IOutboxProcessor, OutboxProcessor>();
builder.Services.AddHostedService<OutboxBackgroundService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers(); // This will now work

app.Run();
