using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models.Trees;
using Umbraco.Cms.Core.Notifications;
using static Umbraco.Cms.Core.Constants;

namespace Umbraco.Community.LanguageVariantTools.Notifications.Handlers
{
    public class RemoveLanguageVariantMenuRenderingHandler : INotificationHandler<MenuRenderingNotification>
    {
        public void Handle(MenuRenderingNotification notification)
        {
            if (notification.TreeAlias == Applications.Content)
            {
                var menuItem = new MenuItem("removeLanguageVariant", "Remove variant")
                {
                    Icon = "backspace"
                };

                menuItem.AdditionalData.Add("actionView", $"../App_Plugins/{LanguageVariantToolsConstants.PluginAlias}/backoffice/language-variant-tools/removeVariant.html");

                notification.Menu.Items.Add(menuItem);
            }
        }
    }
}