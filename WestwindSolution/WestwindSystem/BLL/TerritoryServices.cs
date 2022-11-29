using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WestwindSystem.DAL;
using WestwindSystem.Entities;

namespace WestwindSystem.BLL
{
    public class TerritoryServices
    {
        private readonly WestwindContext _dbContext;

        internal TerritoryServices(WestwindContext dbContext)
        {
            _dbContext = dbContext;
        }   

        public List<Territory> List(
            int pageNumber,
            int pageSize,
            out int totalCount
            )
        {
            var query = _dbContext
                .Territories
                .Include(currentTerritory => currentTerritory.Region)
                .OrderBy(currentTerritory => currentTerritory.TerritoryDescription);
            totalCount = query.Count();
            int skipRows = (pageNumber - 1) * pageSize;
            //return query.ToList();
            return query
                .Skip(skipRows)
                .Take(pageSize)
                .ToList();
        }

        public List<Territory> FindByRegionId(int regionId,
            int pageNumber,
            int pageSize,
            out int totalCount)
        {
            var query = _dbContext
                .Territories
                .Where(currentTerritory => currentTerritory.RegionId == regionId)
                .OrderBy(currentTerritory => currentTerritory.TerritoryDescription);
            totalCount = query.Count();
            int skipRows = (pageNumber - 1) * pageSize;
            //return query.ToList();
            return query
                 .Skip(skipRows)
                 .Take(pageSize)
                 .ToList();
        }

        public List<Territory> FindByPartialName(string partialName, 
            int pageNumber,
            int pageSize,
            out int totalCount)
        {
            var query = _dbContext
                .Territories
                .Where(currentTerritory => currentTerritory.TerritoryDescription.Contains(partialName))
                .OrderBy(currentTerritory => currentTerritory.TerritoryDescription);
            totalCount = query.Count();
            int skipRows = (pageNumber - 1) * pageSize;
            //return query.ToList();
            return query
                 .Skip(skipRows)
                 .Take(pageSize)
                 .ToList();
        }

        //public List<Territory> FindByRegionId(int regionId)
        //{
        //    var query = _dbContext
        //        .Territories
        //        .Where(currentTerritory => currentTerritory.RegionId == regionId)
        //        .OrderBy(currentTerritory => currentTerritory.TerritoryDescription);
        //    return query.ToList();
        //}

        //public List<Territory> FindByPartialName(string partialName)
        //{
        //    var query = _dbContext
        //        .Territories
        //        .Where(currentTerritory => currentTerritory.TerritoryDescription.Contains(partialName))
        //        .OrderBy(currentTerritory => currentTerritory.TerritoryDescription);
        //    return query.ToList();
        //}

    }
}
