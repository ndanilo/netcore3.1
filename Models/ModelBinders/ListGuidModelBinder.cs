using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Models.CustomAttributes;
using Models.DTO.Exceptions;

namespace Models.ModelBinders
{
    public class ListGuidModelBinder : IModelBinder
    {
        public ListGuidModelBinder()
        {
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            var defaultModelMetadata = (DefaultModelMetadata) bindingContext.ModelMetadata;

            var attribute = (ListItemErrorMessageAttribute)defaultModelMetadata.Attributes.Attributes.FirstOrDefault(t => t.GetType() == typeof(ListItemErrorMessageAttribute));

            var valueProvider = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProvider.Values.Count == 0)
            {
                if (attribute != null && !string.IsNullOrWhiteSpace(attribute.DefaultErrorMessage))
                    throw new BadRequestException(attribute.DefaultErrorMessage);

                bindingContext.Result = ModelBindingResult.Success(null);
            }

            var itemErrorFormat = new StringBuilder();
            var listGuid = new List<Guid>();

            foreach (var guidString in valueProvider.Values)
            {
                Guid guid;
                if (!Guid.TryParse(guidString, out guid))
                    if (attribute != null && !string.IsNullOrWhiteSpace(attribute.ItemErrorMessage))
                        itemErrorFormat.AppendLine(attribute.ItemErrorMessage.Replace("{0}", guidString));
                    else
                        itemErrorFormat.AppendLine($"invalid attribute '{guidString}'");
                else
                    listGuid.Add(guid);
            }

            if (itemErrorFormat.Length > 0)
                throw new BadRequestException(itemErrorFormat.ToString());

            bindingContext.Result = ModelBindingResult.Success(listGuid);
        }
    }
}
