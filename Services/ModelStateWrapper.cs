using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace Services
{
    public class ModelStateWrapper : IValidationDictionary
    {
        #region Constructors and fields
        private ModelStateDictionary modelState;
        public ModelStateWrapper(ModelStateDictionary modelState)
        {
            this.modelState = modelState;
        }

        #endregion

        public void AddError(string key, string errorMessage)
        {
            this.modelState.AddModelError(key, errorMessage);
        }

        public bool isValid
        {
            get
            {
                return this.modelState.IsValid;
            }
        }

        public ModelStateDictionary ModelState
        {
            get
            {
                return this.modelState;
            }
        }
    }
}
