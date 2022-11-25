using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

#region namespaces for BLL and Entities
using WestwindSystem.BLL;
using WestwindSystem.Entities;
#endregion

namespace WestwindWebApp.Pages.Products
{
    public class ProductCrudModel : PageModel
    {
        #region internal fields
        private readonly CategoryServices _categoryServices;
        private readonly ProductServices _productServices;
        private readonly SupplierServices _supplierServices;

        #endregion

        #region data properties for page
        [BindProperty(SupportsGet = true)]
        public int? EditProductId { get; set; }

        // Data source for category select element
        public SelectList CategorySelectList { get; private set; }
        // Bindable property for value selected from select element
        [BindProperty]
        public int? SelectedCategoryId { get; set; }

        public SelectList SupplierSelectList { get; private set; }
        [BindProperty]
        public int? SelectedSupplierId { get; set; }

        // The CurrentProduct to add, update, or delete.
        [BindProperty]
        public Product CurrentProduct { get; set; }

        public string InfoMessage { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region constructor with depdendencies
        public ProductCrudModel(
            CategoryServices categoryServices,
            ProductServices productServices,
            SupplierServices supplierServices)
        {
            _categoryServices = categoryServices;
            _supplierServices = supplierServices;
            _productServices = productServices;

            // Fetch list of categories
            List<Category> categories = _categoryServices.List();
            CategorySelectList = new SelectList(categories, "Id", "CategoryName");

            // Fetch list of suppliers
            List<Supplier> suppliers = _supplierServices.List();
            SupplierSelectList = new SelectList(suppliers, "SupplierId", "ListItemText");

        }

        #endregion

        #region page handlers
        public void OnGet()
        {
            if (EditProductId != null)
            {
                int productId = EditProductId.Value;
                var querySingleResult = _productServices.GetById(productId);
                if (querySingleResult != null)
                {
                    CurrentProduct = querySingleResult;
                }
                else
                {
                    ErrorMessage = $"Invalid product id of {EditProductId}";
                }
            }
        }

        #endregion
    }
}
