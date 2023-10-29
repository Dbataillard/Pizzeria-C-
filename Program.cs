using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

class Program
{
    static async Task Main(string[] args)
    {
        List<Customer> customers = LoadCustomers();
        List<Clerk> clerks = LoadClerks();
        List<DeliveryPerson> deliveryPersons = LoadDeliveryPersons();
        List<Order> orders = LoadOrders();

        while (true)
        {
            Console.WriteLine("\nMenu principal:");
            Console.WriteLine("1. Module Customer/Workforce");
            Console.WriteLine("2. Module Commande");
            Console.WriteLine("3. Module Statistiques");
            Console.WriteLine("4. Quitter");
            Console.Write("Veuillez choisir une option: ");

            string mainChoice = Console.ReadLine();

            switch (mainChoice)
            {
                case "1":
                    bool exitCustomerMenu = false;
                    while (!exitCustomerMenu)
                    {
                        Console.WriteLine("\nModule Customer/Workforce:");
                        Console.WriteLine("1. Ajouter, Supprimer ou Modifier un client, clerk ou livreur");
                        Console.WriteLine("2. Afficher les clients par ordre alphabétique");
                        Console.WriteLine("3. Afficher les clients par ville");
                        Console.WriteLine("4. Afficher les clients par montant d'achat cumulé");
                        Console.WriteLine("5. Retour au menu principal");
                        Console.Write("Veuillez choisir une option: ");

                        string customerChoice = Console.ReadLine();

                        switch (customerChoice)
                        {
                            case "1":
                                bool exitAddMenu =false;
                                while (!exitAddMenu)
                                {
                                    Console.WriteLine("\nAjouter, Supprimer ou Modifier un client, clerk ou livreur");
                                    Console.WriteLine("1. Ajouter un client");
                                    Console.WriteLine("2. Supprimer un client");
                                    Console.WriteLine("3. Modifier un client");
                                    Console.WriteLine("4. Ajouter un clerk");
                                    Console.WriteLine("5. Supprimer un clerk");
                                    Console.WriteLine("6. Modifier un clerk");
                                    Console.WriteLine("7. Ajouter un livreur");
                                    Console.WriteLine("8. Supprimer un livreur");
                                    Console.WriteLine("9. Modifier un livreur");
                                    Console.WriteLine("10. Retour au menu précédent");
                                    Console.Write("Que voulez-vous faire ? ");
                                    
                                    string addChoice = Console.ReadLine();

                                    switch (addChoice)
                                    {
                                        case "1":
                                            AddNewCustomer(customers);
                                            break;
                                        case "2":
                                            DeleteCustomer(customers);
                                            break;
                                        case "3":
                                            ModifyCustomer(customers);
                                            break;
                                        case "4":
                                            AddNewClerk(clerks); 
                                            break;
                                        case "5":
                                            DeleteClerk(clerks);
                                            break;
                                        case "6":
                                            ModifyClerk(clerks);
                                            break;
                                        case "7":
                                            AddNewDeliveryPerson(deliveryPersons);
                                            break;
                                        case "8":
                                            DeleteDeliveryPerson(deliveryPersons);  
                                            break;
                                        case "9":
                                            ModifierLivreur(deliveryPersons);
                                            break;
                                        case "10":
                                            Console.WriteLine("Retour au menu precedent...");
                                            exitAddMenu = true;
                                            break;
                                    }
                                }
                                break;
                            case "2":
                                DisplayCustomersAlphabetically(customers);
                                break;
                            case "3":
                                DisplayCustomersByCity(customers);
                                break;
                            case "4":
                                DisplayCustomersByPurchaseAmount(customers);
                                break;
                            case "5":
                                Console.WriteLine("Retour au menu principal...");
                                exitCustomerMenu = true;
                                break;
                            default:
                                Console.WriteLine("Choix non reconnu. Veuillez réessayer.");
                                break;
                        }
                    }
                    break;
                case "2":
                    await HandleOrdering(customers, clerks, deliveryPersons, orders);
                    break;

                case "3":
                    bool exitStatMenu = false;
                    while (!exitStatMenu)
                    {
                        Console.WriteLine("\nModule Statistiques:");
                        Console.WriteLine("1. Afficher les clerks par nombre de commandes");
                        Console.WriteLine("2. Afficher les livreurs par nombre de livraisons");
                        Console.WriteLine("3. Afficher les commandes par période de temps");
                        Console.WriteLine("4. Afficher le prix moyen des commandes");
                        Console.WriteLine("5. Retour au menu principal");
                        Console.Write("Veuillez choisir une option: ");

                        string statsChoice = Console.ReadLine();

                        switch (statsChoice)
                        {
                            case "1":
                                DisplayOrdersByClerk(clerks);
                                break;
                            case "2":                              
                                DisplayDeliveriesByDeliveryPerson(deliveryPersons);
                                break;
                            case "3":
                                DisplayOrdersByTimePeriod(orders);
                                break;
                            case "4":
                                DisplayAverageOrderPrice(orders);
                                break;
                            case "5":
                                Console.WriteLine("Retour au menu principal...");
                                exitStatMenu = true;
                                break;
                            default:
                                Console.WriteLine("Choix non reconnu. Veuillez réessayer.");
                                break;
                        }
                    }
                    break;

                case "4":
                    Console.WriteLine("Au revoir!");
                    return;

                default:
                    Console.WriteLine("Choix non reconnu. Veuillez réessayer.");
                    break;
            }
        }


        static async Task HandleOrdering(List<Customer> customers, List<Clerk> clerks, List<DeliveryPerson> deliveryPersons, List<Order> orders)
        {
            Customer currentCustomer = null;

            Random random = new Random();
            int randomIndexDelivery = random.Next(deliveryPersons.Count);
            DeliveryPerson deliveryPerson = deliveryPersons[randomIndexDelivery];

            if (deliveryPerson == null)
            {
                Console.WriteLine("Aucun livreur disponible.");
                return;
            }

            int randomIndexClerk = random.Next(clerks.Count);
            Clerk clerkPerson = clerks[randomIndexClerk];
            if (clerkPerson == null)
            {
                Console.WriteLine("Aucun clerk disponible.");
                return;
            }

            string response;
            do
            {
                Console.WriteLine("\nEst-ce votre première commande chez nous ? (oui/non)");
                response = Console.ReadLine().ToLower();

                if (response == "non")
                {
                    Console.WriteLine("Veuillez entrer votre numéro de téléphone :");
                    string phoneNumber = Console.ReadLine();

                    Customer existingCustomer = customers.Find(c => c.PhoneNumber == phoneNumber);

                    if (existingCustomer != null)
                    {
                        Console.WriteLine($"Bonjour {existingCustomer.FirstName}, nous avons confirmé votre adresse : {existingCustomer.Address}");
                        currentCustomer = existingCustomer;
                    }
                    else
                    {
                        Console.WriteLine("Désolé, nous n'avons pas trouvé vos coordonnées. Veuillez les saisir.");
                        currentCustomer = AddNewCustomer(customers);
                    }
                }
                else if (response == "oui")
                {
                    AddNewCustomer(customers);
                }
                else
                {
                    Console.WriteLine("Réponse non reconnue. Veuillez répondre par 'oui' ou 'non'.");
                }
            } while (response != "oui" && response != "non");

            Order currentOrder = new Order
            {
                OrderNumber = Order.GenerateOrderNumber(),
                Items = new List<Item>(),
                Customer = currentCustomer,
                Clerk = clerkPerson,
                DeliveryPerson = deliveryPerson,
                Status = OrderStatus.Preparing,
                OrderDate = DateTime.Now
            };

            while (true)
            {
                Console.WriteLine("Que souhaitez-vous commander ? (pizza/boisson/rien)");
                string choix = Console.ReadLine().ToLower();

                if (choix == "rien")
                    break;

                if (choix == "pizza")
                {
                    string size;
                    do
                    {
                        Console.WriteLine("Choisissez la taille de la pizza (petite, moyenne, grande) :");
                        size = Console.ReadLine();
                        if (size != "petite" && size != "moyenne" && size != "grande")
                        {
                            Console.WriteLine("Taille non reconnue. Veuillez choisir entre 'petite', 'moyenne' ou 'grande'.");
                        }
                    } while (size != "petite" && size != "moyenne" && size != "grande");

                    string type;
                    do
                    {
                        Console.WriteLine("Choisissez le type de la pizza (margarita, végétarienne, reine) :");
                        type = Console.ReadLine();
                        if (type != "margarita" && type != "végétarienne" && type != "reine")
                        {
                            Console.WriteLine("Type non reconnu. Veuillez choisir entre 'margarita', 'végétarienne' ou 'reine'.");
                        }
                    } while (type != "margarita" && type != "végétarienne" && type != "reine");

                    Pizza pizza = new Pizza(choix, size, type, 0) { Name = choix, Size = size, Type = type, Price = 0 };
                    pizza.Price = pizza.CalculatePrice();
                    currentOrder.AddItem(pizza);
                }
                else if (choix == "boisson")
                {
                    string drinkType;
                    do
                    {
                        Console.WriteLine("Choisissez la boisson (cola, jus d'orange) :");
                        drinkType = Console.ReadLine();
                        if (drinkType != "cola" && drinkType != "jus d'orange")
                        {
                            Console.WriteLine("Boisson non reconnue. Veuillez choisir entre 'cola' ou 'jus d'orange'.");
                        }
                    } while (drinkType != "cola" && drinkType != "jus d'orange");

                    Console.WriteLine("Combien en voulez-vous ?");
                    int volume = int.Parse(Console.ReadLine());

                    Drink drink = new Drink(choix, volume, drinkType, 0) { Name = choix, Type = drinkType, Volume = volume, Price = 0 };
                    drink.Price = drink.CalculatePrice();
                    currentOrder.AddItem(drink);
                }
                else
                {
                    Console.WriteLine("Choix non reconnu. Veuillez choisir entre 'pizza', 'boisson' ou 'rien'.");
                }
            }

            orders.Add(currentOrder);
            SaveOrders(orders);
            currentOrder.Status = OrderStatus.Preparing;
            string statusName = currentOrder.Status.ToString();
            Console.WriteLine($"Le statut actuel de votre commande est : {statusName}");
            decimal total = currentOrder.CalculateTotal();
            Console.WriteLine($"Le prix total de votre commande est : {total}€");
            currentCustomer.NumberOfOrder++;
            SaveCustomers(customers);
            await ConfirmOrderAsync(currentOrder);
            SaveOrders(orders);
            clerkPerson.OrdersTaken++;
            SaveClerks(clerks);
            currentOrder.Status = OrderStatus.Delivering;
            statusName = currentOrder.Status.ToString();
            Console.WriteLine($"Le statut actuel de votre commande est : {statusName}");
            SaveOrders(orders);
            deliveryPerson.TakeOrderForDelivery(currentOrder);
            await Task.Delay(TimeSpan.FromMinutes(1));
            deliveryPerson.DeliverOrderToCustomer(currentOrder);
            currentOrder.Status = OrderStatus.Closed;
            statusName = currentOrder.Status.ToString();
            Console.WriteLine($"Le statut actuel de votre commande est : {statusName}");
            SaveOrders(orders);
            deliveryPerson.ReturnToPizzeria(currentOrder);
            deliveryPerson.DeliveriesMade++;
            SaveDeliveryPersons(deliveryPersons);
        }

        static List<Order> LoadOrders()
        {
            if (File.Exists("orders.json"))
            {
                string json = File.ReadAllText("orders.json");
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    Converters = new List<JsonConverter> { new ItemConverter() }
                };
                return JsonConvert.DeserializeObject<List<Order>>(json, settings);
            }
            return new List<Order>();
        }

