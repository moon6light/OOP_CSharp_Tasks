
using System;
using System.Collections.Generic;
using System.Linq;

interface IPaymentMethod
{
    void Pay(double amount);
}

class CardPayment : IPaymentMethod
{
    public void Pay(double amount)
    {
        Console.WriteLine($"Оплата картою {amount:F2} грн");
    }
}

class CashPayment : IPaymentMethod
{
    public void Pay(double amount)
    {
        Console.WriteLine($"Оплата готівкою {amount:F2} грн");
    }
}

class Order
{
    public int Number { get; set; }
    public string Description { get; set; }
    public double Weight { get; set; }
    public double Distance { get; set; }
    public bool IsDelivered { get; set; }

    public Order(int number, string description, double weight, double distance)
    {
        Number = number;
        Description = description;
        Weight = weight;
        Distance = distance;
        IsDelivered = false;
    }
}

abstract class Transport
{
    public string Name { get; set; }
    public bool IsBusy { get; set; }
    public Order? CurrentOrder { get; set; }

    public Transport(string name)
    {
        Name = name;
        IsBusy = false;
        CurrentOrder = null;
    }

    public abstract double CalculateCost(Order order);

    public virtual bool CanTake(Order order)
    {
        return order.Weight > 0 && order.Distance > 0;
    }
}

class Bike : Transport
{
    public Bike() : base("Велосипед") { }

    public override double CalculateCost(Order order)
    {
        return 20 + 5 * order.Distance;
    }

    public override bool CanTake(Order order)
    {
        return base.CanTake(order) && order.Distance <= 10;
    }
}

class Car : Transport
{
    public Car() : base("Авто") { }

    public override double CalculateCost(Order order)
    {
        return 50 + 8 * order.Distance + order.Weight;
    }
}

class Truck : Transport
{
    public Truck() : base("Вантажівка") { }

    public override double CalculateCost(Order order)
    {
        return 200 + 15 * order.Distance + 0.5 * order.Weight;
    }
}

class DeliveryService
{
    private List<Transport> transports = new List<Transport>();
    private List<Order> activeOrders = new List<Order>();
    private List<Order> history = new List<Order>();

    public void AddTransport(Transport transport)
    {
        transports.Add(transport);
        Console.WriteLine($"Додано транспорт {transport.Name}");
    }

    public void AddOrder(Order order)
    {
        activeOrders.Add(order);
        Console.WriteLine($"Замовлення №{order.Number}: {order.Description}");
    }

    public void ShowPrices(Order order)
    {
        Console.WriteLine($"\nСума доставки №{order.Number}:");

        foreach (Transport transport in transports)
        {
            if (transport.CanTake(order))
            {
                Console.WriteLine(
                    $"{transport.Name}: {transport.CalculateCost(order):F2} грн");
            }
            else
            {
                Console.WriteLine(
                    $"{transport.Name}: Не підходить до цього замовлення");
            }
        }
    }

    public void AssignOrder(int orderNumber, Transport transport)
    {
        Order? order = activeOrders.FirstOrDefault(
            o => o.Number == orderNumber && !o.IsDelivered);

        if (order == null)
        {
            Console.WriteLine($"Заказ №{orderNumber} не знайдено");
            return;
        }

        if (transport.IsBusy)
        {
            Console.WriteLine($"{transport.Name} вже зянято");
            return;
        }

        if (!transport.CanTake(order))
        {
            Console.WriteLine($"{transport.Name} не можливо");
            return;
        }

        transport.IsBusy = true;
        transport.CurrentOrder = order;

        Console.WriteLine(
            $"Заказ №{order.Number} траспорт - {transport.Name}");
    }

    public void DeliverOrder(int orderNumber, IPaymentMethod payment)
    {
        Order? order = activeOrders.FirstOrDefault(
            o => o.Number == orderNumber);

        if (order == null)
        {
            Console.WriteLine($"Заказ №{orderNumber} не знайдено");
            return;
        }

        Transport? transport = transports.FirstOrDefault(
            t => t.CurrentOrder == order);

        if (transport == null)
        {
            Console.WriteLine("замовлення не зе не доданно");
            return;
        }

        double cost = transport.CalculateCost(order);

        Console.WriteLine($"\nДоставка замовлення№{order.Number}");
        Console.WriteLine($"Транспорт,{transport.Name}");

        payment.Pay(cost);

        order.IsDelivered = true;

        transport.IsBusy = false;
        transport.CurrentOrder = null;

        activeOrders.Remove(order);
        history.Add(order);

        Console.WriteLine("Замовлення доставлено");
        Console.WriteLine("Транспорт вільний");
    }

    public void ShowActiveOrders()
    {
        Console.WriteLine("\n Активні замовлення");

        if (activeOrders.Count == 0)
        {
            Console.WriteLine("Активованих  замовлень немає");
            return;
        }

        foreach (Order order in activeOrders)
        {
            Transport? transport = transports.FirstOrDefault(
                t => t.CurrentOrder == order);

            Console.WriteLine(
                $"№{order.Number} | {order.Description} | " +
                $"{order.Weight} кг | {order.Distance} км | " +
                $"Транспорт: {transport?.Name ?? ""}");
        }
    }

    public void ShowHistory()
    {
        Console.WriteLine("\nДоставки");

        if (history.Count == 0)
        {
            Console.WriteLine("Доставки пусті");
            return;
        }

        foreach (Order order in history)
        {
            Console.WriteLine(
                $"№{order.Number} | {order.Description} | " +
                $"{order.Weight} кг | {order.Distance} км | доставленно");
        }
    }
}

class Program
{
    static void Main()
    {
        DeliveryService service = new DeliveryService();

        Console.WriteLine(" служба доставки\n");

        Bike bike = new Bike();
        Car car = new Car();
        Truck truck = new Truck();

        service.AddTransport(bike);
        service.AddTransport(car);
        service.AddTransport(truck);

        Order order1 = new Order(1, "Документи", 2, 5);
        Order order2 = new Order(2, "Меблі", 100, 15);
        Order order3 = new Order(3, "Продукти", 10, 8);

        Console.WriteLine();

        service.AddOrder(order1);
        service.AddOrder(order2);
        service.AddOrder(order3);

        service.ShowPrices(order1);
        service.ShowPrices(order2);
        service.ShowPrices(order3);

        Console.WriteLine("\nназначення замовлень");

        service.AssignOrder(1, bike);
        service.AssignOrder(2, car);

        service.AssignOrder(3, bike);

        service.ShowActiveOrders();

        Console.WriteLine("\n Доставка");

        service.DeliverOrder(1, new CardPayment());

        service.DeliverOrder(2, new CashPayment());

        service.DeliverOrder(3, new CashPayment());

        Console.WriteLine("\n");

        service.AssignOrder(3, bike);
        service.DeliverOrder(3, new CashPayment());

        service.ShowActiveOrders();
        service.ShowHistory();
    }
}

