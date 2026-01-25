using CommunityToolkit.Mvvm.ComponentModel;
using MauiStrides.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MauiStrides.ViewModels
{
    [QueryProperty(nameof(Activity), nameof(Activity))]
    public partial class ActivityDetailsViewModel : ViewModelBase
    {
        [ObservableProperty]
        private Activity activity = new();

        public ActivityDetailsViewModel()
        {
            Title = "Activity Details";
        }
    }
}
