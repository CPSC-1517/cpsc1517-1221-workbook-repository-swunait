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
        private const int PAGE_SIZE = 5;
        //be able to hold an instance of the Paginator
        public Paginator Pager { get; set; }
        #endregion

        public string InfoMessage { get; set; }
        public string ErrorMessage { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchBy { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? SearchValue { get; set; }

        public void OnGet(int? currentPage)
        {
            if (SearchBy != null && SearchValue != null)
            {
                if (SearchBy.ToLower() == "territory")
                {
                    //determine the current page number
                    int pagenumber = currentPage.HasValue ? currentPage.Value : 1;
                    //setup the current state of the paginator (sizing)
                    PageState current = new(pagenumber, PAGE_SIZE);
                    //temporary local integer to hold the results of the query's total collection size
                    //  this will be need by the Paginator during the paginator's execution
                    int totalcount;

                    try
                    {
                        QueryResultList = _territoryServices.FindByPartialName(SearchValue, pagenumber, PAGE_SIZE, out totalcount);
                        //create the needed Pagnator instance
                        Pager = new Paginator(totalcount, current);

                        InfoMessage = $"Query returned {QueryResultList.Count} results";
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = $"Error searching by territory with exception: {ex.Message} ";
                    }

                }
                else if (SearchBy.ToLower() == "region")
                {
                    //determine the current page number
                    int pagenumber = currentPage.HasValue ? currentPage.Value : 1;
                    //setup the current state of the paginator (sizing)
                    PageState current = new(pagenumber, PAGE_SIZE);
                    //temporary local integer to hold the results of the query's total collection size
                    //  this will be need by the Paginator during the paginator's execution
                    int totalcount;

                    try
                    {
                        int selectedRegionId = int.Parse(SearchValue);
                        QueryResultList = _territoryServices.FindByRegionId(selectedRegionId, pagenumber, PAGE_SIZE, out totalcount);
                        //create the needed Pagnator instance
                        Pager = new Paginator(totalcount, current);

                        InfoMessage = $"Query returned {QueryResultList.Count} results";
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = $"Error searching by region with exception: {ex.Message} ";
                    }
                }
                else
                {
                    ErrorMessage = $"{SearchBy} is not a supported search by value";
                }
            }
        }

        public IActionResult OnPostFilterByTerritory()
        {
            IActionResult nextPage = Page();
            if (TerritoryQuery != null)
            {
                //QueryResultList = _territoryServices.FindByPartialName(TerritoryQuery);
                SearchBy = "territory";
                SearchValue = TerritoryQuery;
                nextPage = RedirectToPage(new { SearchBy = SearchBy, SearchValue = TerritoryQuery });
            }
            else
            {
                FeedbackMessage = "You need to enter a territory first.";
            }

            return nextPage;
        }

        public IActionResult OnPostFilterByRegion()
        {
            IActionResult nextPage = Page();
            // Check if a valid region was selected
            if (SelectedRegionId.HasValue)
            {
                //QueryResultList = _territoryServices.FindByRegionId(SelectedRegionId.Value);
                SearchBy = "region";
                SearchValue = SelectedRegionId.Value.ToString();
                nextPage = RedirectToPage(new { SearchBy = SearchBy, SearchValue = SearchValue });
            }
            else
            {
                FeedbackMessage = "You must select a region first.";
            }

            return nextPage;
        }
    }
}
