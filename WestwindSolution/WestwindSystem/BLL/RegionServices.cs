using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WestwindSystem.DAL;
using WestwindSystem.Entities;

namespace WestwindSystem.BLL
{
    public class RegionServices
    {
        private readonly WestwindContext _dbContext;

        internal RegionServices(WestwindContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Region> GetAll()
        {
            var query = _dbContext.Regions.OrderBy(currentRegion => currentRegion.RegionDescription);
            return query.ToList();
        }

        public Region? GetById(int regionId)
        {
            var query = _dbContext
                .Regions;
            return query.FirstOrDefault(currentRegion => currentRegion.RegionId == regionId);

        }
        public List<Region> FindByPartialDescription(string partialDescription)
        {
            var query = _dbContext
                .Regions
                .Where(currentRegion => currentRegion.RegionDescription.Contains(partialDescription))
                .OrderBy(currentRegion => currentRegion.RegionDescription);
            return query.ToList();

        }

        public int AddRegion(Region newRegion)
        {
            // Enforce business rule that region must be unique
            bool exists = _dbContext
                .Regions
                .Any(region => region.RegionDescription == newRegion.RegionDescription);
            if(exists)
            {
                throw new Exception("The region name already exist!");
            }
            _dbContext.Regions.Add(newRegion);
            _dbContext.SaveChanges();
            return newRegion.RegionId;
        }
    }
}
