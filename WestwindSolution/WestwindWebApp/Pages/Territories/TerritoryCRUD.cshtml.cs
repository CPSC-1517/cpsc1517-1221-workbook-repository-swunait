using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WestwindSystem.BLL;
using WestwindSystem.Entities;

namespace WestwindWebApp.Pages.Territories
{
    public class TerritoryCRUDModel : PageModel
    {
        private readonly RegionServices _regionService;
        private readonly TerritoryServices _territoryServices;

        // The EditTerritoryId property value is read from the route of the page
        [BindProperty(SupportsGet = true)]
        public string? EditTerritoryId { get; set; }

        // The current Territory to create, edit/udpate, or delete
        [BindProperty]
        public Territory? CurrentTerritory { get; set; }

        public SelectList RegionSelectList { get; private set; }

        [BindProperty(SupportsGet = true)]
        public int? SelectedRegionId { get; set; }

        [TempData]
        public string? InfoMessage { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public TerritoryCRUDModel(RegionServices regionServices, TerritoryServices territoryServices)
        {
            _regionService = regionServices;
            _territoryServices = territoryServices;

            RegionSelectList = new SelectList(_regionService.GetAll(), "RegionId", "RegionDescription");
        }

        private Exception GetInnerException(Exception ex)
        {
            //drill down to the REAL ERROR message
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            return ex;
        }

        public IActionResult OnPostSaveNew()
        {
            IActionResult nextPage = Page();

            if (SelectedRegionId.HasValue && SelectedRegionId.Value > 0)
            {
                int regionId = SelectedRegionId.Value;
                CurrentTerritory.RegionId = regionId;
                // Remove the Territory key from ModelState of the CurrentTerritory
                // to work around issue where the generated entities include navigation properties that are not set yet
                ModelState.Remove("CurrentTerritory.Region");
                //CurrentTerritory.Region = _regionService.GetById(SelectedRegionId.Value);

                if (ModelState.IsValid && CurrentTerritory != null)
                {
                    try
                    {
                        _territoryServices.AddTerritory(CurrentTerritory);
                        InfoMessage = "Save New was successful";
                        //EditTerritoryId = CurrentTerritory.TerritoryId;
                        nextPage = RedirectToPage(new { EditTerritoryId = CurrentTerritory.TerritoryId });
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = GetInnerException(ex).Message;
                    }
                }
                else
                {
                    ErrorMessage = $"<p>ModelState is not valid with the following errors:</p>";
                    ErrorMessage += "<ul>";
                    foreach (var modelState in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            ErrorMessage += $"<li>{error.ErrorMessage}</li>";
                        }
                    }
                    ErrorMessage += "</ul>";
                }
            }
            else
            {
                ErrorMessage = "A valid Region must be selected";
            }

            

            return nextPage;
        }

        public void OnGet()
        {
            if (EditTerritoryId != null)
            {
                CurrentTerritory = _territoryServices.GetById(EditTerritoryId);
                if (CurrentTerritory != null)
                {
                    SelectedRegionId = CurrentTerritory.RegionId;
                }
            }
            else
            {
                ErrorMessage = null;
            }
        }

        public IActionResult OnPostUpdate()
        {
            IActionResult nextPage = Page();

            if (SelectedRegionId.HasValue && SelectedRegionId.Value > 0)
            {
                int regionId = SelectedRegionId.Value;
                CurrentTerritory.RegionId = regionId;
                // Remove the Territory key from ModelState of the CurrentTerritory
                // to work around issue where the generated entities include navigation properties that are not set yet
                ModelState.Remove("CurrentTerritory.Region");
                CurrentTerritory.Region = _regionService.GetById(SelectedRegionId.Value);

                if (ModelState.IsValid && CurrentTerritory != null)
                {
                    try
                    {
                        int rowsAffected = _territoryServices.UpdateTerritory(EditTerritoryId, CurrentTerritory);
                        if (rowsAffected == 1)
                        {
                            InfoMessage = "Update was successful";
                        }
                        else
                        {
                            InfoMessage = "Update was not successful";
                        }

                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = GetInnerException(ex).Message;
                    }
                }
                else
                {
                    ErrorMessage = $"ModelState is not valid.";
                    foreach (var modelState in ViewData.ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            ErrorMessage += error.ErrorMessage + "<br />";
                        }
                    }
                }
            }
            else
            {
                ErrorMessage = $"A valid region must be selected.";
            }

           

            return nextPage;
        }

        public IActionResult OnPostDelete()
        {
            IActionResult nextPage = Page();

            if (EditTerritoryId != null)
            {
                try
                {
                    int rowsAffected = _territoryServices.DeleteTerritory(EditTerritoryId);
                    if (rowsAffected == 1)
                    {
                        InfoMessage = "Delete was successful.";
                        //nextPage = RedirectToPage("/Territories/Query", new { InfoMessage = InfoMessage});
                        nextPage = RedirectToPage(new { EditTerritoryId = (int?) null});
                    }
                    else
                    {
                        InfoMessage = "Delete was not successful.";
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage = GetInnerException(ex).Message;
                }
            }
            else
            {
                ErrorMessage = "Error! You have not selected an Territory to delete.";
            }
            return nextPage;
        }

        public IActionResult OnPostClear()
        {
            IActionResult nextPage = Page();
            EditTerritoryId = null;
            CurrentTerritory = new();
            ModelState.Clear();

            return nextPage;
        }

        public IActionResult OnPostSearch()
        {
            IActionResult nextPage = Redirect("/Territories/Query");

            return nextPage;
        }
    }
}
