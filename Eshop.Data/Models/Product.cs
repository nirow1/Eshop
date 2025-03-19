using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop.Data.Models
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "vyplňte kód")]
        [StringLength(255, ErrorMessage = "´Kód je příliš dlouhý, max. 255 znaků")]
        [Display(Name = "´Kód produktu")]
        public string Code { get; set; }

        [Required(ErrorMessage ="Vyplňte kód")]
        [StringLength(255, ErrorMessage ="Kód je příliš dlouý, max. 255 znajků.")]
        [RegularExpression(@"^[a-z0-9\-]+$", ErrorMessage = "Používejte jen malá písmena bez diakritiky nebo číslice")]
        [Display(Name ="Url")]
        public string Url {  get; set; }

        [Required(ErrorMessage = "Vyplňte titulek")]
        [StringLength(255, ErrorMessage = "Titulek je příliš dlouý, max. 255 znajků.")]
        [Display(Name = "Titulek produktu")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Vyplňte krátký popisek")]
        [StringLength(255, ErrorMessage = "krátký popis je příliš dlouý, max. 255 znajků.")]
        [Display(Name = "Krátký popis")]
        public string ShortDescription {  get; set; }

        [Required(ErrorMessage = "Vyplňte popis")]
        [Display(Name = "Popis")]
        public string Description {  get; set; }

        [Required(ErrorMessage = "Vyplňte cenu")]
        [Range(0, double.MaxValue, ErrorMessage = "Cena nesmí být záporná")]
        [Display(Name = "Cena")]
        public decimal Price { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Cena před slevou nesmí být vyšší než nula")]
        [Display(Name = "Cena před slevou")]
        public decimal? OldPrice {  get; set; }

        [Required(ErrorMessage = "Vyplňte počet kusů ne skladě")]
        [Range(0, int.MaxValue, ErrorMessage = "Počet kusů na skladě nesmí být záporný")]
        [Display(Name = "Skladem")]
        public int Stock { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Cena nesmí být záporná")]
        [Display(Name = "Obrázků produktu celkem")]
        public int ImagesCount {  get; set; }

        [Display(Name = "Skrýt")]
        public bool Hidden {  get; set; }

        [NotMapped]
        public double Rating => 2.5;

        [NotMapped]
        public int DiscountPercent => OldPrice.HasValue && OldPrice.Value > Price ?
            (int)Math.Round((OldPrice.Value - Price) / OldPrice.Value * 100) : 0; 

        public virtual ICollection<CategoryProduct> CategoryProducts { get; set; }
        
        public Product()
        {
            ImagesCount = 0;
            Hidden = false;
            CategoryProducts = new List<CategoryProduct>();
        }
    }
}
