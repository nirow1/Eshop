using Eshop.Classes;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Extentions
{
    public static class ControllerExtentions
    {

        public static void AddFlashMessage(this Controller controller, FlashMessage message)
        {

            var list = controller.TempData.DeserializeToObject<List<FlashMessage>>("Messages");

            list.Add(message);

            controller.TempData.SerializeObject(list, "Messages");
        }

        public static void AddFlashMessge(this Controller controller, string message, FlashMessageType messageType)
        {
            controller.AddFlashMessage(new FlashMessage(message, messageType));
        }
    }
}
