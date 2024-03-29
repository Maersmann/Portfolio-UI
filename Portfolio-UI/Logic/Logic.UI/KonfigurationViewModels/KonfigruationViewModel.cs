﻿using Base.Logic.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using Logic.Core.OptionenLogic;
using Logic.Messages.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Logic.UI.KonfigurationViewModels
{
    public class KonfigruationViewModel : ViewModelBasis
    {
        public KonfigruationViewModel()
        {
            Title = "Konfiguration der Software";
        }

        protected override void ExecuteCloseWindowCommand(Window window)
        {
            base.ExecuteCloseWindowCommand(window);
            if (!new BackendLogic().istINIVorhanden())
            {
                 WeakReferenceMessenger.Default.Send(new CloseApplicationMessage());
            }
        }


    }
}
