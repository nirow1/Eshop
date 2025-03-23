using Eshop.Business.Interfaces;
using Eshop.Classes;
using Eshop.Data.Classes;
using Eshop.Data.Models;
using Eshop.Extentions;
using Eshop.Models.ProductViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using X.PagedList.Extensions;

namespace Eshop.Controllers
{
    [ExtentionsToMessageFiler]
    public class ProductController : Controller
    {
        private readonly ICategoryManager categoryManager;
        private readonly IProductManager productManager;
        private const int PageSize = 12;

        public ProductController(ICategoryManager categoryManager, IProductManager productManager)
        {
            this.categoryManager = categoryManager;
            this.productManager = productManager;
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

            int oldProductId = model.Product.ProductId;
            int oldImagesCount = model.Product.ImagesCount;

            productManager.SaveProduct(model.Product);
            categoryManager.UpdateProductCategories(model.Product.ProductId, selectedCategoryIds);

            productManager.SaveProductImages(model.Product, model.UploadedImages, oldProductId, oldImagesCount);

            this.AddFlashMessage("Produkt byl úspěšně uložen", FlashMessageType.Success);
            return RedirectToAction(actionName: "Administration", controllerName: "Account");
        }

        [Authorize(Roles = "Admin")]
        public void DeleteImage(int productID, int imageIndex) 
            => productManager.RemoveProductImage(productID, imageIndex);
        
        public IActionResult Index(int? categoryId, string searchPhrase, int? page, ProductIndexViewModels model)
        {
            if (categoryId.HasValue)
            {
                searchPhrase = String.Empty;
                model.CurrentPhrase = string.Empty;
                model.CurrentCategoryId = categoryId;

                model.Products = productManager.FindByCategoryId(categoryId.Value).ToPagedList(1, PageSize);

                ViewData["CurrentCategoryTitle"] = categoryManager.GetCategoryById(categoryId.Value).Title;
            }
            else if(searchPhrase is not null)
            {
                model.Products = productManager.FindBySearchPhrase(searchPhrase).ToPagedList(1, PageSize);
                model.CurrentPhrase = searchPhrase;
                model.CurrentCategoryId = null;
            }
            else
            {
                searchPhrase = model.CurrentPhrase;
                model.Products = productManager.FindBy(
                    model.CurrentPhrase,
                    model.CurrentCategoryId,
                    model.SortCriteria ?? OrderProductBy.Newest,
                    model.StartPrice is null ? 0 : model.StartPrice.Value,
                    model.EndPrice is null ? int.MaxValue : model.EndPrice.Value,
                    model.InStock
                    ).ToPagedList(page ?? 1, PageSize);
                if (model.CurrentCategoryId.HasValue)
                    ViewData["CurrentCategoryTitle"] = categoryManager.GetCategoryById(model.CurrentCategoryId.Value).Title;
            }

            if (!string.IsNullOrWhiteSpace(searchPhrase))
                ViewData["SearchPhrase"] = searchPhrase;

            model.AreProductsEditable = true;

            return View(model);
        }
        private List<Category> GetAvailableCategories() 
            => categoryManager.GetAll().OrderBy(c => c.TitlesPath).ToList();
    }
}
