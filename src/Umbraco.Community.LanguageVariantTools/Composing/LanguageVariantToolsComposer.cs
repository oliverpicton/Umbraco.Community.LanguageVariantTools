using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Community.LanguageVariantTools.Notifications.Handler;
using Umbraco.Community.LanguageVariantTools.Notifications.Handlers;

namespace Umbraco.Community.LanguageVariantTools.Composing
{
    public class LanguageVariantToolsComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.ManifestFilters().Append<PackageManifestFilter>();
            builder.Services.AddTransient<ITranslationService, OpenAiTranslationService>();
            builder.AddNotificationHandler<MenuRenderingNotification, CreateLanguageVariantsMenuRenderingHandler>();
            builder.AddNotificationHandler<MenuRenderingNotification, RemoveLanguageVariantMenuRenderingHandler>();
        }
    }
}