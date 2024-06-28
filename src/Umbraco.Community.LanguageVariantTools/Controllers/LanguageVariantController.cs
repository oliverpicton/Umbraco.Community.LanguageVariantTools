using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Community.LanguageVariantTools.Models;
using Umbraco.Extensions;

namespace Umbraco.Community.LanguageVariantTools.Controllers
{
    [PluginController(LanguageVariantToolsConstants.PluginAlias)]
    public class LanguageVariantController : UmbracoAuthorizedApiController
    {
        private readonly IContentService _contentService;
        private readonly ILocalizationService _localizationService;

        public LanguageVariantController(IContentService contentService, ILocalizationService localizationService)
        {
            _contentService = contentService;
            _localizationService = localizationService;
        }

        public VariantResult Remove(int nodeId, string culture)
        {
            try
            {
                var node = _contentService.GetById(nodeId);

                node.CultureInfos.Remove(culture.ToLower());

                var result = _contentService.Save(node);

                return new VariantResult
                {
                    IsSuccess = true,
                };

            }
            catch (Exception e)
            {
                return new VariantResult
                {
                    IsSuccess = false,
                    Error = new Error
                    {
                        Message = e.Message,
                    },
                };
            }
        }
    
        public VariantResult Create(int? nodeId, bool includeChildren, string culture)
        {
            try
            {
                if (nodeId is null)
                {
                    return new VariantResult
                    {
                        IsSuccess = false,
                        Error = new Error
                        {
                            Message = "NodeId is not set",
                        },
                    };
                }

                var contentItems = new List<IContent>();
                var contentItem = _contentService.GetById(nodeId.Value);

                if (contentItem == null)
                {
                    return new VariantResult
                    {
                        IsSuccess = false,
                        Error = new Error
                        {
                            Message = "content is not found",
                        },
                    };
                }

                contentItems.Add(contentItem);

                if (includeChildren)
                {
                    contentItems.AddRange(_contentService.GetPagedChildren(contentItem.Id, 0, int.MaxValue, out long totalRecords));
                }

                var languages = _localizationService.GetAllLanguages();

                foreach (var content in contentItems)
                {
                    var contentItemLanguages = content.AvailableCultures;

                    foreach (var targetCulture in languages.Select(x => x.CultureInfo.Name))
                    {
                        var isCreated = contentItemLanguages.Any(x => x == targetCulture.ToLower());

                        if (isCreated) continue;

                        content.SetCultureName(content.Name, targetCulture);

                        foreach (var property in content.Properties)
                        {
                            var value = property.Values.FirstOrDefault(p => p.Culture != null && p.Culture.ToLower() == culture.ToLower())?.EditedValue;

                            if (value is not null)
                            {
                                try
                                {
                                    content.SetValue(property.Alias, value, targetCulture);
                                }
                                catch { continue; }
                            }
                        }
                    }

                    _contentService.Save(content);
                }

                return new VariantResult
                {
                    IsSuccess = true,
                };

            }
            catch (Exception e)
            {
                return new VariantResult
                {
                    IsSuccess = false,
                    Error = new Error
                    {
                        Message = e.Message,
                    },
                };
            }
        }
    }
}