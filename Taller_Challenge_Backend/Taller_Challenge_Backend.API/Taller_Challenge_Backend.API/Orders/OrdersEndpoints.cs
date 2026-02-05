using Asp.Versioning.Builder;
using Taller_Challenge_Backend.API.Common;
using Taller_Challenge_Backend.API.Orders.Commands;
using Taller_Challenge_Backend.API.Orders.Queries;
using Taller_Challenge_Backend.Domain.Models.Responses;

namespace Taller_Challenge_Backend.API.Orders
{
    public class OrdersEndpoints : IEndpoints
    {
        public static IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder builder, ApiVersionSet versions)
        {
            var group = builder.MapGroup("/api/v1/orders")
                .WithTags("Order")
                .WithApiVersionSet(versions)
                .MapToApiVersion(1);

            // Applied all required endpoints

            // GET /orders
            group.MapGet("/", GetAllOrdersQuery.ExecuteQuery)
                .Produces<IEnumerable<OrderResponse>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .WithName("GetOrders")
                .WithSummary("Get all orders")
                .WithDescription("Retrieve orders with optional status filter");

            // GET /orders/{id}
            group.MapGet("/{id:guid}", GetOrderByIdQuery.ExecuteQuery)
                .Produces<OrderResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .WithName("GetOrderById")
                .WithSummary("Get order by ID")
                .WithDescription("Retrieve a specific order by its unique identifier");

            // POST /orders
            group.MapPost("/", CreateOrderCommand.ExecuteCommand)
                .Produces<OrderResponse>(StatusCodes.Status201Created)
                .ProducesValidationProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .WithName("CreateOrder")
                .WithSummary("Create a new order")
                .WithDescription("Create a new service order for a customer");

            // PATCH /orders/{id}/status
            group.MapPatch("/{id:guid}/status", UpdateOrderStatusCommand.ExecuteCommand)
                .Produces<OrderResponse>(StatusCodes.Status200OK)
                .ProducesValidationProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .WithName("UpdateOrderStatus")
                .WithSummary("Update order status")
                .WithDescription("Update the status of an existing order");

            // POST /orders/{id}/price
            group.MapPost("/{id:guid}/price", CalculateOrderPriceCommand.ExecuteCommand)
                .Produces(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .WithName("CalculateOrderPrice")
                .WithSummary("Calculate order price")
                .WithDescription("Calculate final price including taxes and discounts");

            return builder;
        }
    }
}
