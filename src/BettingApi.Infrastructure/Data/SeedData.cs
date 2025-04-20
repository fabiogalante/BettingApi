// src/BettingApi.Infrastructure/Data/SeedData.cs
using BettingApi.Core.Customers;
using BettingApi.Core.Bets;
using BettingApi.Core.Common;

namespace BettingApi.Infrastructure.Data;

public static class SeedData
{
    public static void Initialize(AppDbContext context)
    {
        if (!context.Customers.Any())
        {
            var customers = new List<Customer>
            {
                new Customer
                {
                   
                    Name = "João Silva",
                    Email = "joao.silva@email.com",
                  
                },
                new Customer
                {
                  
                    Name = "Maria Oliveira",
                    Email = "maria.oliveira@email.com",
                  
                },
                new Customer
                {
                  
                    Name = "Carlos Souza",
                    Email = "carlos.souza@email.com",
                   
                },
                new Customer
                {
                  
                    Name = "Ana Costa",
                    Email = "ana.costa@email.com",
                   
                },
                new Customer
                {
                   
                    Name = "Pedro Santos",
                    Email = "pedro.santos@email.com",
                 
                }
            };

            context.Customers.AddRange(customers);
            context.SaveChanges();

            var bets = new List<Bet>
            {
                new Bet
                {
                   
                    Description = "Aposta no jogo Flamengo x Vasco",
                    Amount = 250.50m,
                    CustomerId = customers[0].Id,
                
                },
                new Bet
                {
                   
                    Description = "Aposta no campeonato brasileiro",
                    Amount = 1000.00m,
                    CustomerId = customers[0].Id,
                 
                },
                new Bet
                {
                
                    Description = "Aposta na Copa do Mundo",
                    Amount = 5000.00m,
                    CustomerId = customers[1].Id,
                
                },
                new Bet
                {
                   
                    Description = "Aposta no UFC 300",
                    Amount = 750.25m,
                    CustomerId = customers[2].Id,
                   
                },
                new Bet
                {
                 
                    Description = "Aposta no Grand Prix de F1",
                    Amount = 1500.00m,
                    CustomerId = customers[3].Id,
                   
                },
                new Bet
                {
                 
                    Description = "Aposta no NBA Finals",
                    Amount = 3000.00m,
                    CustomerId = customers[4].Id,
                 
                },
                new Bet
                {
                   
                    Description = "Aposta no Wimbledon",
                    Amount = 1200.00m,
                    CustomerId = customers[4].Id,
                   
                }
            };

            context.Bets.AddRange(bets);
            context.SaveChanges();
        }
    }
}