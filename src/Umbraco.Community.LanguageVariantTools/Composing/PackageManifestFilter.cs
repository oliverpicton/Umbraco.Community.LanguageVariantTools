namespace Umbraco.Community.LanguageVariantTools.Composing;

using Umbraco.Cms.Core.Manifest;

internal sealed class PackageManifestFilter : IManifestFilter
{
    public void Filter(List<PackageManifest> manifests)
    {
        manifests.Add(new PackageManifest
        {
            PackageName = LanguageVariantToolsConstants.PackageAlias,
            Scripts = new[] {
                $"/App_Plugins/{LanguageVariantToolsConstants.PluginAlias}/backoffice/language-variant-tools/languageVariantToolsCreateVariantsController.js",
                $"/App_Plugins/{LanguageVariantToolsConstants.PluginAlias}/backoffice/language-variant-tools/languageVariantToolsRemoveVariantController.js"
            },
            Version = typeof(PackageManifestFilter)?.Assembly?.GetName()?.Version?.ToString(3) ?? string.Empty,
            AllowPackageTelemetry = true
        });
    }
}