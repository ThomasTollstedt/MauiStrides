using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MauiStrides.ViewModels
{
    public partial class ViewModelBase : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))] // Smart trick: Uppdatera IsNotBusy när IsBusy ändras
        private bool isBusy;

        [ObservableProperty]
        private string title;

        // Hjälp-property som är tvärtom mot IsBusy (bra för att disabla knappar i XAML)
        public bool IsNotBusy => !IsBusy;



    }
}
