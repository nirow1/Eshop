using Eshop.Classes;
using Eshop.Data.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.CompilerServices;

namespace Eshop.Extentions
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent RenderFlashMessages(this IHtmlHelper helper)
        {
            var messageList = helper.ViewContext.TempData.DeserializeToObject<List<FlashMessage>>("Messages");

            var html = new HtmlContentBuilder();

            foreach (var message in messageList)
            {
                var container = new TagBuilder("div");
                container.AddCssClass($"alert alert-dismissible fade show alert-{message.MessageType.ToString().ToLower()}");
                container.InnerHtml.SetContent(message.Message);

                var dismissButton = new TagBuilder("button");
                dismissButton.AddCssClass("btn-close");
                dismissButton.Attributes.Add("type", "button");
                dismissButton.Attributes.Add("data-bs-dismiss", "alert");
                container.InnerHtml.AppendHtml(dismissButton);

                html.AppendHtml(container);

            }
            return html;
        }

        private static bool ContainsCategoryId(IEnumerable<Category> children, int categoryId)
        {
            return children.Any(c  => c.CategoryId == categoryId || ContainsCategoryId(c.ChildCategories, categoryId));
        }

        private static TagBuilder CreateCategoryAnchorTag(string title, int categoryId)
        {
            var anchorTag = new TagBuilder("a");
            anchorTag.AddCssClass("nav-link py-3");
            anchorTag.Attributes.Add("href", categoryId > 0 ? "/Produckty?categoryId=" + categoryId : "/Produkty");
            anchorTag.InnerHtml.SetContent(title);

            return anchorTag;
        }

        private static TagBuilder CreateCategoryButtonTag(bool show, string contentId)
        {
            var buttonTag = new TagBuilder("button");
            buttonTag.AddCssClass("accordion-button");
            buttonTag.Attributes.Add("type", "button");
            buttonTag.Attributes.Add("data-bs-toggle", "collapse");
            buttonTag.Attributes.Add("data-bs-target", $"#{contentId}");
            buttonTag.Attributes.Add("aria-controls", contentId);

            if (show)
                buttonTag.Attributes.Add("aria-expanded", "true");
            else
            {
                buttonTag.Attributes.Add("aria-expanded", "false");
                buttonTag.AddCssClass("collapsed");
            }
            return buttonTag;
        }

        private static TagBuilder CreateCategoryUlTag(string parentAccordionId, string accordionId, bool show)
        {
            var ulTag = new TagBuilder("ul");

            ulTag.AddCssClass("accordion accordion-flush list-unstyled");
            ulTag.Attributes.Add("id", accordionId);

            if (!string.IsNullOrWhiteSpace(parentAccordionId))
            {
                ulTag.AddCssClass("accordion-collapse collapse ps-3");
                ulTag.Attributes.Add("data-bs-paret", $"#{parentAccordionId}");
            }

            if (show)
                ulTag.AddCssClass("show");

            return ulTag;
        }
        public static IHtmlContent RenderCategories(this IHtmlHelper helper, IEnumerable<Category> categories,
                                                string selectedCategoryId = "", int parentCategoryId = 0,
                                                string parentAccordionId = "", bool show = false)
        {
            var accordionId = $"accordion-{parentCategoryId}";
            var ulTag = CreateCategoryUlTag(parentAccordionId, accordionId, show);

            foreach(var category in categories)
            {
                var liTag = new TagBuilder("li");

                if(category.ChildCategories.Count > 0)
                {
                    if(!string.IsNullOrWhiteSpace(selectedCategoryId))
                    {
                        var categoryId = int.Parse(selectedCategoryId);
                        show = ContainsCategoryId(category.ChildCategories, categoryId);
                    }

                    var childAccordionId = $"accordion-{category.CategoryId}";
                    var buttonTag = CreateCategoryButtonTag(show, childAccordionId);

                    liTag.InnerHtml.SetHtmlContent(buttonTag);
                    buttonTag.InnerHtml.SetContent(category.Title);
                    liTag.InnerHtml.AppendHtml(RenderCategories(helper, category.ChildCategories.OrderBy(c => c.OrderNo), selectedCategoryId, category.CategoryId, accordionId, show));
                }
                else
                {
                    var anchorTag = CreateCategoryAnchorTag(category.Title, category.CategoryId);

                    liTag.AddCssClass("nav-item ms-3 ps-1");
                    liTag.InnerHtml.SetHtmlContent(anchorTag);
                }

                ulTag.InnerHtml.AppendHtml(liTag);
            }

            var allLiTag = new TagBuilder("li");
            var AllAnchorTag = CreateCategoryAnchorTag("Všechno", parentCategoryId);
            allLiTag.AddCssClass("nav-item ms-3 ps-1");
            allLiTag.InnerHtml.SetHtmlContent(AllAnchorTag);
            ulTag.InnerHtml.AppendHtml(allLiTag);

            var contentBuilder = new HtmlContentBuilder().AppendHtml(ulTag);

            return contentBuilder;
        }
    }
}
