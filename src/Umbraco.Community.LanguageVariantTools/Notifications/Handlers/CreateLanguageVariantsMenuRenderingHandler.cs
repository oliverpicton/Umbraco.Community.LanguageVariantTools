using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Models.Trees;

namespace Umbraco.Community.LanguageVariantTools.Notifications.Handler
{
    public class CreateLanguageVariantsMenuRenderingHandler : INotificationHandler<MenuRenderingNotification>
    {
        public void Handle(MenuRenderingNotification notification)
        {
            if (notification.TreeAlias.Equals("content"))
            {
                var menuItem = new MenuItem("createVariants", "Create variants");

                menuItem.AdditionalData.Add("actionView", $"../App_Plugins/{LanguageVariantToolsConstants.PluginAlias}/backoffice/language-variant-tools/createVariants.html");

                menuItem.Icon = "globe";

                var index = notification.Menu.Items.Count;
                notification.Menu.Items.Insert(index, menuItem);
            }
        }
    }
}