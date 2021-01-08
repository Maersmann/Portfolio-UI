using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Aktien.Logic.Messages.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Aktien.Logic.UI.BaseViewModels
{
    public class ViewModelBasis : ViewModelBase, INotifyDataErrorInfo
    {
        public ViewModelBasis()
        {
            CloseCommand = new RelayCommand(() => ExecuteCloseCommand());
        }

        public string Title { get; protected set; }

        public ICommand CloseCommand { get; private set; }

        public readonly Dictionary<string, ICollection<string>>
            ValidationErrors = new Dictionary<string, ICollection<string>>();

        protected virtual void ExecuteCloseCommand()
        {
            throw new NotImplementedException();   
        }


        public void OnlyNumbersCommand(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        public void OnlyBetragCommand(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Equals(","))
            {
                e.Handled = true;
            }
            else
            { 
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }
        }

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



    }
}
