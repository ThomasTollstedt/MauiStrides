using CommunityToolkit.Mvvm.ComponentModel;
using MauiStrides.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MauiStrides.ViewModels
{
    [QueryProperty(nameof(Activity), "Activity")]
    public partial class ActivityDetailsViewModel : ObservableObject
    {
        [ObservableProperty]
       Activity activity;
    }
}
