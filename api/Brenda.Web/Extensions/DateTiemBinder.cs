using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Brenda.Web.Extensions
{
    public class DateTimeBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(DateTime) || context.Metadata.ModelType == typeof(DateTime?))
            {
                return new BinderTypeModelBinder(typeof(DateTiemBinder));
            }

            return null;
        }
    }

    public class DateTiemBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var value = valueProviderResult.FirstValue;
            DateTime outDate;
            var parsed = DateTime.TryParse(value, CultureInfo.GetCultureInfo("pt-BR").DateTimeFormat,
                DateTimeStyles.None, out outDate);

            var result = ModelBindingResult.Success(outDate);
            if (!parsed)
            {
                result = ModelBindingResult.Failed();
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Data inválida");
            }

            bindingContext.Result = result;

            return Task.FromResult(0);
        }
    }
}
