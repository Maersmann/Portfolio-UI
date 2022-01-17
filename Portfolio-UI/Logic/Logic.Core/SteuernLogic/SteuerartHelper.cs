using Data.Model.SteuerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Core.SteuernLogic
{
    public class SteuerartHelper
    {
        public static IList<SteuerartModel> ErmittelAusSteuern(IList<SteuerModel> steuern)
        {
            var Steuerarten = new List<SteuerartModel>();
            steuern.ToList().ForEach(steuer =>
           {
               Steuerarten.Add(steuer.Steuerart);
           });
            return Steuerarten;
        }
    }
}
