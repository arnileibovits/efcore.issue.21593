using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EFTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Setup...");
            using (var context = new MyDbContext(useLazyLoading: false))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var parent = new Parent
                {
                    Child = new ConcreteChild
                    {
                        GrandChildren = new List<GrandChild>
                        {
                            new GrandChild()
                        }
                    }
                };
                context.Parents.Add(parent);
                context.SaveChanges();
            }

            Console.WriteLine($"Eagerly loading GrandChild entities fails (lazy loading ENABLED, no tracking)");

            try
            {
                using (var context = new MyDbContext(useLazyLoading: true))
                {
                    var results = context.Parents
                        .AsNoTracking()
                        .Include(x => x.Child.GrandChildren)
                        .ToList();

                    var child = results.Single().Child;

                    // Exception is thrown when accessing child.GrandChildren
                    Console.WriteLine($"GrandChildren.Count: {child.GrandChildren.Count}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }


            Console.WriteLine();
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine();


            Console.WriteLine($"Eagerly loading too many GrandChild entities (lazy loading DISABLED, no tracking)");

            using (var context = new MyDbContext(useLazyLoading: false))
            {
                var results = context.Parents
                    .AsNoTracking()
                    .Include(x => x.Child.GrandChildren)
                    .Select(parent => new
                    {
                        Parent = parent,
                        Child = parent.Child
                    })
                    .ToList();

                var child = results.Single().Child;
                Console.WriteLine($"GrandChildren.Count expected: 1");
                Console.WriteLine($"GrandChildren.Count actual: {child.GrandChildren.Count}");
            }
        }
    }
}
