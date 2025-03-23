using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Eshop.Classes
{
    public class RatingTagHelper : TagHelper
    {
        private double value = 0;

        public string Value
        {
            get => value.ToString();
            set => this.value = double.Parse(value);
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";

            for(int i = 0; i < Math.Round(value); i++)
            {
                var builder = new TagBuilder("span");
                builder.AddCssClass("bi bi-star-fill");
                output.Content.AppendHtml(builder);
            }

            for(int x = 0; x < 5 - Math.Round(value); x++)
            {
                var builder = new TagBuilder("span");
                builder.AddCssClass("bi bi-star");
                output.Content.AppendHtml(builder);
            }
        }
    }
}
