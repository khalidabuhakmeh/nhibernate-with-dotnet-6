using Demo.Models.Entities;
using FluentNHibernate.Mapping;

namespace Demo.Models.Mappings
{
    public class LocationMap : ComponentMap<Location>
    {
        public LocationMap()
        {
            Map(x => x.Aisle);
            Map(x => x.Shelf);
        }
    }
}