using System;
using System.Collections.Generic;
using System.Linq;
public class Order
{
    public double OrderNumber { get; set; }
    public List<Item> Items { get; set; }
    public Customer Customer { get; set; }
    public Employee Clerk { get; set; }
    public Employee DeliveryPerson { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime OrderDate { get; set; }

    public void AddItem(Item item)
    {
        Items.Add(item);
    }

    public decimal CalculateTotal()
    {
        decimal total = 0;
        foreach (var item in Items)
        {
            total += item.CalculatePrice();
        }
        return total;
    }

    public void DisplayOrderDetails()
    {
        Console.WriteLine($"Order Number: {OrderNumber}");
        Console.WriteLine($"Customer: {Customer.FirstName}");
        Console.WriteLine($"Status: {Status}");
        Console.WriteLine("Items:");
        foreach (var item in Items)
        {
            Console.WriteLine($"- {item.Name}: ${item.Price}");
        }
        Console.WriteLine($"Total: ${CalculateTotal()}");
    }
    

    public void UpdateStatus(OrderStatus newStatus)
    {
        Status = newStatus;
    }

    public static double GenerateOrderNumber()
    {
        return double.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
    }

}

public enum OrderStatus
{
    Preparing,
    Delivering,
    Closed,
    Paid,
    Lost
}