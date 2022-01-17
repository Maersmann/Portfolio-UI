using Aktien.Logic.Messages.Base;
using Base.Logic.Messages;
using Data.Model.SteuerModels;
using Data.Types.SteuerTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Messages.SteuernMessages
{
    public class OpenSteuerStammdatenMessage<T> : BaseStammdatenMessage<T>
    {
        public SteuerModel Steuer { get; set; }
        public IList<SteuerartModel> Steuerarts { get; set; }

        public Action<bool, SteuerModel> Callback { get; private set; }

        public OpenSteuerStammdatenMessage(Action<bool, SteuerModel> callback, SteuerModel steuer, IList<SteuerartModel> steuerarts)
        {
            Callback = callback;
            Steuer = steuer;
            Steuerarts = steuerarts;
        }
    }
}
