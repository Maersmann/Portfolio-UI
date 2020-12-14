using GalaSoft.MvvmLight;

namespace Logic.UI
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                Title = "MvvSample (Designmode)";
            }
            else
            {
                Title = "Aktien";
            }
        }

        public string Title { get; private set; }
    }
}