using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aktien.Logic.UI.BaseViewModels
{
    public class ViewModelValidate : ViewModelBasis, INotifyDataErrorInfo
    {
        public readonly Dictionary<string, ICollection<string>>
            ValidationErrors = new Dictionary<string, ICollection<string>>();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)
                || !ValidationErrors.ContainsKey(propertyName))
                return null;

            return ValidationErrors[propertyName];
        }

        public bool HasErrors
        {
            get { return ValidationErrors.Count > 0; }
        }

        protected void AddValidateInfo(Boolean inValid, String inPropertyKey, ICollection<string> inValidationErrors)
        {
            if (!inValid)
            {

                ValidationErrors[inPropertyKey] = inValidationErrors;

                RaiseErrorsChanged(inPropertyKey);
            }
            else if (ValidationErrors.ContainsKey(inPropertyKey))
            {

                ValidationErrors.Remove(inPropertyKey);

                RaiseErrorsChanged(inPropertyKey);
            }
        }

        protected void DeleteValidateInfo(string inPropertyKey)
        {
            if (ValidationErrors.ContainsKey(inPropertyKey))
            {

                ValidationErrors.Remove(inPropertyKey);

                RaiseErrorsChanged(inPropertyKey);
            }
        }
    }
}
