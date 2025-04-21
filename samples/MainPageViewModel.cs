using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fonts;
using MauiSparkline.Models;

namespace MauiSparkline;

public partial class MainPageViewModel: ObservableObject
{
    public ObservableCollection<SparklineItem> Sparklines { get; set; } = new ObservableCollection<SparklineItem>();
    
    [RelayCommand]
     private Task LoadItems()
     {
        Sparklines.Clear();
        Sparklines.Add(new SparklineItem
        {
            Name = "API.VOUCHER",
            Description = "Voucher reference data",
            Image = FluentUI.arrow_redo_48_regular,
            Threshold = 50,
            Sparkies = [12, 45, 78, 23, 56, 89, 34, 67, 90, 21]
        });
        Sparklines.Add(new SparklineItem
        {
            Name = "API.CLIENT",
            Description = "Client information",
            Image = "\ue3f3",
            Threshold = 30,
            Sparkies = [84, 16, 78, 23, 56, 44, 34, 67, 0, 1]
        });
        Sparklines.Add(new SparklineItem
        {
            Name = "API.ORDER",
            Description = "Placing orders for vouchers",
            Image = "\ue411",
            Threshold = 30,
            Sparkies = [84, 16, 78, 23, 56, 44, 34, 67, 0, 44]
        });        
        return Task.CompletedTask;
     }

}



