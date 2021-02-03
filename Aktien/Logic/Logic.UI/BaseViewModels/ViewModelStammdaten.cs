using Aktien.Data.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.BaseViewModels
{
    public class ViewModelStammdaten : ViewModelValidate
    {
        protected State state;

        protected bool LoadAktie;
        public ICommand SaveCommand { get; protected set; }

        public ViewModelStammdaten()
        {
            LoadAktie = false;
        }

        protected bool CanExecuteSaveCommand()
        {
            return ValidationErrors.Count == 0;
        }

        protected virtual void ExecuteSaveCommand()
        {
            throw new NotImplementedException();
        }
    }
}
