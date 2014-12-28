using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace CarpoolPlanner
{
    public class KeyedCollectionModelBinder : DefaultModelBinder
    {
        #region IModelBinder Members

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            bool contains = bindingContext.ValueProvider.ContainsPrefix(bindingContext.ModelName);
            string indexKey = CreateSubPropertyName(bindingContext.ModelName, "index");
            bool filter = bindingContext.PropertyFilter(indexKey);
            var model = new CarpoolPlanner.Model.UserTripRecurrenceCollection();
            var md = ModelMetadataProviders.Current.GetMetadataForType(() => model, bindingContext.ModelType);
            var valueProviderResult = bindingContext.ValueProvider.GetValue(indexKey);
            var bc2 = bindingContext;
            var valueProviderResult2 = bc2.ValueProvider.GetValue(indexKey);
            //var eys = bc2.ValueProvider.GetK

            var valResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + ".index");

            return model;
        }

        #endregion
    }
}