using System;
public class Employee
{
    public string Name { get; set; }
    public Employee(string name)
    {
        Name = name;
    }

}

public class DeliveryPerson : Employee
{
    public int DeliveriesMade { get; set; }
    public decimal MoneyCollected { get; set; }

    public DeliveryPerson(string name) : base(name)
    {
    }

    public void TakeOrderForDelivery(Order order)
    {
        Console.WriteLine($"{Name} prend la commande #{order.OrderNumber} pour la livraison.");       
    }

    public void DeliverOrderToCustomer(Order order)
    {
        Console.WriteLine($"{Name} livre la commande à l'adresse {order.Customer.Address}.");
        Console.WriteLine($"Total à payer : {order.CalculateTotal()}€");

        // Simulons la réception du paiement du client
        MoneyCollected = order.CalculateTotal();
        Console.WriteLine($"Le client a payé {MoneyCollected}€.");

        // Envoyer un message de confirmation à la pizzeria
        Console.WriteLine($"Message à la pizzeria : La commande #{order.OrderNumber} a été livrée et payée.");
    }

    public void ReturnToPizzeria(Order order)
    {
        Console.WriteLine($"{Name} retourne à la pizzeria avec le double de la facture et {MoneyCollected}€.");
        MoneyCollected = 0;
        order = null;
    }

}

public class Kitchen : Employee
{
    public int PizzasMade { get; set; }

    public Kitchen(string name, int pizzasMade) : base(name)
    {
        PizzasMade = pizzasMade;
    }

    public async Task PrepareOrder(Order order)
    {
        Console.WriteLine($"\n{Name} commence la préparation de la commande #{order.OrderNumber}.");

        foreach (var item in order.Items)
        {
            if (item is Pizza pizza)
            {
                Console.WriteLine($"Préparation de la pizza {pizza.Type} de taille {pizza.Size}.");
                PizzasMade += 1;
            }
        }

        
        await Task.Delay(TimeSpan.FromMinutes(1));

        Console.WriteLine($"La commande #{order.OrderNumber} est prête pour la livraison!");
    }
}

public class Clerk : Employee
{
    public int OrdersTaken { get; set; }

    public Clerk(string name, int ordersTaken) : base(name)
    {
        OrdersTaken = ordersTaken;
    }
}