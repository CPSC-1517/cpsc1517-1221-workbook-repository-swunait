using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using WestwindSystem.Entities;
using WestwindSystem.BLL;
using Microsoft.Identity.Client;
using WestwindWebApp.Helpers;

namespace WestwindWebApp.Pages.Territories
{
    public class IndexModel : PageModel
    {
        private readonly TerritoryServices _territoryServices;

        public IndexModel(TerritoryServices territoryServices)
        {
            _territoryServices = territoryServices;
        }

        public List<Territory>? QueryResultList { get; private set; }

        public string InfoMessage { get; set; }
        public string ErrorMessage { get; set; }

        #region Paginator
        //my desired page size
        private const int PAGE_SIZE = 10;
        //be able to hold an instance of the Paginator
        public Paginator Pager { get; set; }
        #endregion
       

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
    }
}
