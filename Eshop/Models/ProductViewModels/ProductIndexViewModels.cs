using Eshop.Data.Classes;
using Eshop.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using X.PagedList;

namespace Eshop.Models.ProductViewModels
{
    public class ProductIndexViewModels
    {
        public IPagedList<Product> Products { get; set; }

        public Product Product { get; set; }

        [Display(Name = "Cena od")]
        public decimal? StartPrice { get; set; }

        [Display(Name = "Cena do")]
        public decimal? EndPrice { get; set;}

        [Display(Name = "Skladem")]
        public bool InStock {  get; set; }

        [Display(Name = "Řadit podle")]
        public string SortCriteria {  get; set; }

        public int? CurrentCategoryId {  get; set; }

        public string CurrentPhrase {  get; set; }

        public bool AreProductsEditable {  get; set; }

        public List<SelectListItem> SortList { get; set; } = new List<SelectListItem>()
        {
            new SelectListItem{ Text = "Nejnovější", Value = OrderProductBy.Newest},
            new SelectListItem{ Text = "Nejlevnějsí ceny", Value = OrderProductBy.LowestPrice},
            new SelectListItem{ Text = "Nejdražší ceny", Value = OrderProductBy.HighestPrice},
            new SelectListItem{ Text = "hodnocení", Value = OrderProductBy.Rating}
        };
    }
}
