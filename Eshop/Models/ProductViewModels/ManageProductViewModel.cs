using Eshop.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Eshop.Models.ProductViewModels
{
    public class ManageProductViewModel
    {
        private List<Category> availableCategories;

        public Product Product { get; set; }

        public List<Category> AvailableCategories
        {
            get => availableCategories;
            set
            {
                availableCategories = value;
                PostedCategories = new bool[value.Count];
            }
        }

        [Required(ErrorMessage = "musíte vybrat nejméně jednu kategoii pro produkt")]
        [Display(Name = "Kategorie")]
        public bool[] PostedCategories { get; set; }

        [Display(Name = "Nahrát obrázky")]
        public List<IFormFile> UploadedImages { get; set; }
        public string FormCaption {  get; set; }
        public string Price {  get; set; }
        public string OldPrice {  get; set; }

        public ManageProductViewModel()
        {
            Product = new Product();
            AvailableCategories = new List<Category>();
            PostedCategories = new bool[0];
            UploadedImages = new List<IFormFile>();
        }
    }
}
