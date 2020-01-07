using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Models.CustomAttributes;

namespace Models.ModelBinders
{
    public class ListGuidModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType != typeof(IList<Guid>))
                return null;

            return new ListGuidModelBinder();
        }
    }
}
