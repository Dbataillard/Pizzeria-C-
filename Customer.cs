using System;

public class Customer
{
    public string Surname { get; set; }
    public string FirstName { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime FirstOrderDate { get; set; }
    public int NumberOfOrder { get; set; }

    // Constructeur pour un nouveau client
    public Customer(string surname, string firstName, string address, string city, string phoneNumber)
    {
        Surname = surname;
        FirstName = firstName;
        Address = address;
        City = city;
        PhoneNumber = phoneNumber;
        FirstOrderDate = DateTime.Now; // La date de la première commande est définie sur la date actuelle
        NumberOfOrder = 0;
    }

    // Méthode pour afficher les détails complets du client
    public void DisplayDetails()
    {
        Console.WriteLine($"Name: {FirstName} {Surname}");
        Console.WriteLine($"Address: {Address}");
        Console.WriteLine($"Phone Number: {PhoneNumber}");
        Console.WriteLine($"First Order Date: {FirstOrderDate.ToShortDateString()}");
    }
}