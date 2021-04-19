using System.Collections.Generic;
using System.IO;
using System.Linq;
using Demo.Models.Entities;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Demo.Models
{
    public static class Database
    {
        public const string DatabaseFilename = "database.db";
        
        public static void BuildSchema(Configuration configuration)
        {
            if (File.Exists(DatabaseFilename))
                File.Delete(DatabaseFilename);
            
            var schema = new SchemaExport(configuration);

            schema.Create(false, true);
        }

        public static void Initialize(ISession session)
        {
            using var transaction = session.BeginTransaction();
            // create a couple of Stores each with some Products and Employees
            var barginBasin = new Store { Name = "Bargin Basin" };
            var superMart = new Store { Name = "SuperMart" };

            var potatoes = new Product { Name = "Potatoes", Price = 3.60 };
            var fish = new Product { Name = "Fish", Price = 4.49 };
            var milk = new Product { Name = "Milk", Price = 0.79 };
            var bread = new Product { Name = "Bread", Price = 1.29 };
            var cheese = new Product { Name = "Cheese", Price = 2.10 };
            var waffles = new Product { Name = "Waffles", Price = 2.41 };

            var daisy = new Employee { FirstName = "Daisy", LastName = "Harrison" };
            var jack = new Employee { FirstName = "Jack", LastName = "Torrance" };
            var sue = new Employee { FirstName = "Sue", LastName = "Walkters" };
            var bill = new Employee { FirstName = "Bill", LastName = "Taft" };
            var joan = new Employee { FirstName = "Joan", LastName = "Pope" };

            // add products to the stores, there's some crossover in the products in each
            // store, because the store-product relationship is many-to-many
            new List<Product> { potatoes, fish, milk, bread, cheese}
                .ForEach(barginBasin.AddProduct);
            new List<Product> { bread, cheese, waffles }
                .ForEach(superMart.AddProduct);

            // add employees to the stores, this relationship is a one-to-many, so one
            // employee can only work at one store at a time
            new List<Employee> { daisy, jack, sue }.ForEach(barginBasin.AddEmployee);
            new List<Employee> { bill, joan }.ForEach(superMart.AddEmployee);

            // save both stores, this saves everything else via cascading
            session.SaveOrUpdate(barginBasin);
            session.SaveOrUpdate(superMart);

            transaction.Commit();
        }
    }
}