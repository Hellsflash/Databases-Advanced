using System;
using System.Collections.Generic;
using System.Linq;

namespace Problem_4.Shopping_Spree
{
    class StartUp
    {
        static void Main()
        {
            var people = new List<Person>();
            var products = new List<Product>();

            var personTokens = Console.ReadLine().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
            try
            {
                for (int i = 0; i < personTokens.Length; i++)
                {
                    var personInfo = personTokens[i].Split('=');

                    var personName = personInfo[0];
                    var money = decimal.Parse(personInfo[1]);

                    var person = new Person(personName, money);
                    people.Add(person);
                }

                var productsTokens = Console.ReadLine().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToArray();

                for (int i = 0; i < productsTokens.Length; i++)
                {
                    var productInfo = productsTokens[i].Split('=');

                    var productName = productInfo[0];
                    var cost = decimal.Parse(productInfo[1]);

                    var product = new Product(productName, cost);
                    products.Add(product);
                }


                string input;
                while ((input = Console.ReadLine()) != "END")
                {
                    var commandInfo = input.Split();

                    var buyer = commandInfo[0];
                    var item = commandInfo[1];
                    if (string.IsNullOrEmpty(buyer) || string.IsNullOrWhiteSpace(buyer))
                    {
                        Console.WriteLine("Name can not be empty");
                        return;
                    }
                    if (string.IsNullOrEmpty(item) || string.IsNullOrWhiteSpace(item))
                    {
                        Console.WriteLine("Name can not be empty");
                        return;
                    }

                    var currentPerson = people.Where(n => n.Name == buyer).FirstOrDefault();
                    var currentProduct = products.Where(n => n.Name == item).FirstOrDefault();

                    if (currentPerson.Money >= currentProduct.Cost)
                    {
                        currentPerson.Products.Add(currentProduct);
                        currentPerson.Money -= currentProduct.Cost;
                        Console.WriteLine($"{currentPerson.Name} bought {currentProduct.Name}");
                    }
                    else
                    {
                        Console.WriteLine($"{currentPerson.Name} can't afford {currentProduct.Name}");
                    }
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return;
            }

            foreach (var person in people)
            {
                if (person.Products.Count <= 0)
                {
                    Console.WriteLine($"{person.Name} - Nothing bought");
                }
                else
                {
                    Console.WriteLine($"{person.Name} - {string.Join(", ", person.Products.Select(n => n.Name))}");
                }
            }
        }
    }
}
