using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Data.Models
{
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Vyplňte url")]
        [StringLength(255, ErrorMessage = "Url je příliš dlouhá, max. 255 znaků")]
        [RegularExpression(@"^[a-z0-9\-]+$", ErrorMessage = "Používejte jen malá písmena bez diakritiky nebo číslice")]
        [Display(Name = "Url")]
        public string Url { get; set; }

        [Required(ErrorMessage = "Vyplňte titulek")]
        [StringLength(255, ErrorMessage = "Titulek je příliš dlouhý, max. 255 znaků")]
        [Display(Name = "Titulek")]
        public string Title { get; set; }

        [Required]
        public int OrderNo { get; set; }

        [Required]
        [Display(Name = "Skrýt")]
        public bool Hidden { get; set; }

        public int? ParentCategoryId { get; set; }

        public virtual ICollection<CategoryProduct> CategoryProducts { get; set; }

        public Category()
        {
            CategoryProducts = new List<CategoryProduct>();
        }
    }
}
