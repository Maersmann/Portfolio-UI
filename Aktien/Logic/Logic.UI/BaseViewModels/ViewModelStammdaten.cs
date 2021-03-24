using Aktien.Data.Types;
using Aktien.Logic.Core.Interfaces;
using Aktien.Logic.Messages.Base;
using GalaSoft.MvvmLight.Messaging;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.BaseViewModels
{
    public class ViewModelStammdaten<T> : ViewModelValidate
    {
        protected T data;
        protected State state;
        protected bool LoadAktie;
        private readonly IAPI<T> api;

        public ICommand SaveCommand { get; protected set; }

        public ViewModelStammdaten()
        {
            LoadAktie = false;
            SaveCommand = new DelegateCommand(this.ExecuteSaveCommand, this.CanExecuteSaveCommand);
            Cleanup();
        }
        public ViewModelStammdaten(IAPI<T> api) : this()
        {
            this.api = api;
        }


        protected virtual StammdatenTypes GetStammdatenTyp() { return 0; }

        protected virtual bool CanExecuteSaveCommand()
        {
            return ValidationErrors.Count == 0;
        }

        protected virtual void ExecuteSaveCommand()
        {
            if (state.Equals(State.Neu))
            {
                api.Speichern(data);
                Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Gespeichert" }, GetStammdatenTyp());
            }
            else
            {
                api.Aktualisieren(data);
                Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Aktualisiert" }, GetStammdatenTyp());
            }
            Messenger.Default.Send<AktualisiereViewMessage>(new AktualisiereViewMessage(), GetStammdatenTyp());
        }
    }
}
