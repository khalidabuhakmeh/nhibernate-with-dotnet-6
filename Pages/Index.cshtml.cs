using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NHibernate.Linq;
using ISession = NHibernate.ISession;

namespace Demo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IList<StoreModel> Stores { get; set; }
            = Array.Empty<StoreModel>();

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGet([FromServices]ISession session)
        {
            Stores = await session
                .Query<Models.Entities.Store>()
                .Select(s => new StoreModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    NumberOfProducts = s.Products.Count,
                    NumberOfStaff = s.Staff.Count
                })
                .ToListAsync();
        }
    }

    public class StoreModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayName("# of Products")]
        public int NumberOfProducts { get; set; }
        [DisplayName("# of Staff")]
        public int NumberOfStaff { get; set; }
    }
}