using Eshop.Classes;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace Eshop.Extentions
{
    public static class Extensions
    {
        public static T DeserializeToObject<T>(this ITempDataDictionary tempData, string key) where T : new()
        {
            string entry = tempData[key]?.ToString();

            T result = entry is null ? new T() : JsonConvert.DeserializeObject<T>(entry);

            return result;
        }

        public static void SerializeObject<T>(this ITempDataDictionary tempData, T obj, string key)
        {
            tempData[key] = JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        public static IHtmlContent RenderFlashMessages(this IHtmlHelper helper)
        {
            var messageList = helper.ViewContext.TempData.DeserializeToObject<List<FlashMessage>>("Messages");

            var html = new HtmlContentBuilder();

            foreach ( var message in messageList)
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
    }
}
