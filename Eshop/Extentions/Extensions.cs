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

        public static string CreateImagePath(this IHtmlHelper helper, string imageName, string extension)
        {
            return $"/Images/Products/{imageName}.{extension}";
        }
    }
}
