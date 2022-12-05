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

        public string AddTerritory(Territory newTerritory)
        {
            // Enforce business rule that Territory Description must be unique
            bool duplicateTerritoryDescription = _dbContext
                .Territories
                .Any(currentTerritory => currentTerritory.TerritoryDescription == newTerritory.TerritoryDescription);
            if (duplicateTerritoryDescription)
            {
                throw new ArgumentException("The territory description already exist.");
            }

            // Enforce business rule that TerritoryId is required
            if (string.IsNullOrWhiteSpace(newTerritory.TerritoryId))
            {
                throw new ArgumentException($"TerritoryId value is required.");
            }

            // Enforce business rule that TerritoryId must be unique
            bool duplicateTerritoryId = _dbContext
                .Territories
                .Any(currentTerritory => currentTerritory.TerritoryId == newTerritory.TerritoryId);
            if (duplicateTerritoryId)
            {
                throw new ArgumentException($"The TerritoryId {newTerritory.TerritoryId} already exist.");
            }

            _dbContext.Territories.Add(newTerritory);
            _dbContext.SaveChanges();
            return newTerritory.TerritoryId;
        }

        public Territory? GetById(string territoryId)
        {
            var query = _dbContext
                .Territories
                .Include(currentTerritory => currentTerritory.Region)
                .Where(currentTerritory => currentTerritory.TerritoryId == territoryId);
            return query.FirstOrDefault();
        }

        public int UpdateTerritory(string editId, Territory existingTerritory)
        {
            //_dbContext.Territories.Attach(existingTerritory).State = EntityState.Modified;
            Territory? queryTerritorySingleResult = _dbContext
                .Territories
                .FirstOrDefault(currentTerritory => currentTerritory.TerritoryId == editId);
            if (queryTerritorySingleResult == null)
            {
                throw new ArgumentException($"Invalid territoryId of {editId}");
            }
            // Change only the properties that is allowed to be changed
            queryTerritorySingleResult.TerritoryDescription = existingTerritory.TerritoryDescription;
            queryTerritorySingleResult.RegionId = existingTerritory.RegionId;

            int rowsUpdated = _dbContext.SaveChanges();
            return rowsUpdated;
        }

        public int DeleteTerritory(string TerritoryID)
        {
            Territory? existingTerritory = _dbContext
                .Territories
                .Where(currentTerritory => currentTerritory.TerritoryId == TerritoryID)
                .FirstOrDefault();

            if (existingTerritory == null)
            {
                throw new ArgumentException($"TerritoryID {TerritoryID} does not exists.");
            }

            _dbContext.Territories.Remove(existingTerritory);
            int rowsDeleted = _dbContext.SaveChanges();
            return rowsDeleted;
        }

    }


}
