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

        protected void AddValidateInfo(Boolean valid, String propertyKey, ICollection<string> validationErrors)
        {
            if (!valid)
            {

                ValidationErrors[propertyKey] = validationErrors;

                RaiseErrorsChanged(propertyKey);
            }
            else if (ValidationErrors.ContainsKey(propertyKey))
            {

                ValidationErrors.Remove(propertyKey);

                RaiseErrorsChanged(propertyKey);
            }
        }

        protected void DeleteValidateInfo(string propertyKey)
        {
            if (ValidationErrors.ContainsKey(propertyKey))
            {

                ValidationErrors.Remove(propertyKey);

                RaiseErrorsChanged(propertyKey);
            }
        }
    }
}
