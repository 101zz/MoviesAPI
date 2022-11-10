using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace MoviesAPI.Helpers
{
    public class TypeBinder<T> : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var proprtyname = bindingContext.ModelName;
            var value = bindingContext.ValueProvider.GetValue(proprtyname);

            if (value == ValueProviderResult.None)
                return Task.CompletedTask;
            else
            {
                try
                {
                    var deserializedValue = JsonConvert.DeserializeObject<T>(value.FirstValue);
                    bindingContext.Result = ModelBindingResult.Success(deserializedValue);
                }
                catch
                {
                    bindingContext.ModelState.TryAddModelError(proprtyname, "The given value is not of the correct type");
                }

                return Task.CompletedTask;
            }
        }
    }
}
