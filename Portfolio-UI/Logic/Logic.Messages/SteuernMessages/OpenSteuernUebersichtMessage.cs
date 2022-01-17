using Data.Model.SteuerModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Messages.SteuernMessages
{
    public class OpenSteuernUebersichtMessage
    {
        public Action<bool, IList<SteuerModel>> Callback { get; private set; }
        public IList<SteuerModel> Steuern { get; set; }

        public OpenSteuernUebersichtMessage(Action<bool, IList<SteuerModel>> callback)
        {
            Callback = callback;
            Steuern = new List<SteuerModel>();
        }
        public OpenSteuernUebersichtMessage(Action<bool, IList<SteuerModel>> callback, IList<SteuerModel> steuern)
        {
            Callback = callback;
            Steuern = steuern;
        }
    }
}
