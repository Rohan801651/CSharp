using System;
using System.Collections.Generic;

// Single Responsibility Principle (SRP)
// Base class for an Order, only holds order-related details
public class Order
{
    public int OrderId { get; set; } // Unique Order ID
    public string CustomerName { get; set; } // Name of the customer
    public List<string> Items { get; set; } // List of ordered items
    public decimal TotalAmount { get; set; } // Total order cost

    // Constructor to initialize order
    public Order(int orderId, string customerName, List<string> items, decimal totalAmount)
    {
        OrderId = orderId;
        CustomerName = customerName;
        Items = items;
        TotalAmount = totalAmount;
    }
}

// Open/Closed Principle (OCP)
// Payment processing is extensible without modifying existing code
public interface IPaymentProcessor
{
    void ProcessPayment(decimal amount); // Generic method for processing payments
}

// Implementation of credit card payment processing
public class CreditCardPayment : IPaymentProcessor
{
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing credit card payment of {amount}");
    }
}

// Implementation of PayPal payment processing
public class PayPalPayment : IPaymentProcessor
{
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing PayPal payment of {amount}");
    }
}

// Liskov Substitution Principle (LSP)
// OrderProcessor does not depend on specific payment methods
public class OrderProcessor
{
    private readonly IPaymentProcessor _paymentProcessor; // Using abstraction

    // Constructor injection ensures loose coupling
    public OrderProcessor(IPaymentProcessor paymentProcessor)
    {
        _paymentProcessor = paymentProcessor;
    }

    // Process the order and handle the payment
    public void ProcessOrder(Order order)
    {
        Console.WriteLine($"Processing Order #{order.OrderId} for {order.CustomerName}");
        Console.WriteLine($"Total Amount: {order.TotalAmount}");

        // Using payment processor (CreditCard or PayPal)
        _paymentProcessor.ProcessPayment(order.TotalAmount);

        Console.WriteLine("Order processed successfully!\n");
    }
}

// Interface Segregation Principle (ISP)
// Shipping services are separated into their own interface
public interface IShippingService
{
    void ShipOrder(Order order);
}

// Implementation of standard shipping service
public class StandardShipping : IShippingService
{
    public void ShipOrder(Order order)
    {
        Console.WriteLine($"Shipping Order #{order.OrderId} via Standard Shipping");
    }
}

// Implementation of express shipping service
public class ExpressShipping : IShippingService
{
    public void ShipOrder(Order order)
    {
        Console.WriteLine($"Shipping Order #{order.OrderId} via Express Shipping");
    }
}

// Dependency Inversion Principle (DIP)
// OrderService depends on abstractions instead of concrete classes
public class OrderService
{
    private readonly IShippingService _shippingService; // Shipping service abstraction

    public OrderService(IShippingService shippingService)
    {
        _shippingService = shippingService;
    }

    public void Ship(Order order)
    {
        _shippingService.ShipOrder(order);
    }
}

// Program Execution (Main Method)
class Program
{
    static void Main(string[] args)
    {
        // Create an Order
        List<string> items = new List<string> { "Laptop", "Mouse", "Keyboard" };
        Order order1 = new Order(101, "Ali", items, 1200);

        // Select a payment method (Credit Card)
        IPaymentProcessor paymentProcessor = new CreditCardPayment();

        // Process the order
        OrderProcessor orderProcessor = new OrderProcessor(paymentProcessor);
        orderProcessor.ProcessOrder(order1);

        // Select a shipping method (Express Shipping)
        IShippingService shippingService = new ExpressShipping();

        // Ship the order
        OrderService orderService = new OrderService(shippingService);
        orderService.Ship(order1);
    }
}