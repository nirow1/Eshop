using Eshop.Business.Interfaces;
using Eshop.Classes;
using Eshop.Data.Models;
using Eshop.Extentions;
using Eshop.Models.ProductViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Eshop.Controllers
{
    [ExtentionsToMessageFiler]
    public class ProductController : Controller
    {
        private readonly ICategoryManager categoryManager;
        private readonly IProductManager productManager;

        public ProductController(ICategoryManager categoryManager, IProductManager productManager)
        {
            this.categoryManager = categoryManager;
            this.productManager = productManager;
        }

        private List<Category> GetAvailableCategories()
        {
            return categoryManager.GetAll().OrderBy(c => c.TitlesPath).ToList();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult ManageProduct(string url)
        {
            var model = new ManageProductViewModel
            {
                FormCaption = string.IsNullOrWhiteSpace(url) ? "Nový produkt" : "Editace Produktu"
            };

            model.Product = string.IsNullOrWhiteSpace(url) ? new Product() :
                productManager.FindProductByUrl(url) ?? throw new NullReferenceException($"Produkt {url} nenalezen");

            model.AvailableCategories = GetAvailableCategories();

            if (!string.IsNullOrWhiteSpace(url))
                model.PostedCategories = productManager.FindAssignedCategoriesToProduct(model.AvailableCategories, model.Product.CategoryProducts.ToList(), model.PostedCategories);

            model.Price = model.Product.Price.ToString(CultureInfo.InvariantCulture);
            model.OldPrice = model.Product.OldPrice?.ToString(CultureInfo.InvariantCulture);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult ManageProduct(ManageProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.FormCaption = model.Product.ProductId == 0 ? "Nový produkt" : "Editace produktu";

                model.AvailableCategories = GetAvailableCategories();
                this.AddFlashMessage("Špatné parametry výrobku", FlashMessageType.Danger);
                return View(model);
            }

            model.Product.Price = decimal.Parse(model.Price, NumberStyles.Any, CultureInfo.InvariantCulture);
            if(decimal.TryParse(model.OldPrice, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal oldPrice))
                model.Product.OldPrice = oldPrice;

            List<Category> availableCategories = GetAvailableCategories();

            var selectedCategories = availableCategories.Where(c => model.PostedCategories[availableCategories.IndexOf(c)]);
            var selectedCategoryIds = new HashSet<int>();
            foreach(var category in selectedCategories)
            {
                selectedCategoryIds.Add(category.CategoryId);

                var parent = category.ParentCategory;

                while(parent is not null)
                {
                    selectedCategoryIds.Add(parent.CategoryId);
                    parent = parent.ParentCategory;
                }
            }

            productManager.SaveProduct(model.Product);
            categoryManager.UpdateProductCategories(model.Product.ProductId, selectedCategoryIds);

            this.AddFlashMessage("Produkt byl úspěšně uložen", FlashMessageType.Success);
            return RedirectToAction(actionName: "Administration", controllerName: "Account");
        }

    }
}
