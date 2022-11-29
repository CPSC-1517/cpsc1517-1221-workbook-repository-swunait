using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using WestwindSystem.Entities;
using WestwindSystem.BLL;
using WestwindWebApp.Helpers;

namespace WestwindWebApp.Pages.Territories
{
    public class QueryModel : PageModel
    {
        private readonly RegionServices _regionServices;
        private readonly TerritoryServices _territoryServices;

        public QueryModel(RegionServices regionServices, TerritoryServices territoryServices)
        {
            _regionServices = regionServices;
            _territoryServices = territoryServices;

            Regions = _regionServices.GetAll();
        }

        public List<Region> Regions { get; private set; }

        [BindProperty]
        public int? SelectedRegionId { get; set; }

        public List<Territory> QueryResultList { get; private set; }
        
        public string? FeedbackMessage { get; set; }

        [BindProperty]
        public string TerritoryQuery { get; set; }

        #region Paginator
        //my desired page size
        private const int PAGE_SIZE = 10;
        //be able to hold an instance of the Paginator
        public Paginator Pager { get; set; }
        #endregion

        public string InfoMessage { get; set; }
        public string ErrorMessage { get; set; }

        public void OnGet(int? currentPage)
        {
            try
            {
                //determine the current page number
                int pagenumber = currentPage.HasValue ? currentPage.Value : 1;
                //setup the current state of the paginator (sizing)
                PageState current = new(pagenumber, PAGE_SIZE);
                //temporary local integer to hold the results of the query's total collection size
                //  this will be need by the Paginator during the paginator's execution
                int totalcount;

                //we need to pass paging data into our query so that efficiencies in the
                //  system will ONLY return the amount of records to actually be display
                //  on the browser page.

                //QueryResultList = _territoryServices.List();
                QueryResultList = _territoryServices.List(
                                    pagenumber, PAGE_SIZE, out totalcount);

                //create the needed Pagnator instance
                Pager = new Paginator(totalcount, current);


                InfoMessage = $"Query returned {QueryResultList.Count} result.s";
            }
            catch
            {
                ErrorMessage = "Error reading territories";
            }
        }

        public void OnPostFilterByTerritory()
        {
            //if (TerritoryQuery != null)
            //{
            //    QueryResultList = _territoryServices.FindByPartialName(TerritoryQuery);
            //}
            //else
            //{
            //    FeedbackMessage = "You need to enter a territory first.";
            //}
        }

        public void OnPostFilterByRegion()
        {
            // Check if a valid region was selected
            //if (SelectedRegionId.HasValue)
            //{
            //    QueryResultList = _territoryServices.FindByRegionId(SelectedRegionId.Value);
            //}
            //else
            //{
            //    FeedbackMessage = "You must select a region first.";
            //}
        }
    }
}
