using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Logic.UI.BaseModels
{
    public class ViewModelStammdatan : ViewModelBasis
    {
        public ICommand SaveNeueAktieCommand { get; protected set; }

        protected bool CanExecuteSaveNeueAktieCommand(String arg)
        {
            return ValidationErrors.Count == 0;
        }

        protected virtual void ExecuteSaveNeueAktieCommand(String arg)
        {
            throw new NotImplementedException();
        }
    }
}
