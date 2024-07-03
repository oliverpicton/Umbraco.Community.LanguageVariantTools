
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Community.LanguageVariantTools.Models;
using Umbraco.Extensions;
using System.Linq;

namespace Umbraco.Community.LanguageVariantTools.Controllers
{
    [PluginController(LanguageVariantToolsConstants.PluginAlias)]
    public class LanguageVariantController : UmbracoAuthorizedApiController
    {
        private readonly IContentService _contentService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger<LanguageVariantController> _logger;

        public LanguageVariantController(IContentService contentService, ILocalizationService localizationService, ILogger<LanguageVariantController> logger)
        {
            _contentService = contentService;
            _localizationService = localizationService;
            _logger = logger;
        }

        public VariantResult Remove(int nodeId, string culture)
        {
            var content = _contentService.GetById(nodeId);

            var response =  new VariantResult
            {
                IsSuccess = false,
            };

            if (content == null)
            {
                var message = "No content node found with id {Id}.";
                _logger.LogCritical(message, nodeId);
                response.AddError(message, nodeId);
                return response;
            }

            if (string.IsNullOrEmpty(culture))
            {
                var message = "Provided culture is null or empty.";
                _logger.LogCritical(message);
                response.AddError(message);
                return response;
            }

            string cultureLower = culture.ToLower();

            if (content.CultureInfos != null && content.IsCultureAvailable(culture))
            {
                content.CultureInfos.Remove(cultureLower);

                foreach (var property in content.Properties)
                {
                    var valuesToRemove = property.Values
                        .Where(value => value.Culture?.Equals(culture, StringComparison.OrdinalIgnoreCase) == true)
                        .ToList();

                    foreach (var value in valuesToRemove)
                    {
                        property.SetValue(null, value.Culture, value.Segment);
                    }
                }

                var result = _contentService.Save(content);

                if (result.Success)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    var message = "Failed to save content";
                    _logger.LogCritical(message, culture);

                    response.AddError(message, culture);
                }
            }
            else
            {
                var message = "Culture {culture} not found in the collection.";
                _logger.LogCritical(message, culture);

                response.AddError(message, culture);
            }

            return response;
        }
    
        public VariantResult Create(int? nodeId, bool includeChildren, string culture)
        {
            var response = new VariantResult
            {
                IsSuccess = false
            };

            if (nodeId is null)
            {
                response.AddError("NodeId is not set");
                return response;
            }

            var contentItems = new List<IContent>();
            var contentItem = _contentService.GetById(nodeId.Value);

            if (contentItem == null)
            {                  
                response.AddError("content is not found");
                return response;
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
    }
}