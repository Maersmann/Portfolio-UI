using Aktien.Data.Model.WertpapierEntitys;
using Aktien.Data.Types.DividendenTypes;
using Aktien.Data.Types.WertpapierTypes;
using Aktien.Logic.Core.DividendeLogic;
using Aktien.Logic.Core.DividendeLogic.Classes;
using Aktien.Logic.Messages.Base;
using Aktien.Logic.UI.BaseViewModels;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Aktien.Logic.UI.DividendeViewModels
{
    public class DividendeProStueckAnpassenViewModel : ViewModelStammdaten
    {
        Dividende dividende;
        private double umrechnungskurs;

        public DividendeProStueckAnpassenViewModel()
        {
            Title = "Dividende pro Stück";
            dividende = new Dividende();
            RundungTyp = DividendenRundungTypes.Normal;
            umrechnungskurs = 0;
            OKCommand = new RelayCommand(() => ExecuteOKCommand());
        }

        private void ExecuteOKCommand()
        {
            new DividendeAPI().Speichern(dividende);
            Messenger.Default.Send<StammdatenGespeichertMessage>(new StammdatenGespeichertMessage { Erfolgreich = true, Message = "Dividende aktualisiert." }, "DividendeProStueckAnpassen");
        }

        public void LoadData(int dividendeErhaltenID, double umrechungskurs)
        {
            dividende = new DividendeAPI().LadeAnhandID(dividendeErhaltenID);
            umrechnungskurs = umrechungskurs;
            this.RaisePropertyChanged("Datum");
            this.RaisePropertyChanged("Betrag");
            this.RaisePropertyChanged("Waehrung");
            this.RaisePropertyChanged("Umrechnungskurs");
            this.RaisePropertyChanged("ErmittelterBetrag");
            this.RaisePropertyChanged("ErhaltenerBetrag");
        }

        #region Bindings

        public ICommand OKCommand { get; set; }

        public DateTime Datum { get { return dividende.Datum; } }
        public Double Betrag { get { return dividende.Betrag; } }
        public Waehrungen Waehrung { get { return dividende.Waehrung; } }
        public Double Umrechnungskurs { get { return umrechnungskurs; } }

        public Double ErmittelterBetrag { get { return new DividendenBerechnungen().BetragUmgerechnet(dividende.Betrag, umrechnungskurs, false, DividendenRundungTypes.Normal); } }
        public Double ErhaltenerBetrag { get { return new DividendenBerechnungen().BetragUmgerechnet(dividende.Betrag, umrechnungskurs, true, dividende.RundungArt); } }

        public IEnumerable<DividendenRundungTypes> RundungTypes
        {
            get
            {
                return Enum.GetValues(typeof(DividendenRundungTypes)).Cast<DividendenRundungTypes>();
            }
        }
        public DividendenRundungTypes RundungTyp
        {
            get { return dividende.RundungArt; }
            set
            {
                if ((this.dividende.RundungArt != value))
                {
                    this.dividende.RundungArt = value;
                    this.RaisePropertyChanged();
                    this.RaisePropertyChanged("ErhaltenerBetrag");
                }
            }
        }

        #endregion
    }
}
