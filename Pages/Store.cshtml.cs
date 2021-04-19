using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Models.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NHibernate;
using NHibernate.Linq;

namespace Demo.Pages
{
    public class Store : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        
        public string Name { get; set; }
        public List<Product> Products { get; set; }
        public List<Employee> Staff { get; set; }
        
        public async Task OnGet([FromServices] ISession session)
        {
            var store = 
                await session
                .Query<Models.Entities.Store>()
                .FetchLazyProperties()
                .Where(s => s.Id == Id)
                .FirstAsync();

            Id = store.Id;
            Name = store.Name;
            Products = store.Products.ToList();
            Staff = store.Staff.OrderBy(x => x.LastName).ToList();
        }
    }
}