        static void SaveOrders(List<Order> orders)
        {
            string json = JsonConvert.SerializeObject(orders, Formatting.Indented);
            File.WriteAllText("orders.json", json);
        }

        static void DisplayOrdersByTimePeriod(List<Order> orders)
        {
            Console.WriteLine("Entrez la date de début (format: dd/MM/yyyy):");
            DateTime startDate = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Entrez la date de fin (format: dd/MM/yyyy):");
            DateTime endDate = DateTime.Parse(Console.ReadLine());

            var filteredOrders = orders.Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate).ToList();

            Console.WriteLine($"Commandes du {startDate.ToShortDateString()} au {endDate.ToShortDateString()}:");
            foreach (var order in filteredOrders)
            {
                Console.WriteLine($"Commande #{order.OrderNumber} - Date: {order.OrderDate}");
            }
        }

        static void DisplayAverageOrderPrice(List<Order> orders)
        {
            decimal totalAmount = 0;
            foreach (var order in orders)
            {
                totalAmount += order.CalculateTotal();
            }

            decimal averagePrice = orders.Count > 0 ? totalAmount / orders.Count : 0;
            Console.WriteLine($"Prix moyen des commandes: {averagePrice}€");
        }

        static Clerk AddNewClerk(List<Clerk> clerks)
        {
            Console.WriteLine("Veuillez entrer votre nom :");
            string name = Console.ReadLine();

            Clerk newClerk = new Clerk(name, 0);

            clerks.Add(newClerk);
            SaveClerks(clerks);
            Console.WriteLine("Nouveau clerk ajouté avec succès !");

            return newClerk;
        }

        static void DeleteClerk(List<Clerk> clerks)
        {
            Console.WriteLine("Entrer le nom du clerk a supprimer");
            string name = Console.ReadLine();
            Clerk clerkToDelete = clerks.Find(c => c.Name == name);
            if (clerkToDelete != null)
            {
                clerks.Remove(clerkToDelete);
                SaveClerks(clerks);
                Console.WriteLine("Clerk supprimer avec succes.");
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
        }

        static void ModifyClerk(List<Clerk> clerks)
        {
            Console.WriteLine("Entrer le nom du clerk a modifier:");
            string name = Console.ReadLine();
            Clerk clerkToModify = clerks.Find(c => c.Name == name);
            if (clerkToModify != null)
            {
                while (true)
                {
                    Console.WriteLine("Which detail would you like to modify?");
                    Console.WriteLine("1. Nom");
                    Console.WriteLine("2. Nombre de commande ");
                    Console.WriteLine("3. Finish modifications");
                    Console.Write("Choose an option: ");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            Console.WriteLine("Enter the new  name:");
                            clerkToModify.Name = Console.ReadLine();
                            break;
                        case "2":
                            Console.WriteLine("Enter the new surname:");
                            clerkToModify.OrdersTaken = Console.Read();
                            break;
                        case "3":
                            SaveClerks(clerks);
                            Console.WriteLine("Customer modified successfully.");
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
        }

        static List<Clerk> LoadClerks()
        {
            if (File.Exists("clerks.json"))
            {
                string json = File.ReadAllText("clerks.json");
                return JsonConvert.DeserializeObject<List<Clerk>>(json);
            }
            return new List<Clerk>();
        }

        static void SaveClerks(List<Clerk> clerks)
        {
            string json = JsonConvert.SerializeObject(clerks, Formatting.Indented);
            File.WriteAllText("clerks.json", json);
        }

        static void DisplayOrdersByClerk(List<Clerk> clerks)
        {
            Console.WriteLine("Nombre de commandes gérées par commis:");
            foreach (var clerk in clerks)
            {
                Console.WriteLine($"{clerk.Name}: {clerk.OrdersTaken} commandes");
            }
        }

        static DeliveryPerson AddNewDeliveryPerson(List<DeliveryPerson> deliveryPersons)
        {
            Console.WriteLine("Veuillez entrer le nom du livreur :");
            string name = Console.ReadLine();

            DeliveryPerson newDeliveryPerson = new DeliveryPerson(name);
            deliveryPersons.Add(newDeliveryPerson);
            SaveDeliveryPersons(deliveryPersons);
            Console.WriteLine("Nouveau livreur ajouté avec succès !");
            return newDeliveryPerson;
        }

        static void DeleteDeliveryPerson(List<DeliveryPerson> deliveryPersons)
        {
            Console.WriteLine("Entrez le nom du livreur à supprimer:");
            string name = Console.ReadLine();
            DeliveryPerson deliveryPersonToDelete = deliveryPersons.Find(d => d.Name == name);
            if (deliveryPersonToDelete != null)
            {
                deliveryPersons.Remove(deliveryPersonToDelete);
                SaveDeliveryPersons(deliveryPersons);
                Console.WriteLine("Livreur supprimé avec succès.");
            }
            else
            {
                Console.WriteLine("Livreur non trouvé.");
            }
        }

        static void ModifierLivreur(List<DeliveryPerson> livreurs)
        {
            Console.WriteLine("Entrez le nom du livreur à modifier:");
            string nom = Console.ReadLine();
            DeliveryPerson livreurAModifier = livreurs.Find(l => l.Name == nom);
            if (livreurAModifier != null)
            {
                while (true)
                {
                    Console.WriteLine("Quel détail souhaitez-vous modifier ?");
                    Console.WriteLine("1. Nom");
                    Console.WriteLine("2. Nombre de livraisons effectuées");
                    Console.WriteLine("3. Terminer les modifications");
                    Console.Write("Choisissez une option: ");

                    string choix = Console.ReadLine();

                    switch (choix)
                    {
                        case "1":
                            Console.WriteLine("Entrez le nouveau nom:");
                            livreurAModifier.Name = Console.ReadLine();
                            break;
                        case "2":
                            Console.WriteLine("Entrez le nouveau nombre de livraisons effectuées:");
                            int nombreLivraisons;
                            if (int.TryParse(Console.ReadLine(), out nombreLivraisons))
                            {
                                livreurAModifier.DeliveriesMade = nombreLivraisons;
                            }
                            else
                            {
                                Console.WriteLine("Veuillez entrer un nombre valide.");
                            }
                            break;
                        case "3":
                            SaveDeliveryPersons(livreurs);
                            Console.WriteLine("Livreur modifié avec succès.");
                            return;
                        default:
                            Console.WriteLine("Choix non reconnu. Veuillez réessayer.");
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Livreur non trouvé.");
            }
        }

        static List<DeliveryPerson> LoadDeliveryPersons()
        {
            if (File.Exists("deliverypersons.json"))
            {
                string json = File.ReadAllText("deliverypersons.json");
                return JsonConvert.DeserializeObject<List<DeliveryPerson>>(json);
            }
            return new List<DeliveryPerson>();
        }

        static void SaveDeliveryPersons(List<DeliveryPerson> deliveryPersons)
        {
            string json = JsonConvert.SerializeObject(deliveryPersons, Formatting.Indented);
            File.WriteAllText("deliverypersons.json", json);
        }

        static void DisplayDeliveriesByDeliveryPerson(List<DeliveryPerson> deliveryPersons)
        {
            Console.WriteLine("Nombre de livraisons effectuées par livreur:");
            foreach (var deliveryPerson in deliveryPersons)
            {
                Console.WriteLine($"{deliveryPerson.Name}: {deliveryPerson.DeliveriesMade} livraisons");
            }
        }

        static void DisplayCustomersAlphabetically(List<Customer> customers)
        {
            var sortedCustomers = customers.OrderBy(c => c.Surname).ThenBy(c => c.FirstName);
            foreach (var customer in sortedCustomers)
            {
                Console.WriteLine($"{customer.FirstName} {customer.Surname}");
            }
        }

        static void DisplayCustomersByCity(List<Customer> customers)
        {
            var groupedCustomers = customers.GroupBy(c => c.City);
            foreach (var group in groupedCustomers)
            {
                Console.WriteLine($"City: {group.Key}");
                foreach (var customer in group)
                {
                    Console.WriteLine($"{customer.FirstName} {customer.Surname}");
                }
            }
        }

        static void DisplayCustomersByPurchaseAmount(List<Customer> customers)
        {
            var sortedCustomers = customers.OrderByDescending(c => c.NumberOfOrder);
            foreach (var customer in sortedCustomers)
            {
                Console.WriteLine($"{customer.FirstName} {customer.Surname} - {customer.NumberOfOrder}");
            }
        }

        static Customer AddNewCustomer(List<Customer> customers)
        {
            Console.WriteLine("\nVeuillez entrer votre nom :");
            string surname = Console.ReadLine();

            Console.WriteLine("Veuillez entrer votre prénom :");
            string firstName = Console.ReadLine();

            Console.WriteLine("Veuillez entrer votre adresse :");
            string address = Console.ReadLine();

            Console.WriteLine("Veuillez entrer votre ville :");
            string city = Console.ReadLine();

            Console.WriteLine("Veuillez entrer votre numéro de téléphone :");
            string phoneNumber = Console.ReadLine();

            Customer newCustomer = new Customer(surname, firstName, address, city, phoneNumber);

            customers.Add(newCustomer);
            SaveCustomers(customers);
            Console.WriteLine("Nouveau client ajouté avec succès !");

            return newCustomer;
        }

        static void DeleteCustomer(List<Customer> customers)
        {
            Console.WriteLine("Enter the phone number of the customer to delete:");
            string phoneNumber = Console.ReadLine();
            Customer customerToDelete = customers.Find(c => c.PhoneNumber == phoneNumber);
            if (customerToDelete != null)
            {
                customers.Remove(customerToDelete);
                SaveCustomers(customers);
                Console.WriteLine("Customer deleted successfully.");
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
        }

        static void ModifyCustomer(List<Customer> customers)
        {
            Console.WriteLine("Enter the phone number of the customer to modify:");
            string phoneNumber = Console.ReadLine();
            Customer customerToModify = customers.Find(c => c.PhoneNumber == phoneNumber);
            if (customerToModify != null)
            {
                while (true)
                {
                    Console.WriteLine("Which detail would you like to modify?");
                    Console.WriteLine("1. First Name");
                    Console.WriteLine("2. Surname");
                    Console.WriteLine("3. Address");
                    Console.WriteLine("4. City");
                    Console.WriteLine("5. Phone Number");
                    Console.WriteLine("6. Finish modifications");
                    Console.Write("Choose an option: ");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            Console.WriteLine("Enter the new first name:");
                            customerToModify.FirstName = Console.ReadLine();
                            break;
                        case "2":
                            Console.WriteLine("Enter the new surname:");
                            customerToModify.Surname = Console.ReadLine();
                            break;
                        case "3":
                            Console.WriteLine("Enter the new address:");
                            customerToModify.Address = Console.ReadLine();
                            break;
                        case "4":
                            Console.WriteLine("Entrer la nouvelle ville:");
                            customerToModify.City = Console.ReadLine();
                            break;
                        case "5":
                            Console.WriteLine("Enter the new phone number:");
                            string newPhoneNumber = Console.ReadLine();
                            if (customers.Any(c => c.PhoneNumber == newPhoneNumber))
                            {
                                Console.WriteLine("This phone number already exists. Please enter a different one.");
                            }
                            else
                            {
                                customerToModify.PhoneNumber = newPhoneNumber;
                            }
                            break;
                        case "6":
                            SaveCustomers(customers);
                            Console.WriteLine("Customer modified successfully.");
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
        }

        static List<Customer> LoadCustomers()
        {
            if (File.Exists("customers.json"))
            {
                string json = File.ReadAllText("customers.json");
                return JsonConvert.DeserializeObject<List<Customer>>(json);
            }
            return new List<Customer>();
        }

        static void SaveCustomers(List<Customer> customers)
        {
            string json = JsonConvert.SerializeObject(customers, Formatting.Indented);
            File.WriteAllText("customers.json", json);
        }

        static async Task ConfirmOrderAsync(Order order)
        {
            Kitchen kitchenStaff = new Kitchen("Luigi", 0);
            Console.WriteLine($"\nCher {order.Customer.FirstName}, votre commande a bien été prise en compte!");

            Console.WriteLine("\nCuisine : Veuillez préparer les pizzas suivantes :");
            foreach (var item in order.Items)
            {
                if (item is Pizza pizza) 
                {
                    Console.WriteLine($"- Pizza {pizza.Type} de taille {pizza.Size}");
                    await kitchenStaff.PrepareOrder(order);
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(1)); 
            Console.WriteLine("\nLivreur : Voici les détails de la commande :");
            Console.WriteLine($"Client : {order.Customer.FirstName} {order.Customer.Surname}");
            Console.WriteLine($"Adresse : {order.Customer.Address}");
            Console.WriteLine($"Téléphone : {order.Customer.PhoneNumber}");
            foreach (var item in order.Items)
            {
                if (item is Pizza pizza)
                {
                    Console.WriteLine($"- Pizza {pizza.Type} de taille {pizza.Size}");
                }
                else if (item is Drink drink)
                {
                    Console.WriteLine($"- {drink.Type}");
                }
            }

            Console.WriteLine("\nPréposé : La commande a bien été enregistrée et les notifications ont été envoyées.");
        }
    }
}
