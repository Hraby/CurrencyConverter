using System;
using System.Collections.ObjectModel;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using System.IO;
using System.Reactive;
using CommunityToolkit.Mvvm.Input;

namespace UnitConverter.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private Bitmap _logo;

        [ObservableProperty]
        private bool _isPaneOpen = false;

        [ObservableProperty] 
        private ViewModelBase _currentPage = new HomePageViewModel();

        [ObservableProperty]
        private ListItemTemplate? _selectedListItem;

        partial void OnSelectedListItemChanged(ListItemTemplate? value)
        {
            if (value == null) return;
            var instance = Activator.CreateInstance(value.ModelType);
            if (instance == null) return;
            CurrentPage = (ViewModelBase)instance;
        }
        public ObservableCollection<ListItemTemplate> Items { get; set; } = new()
        {
            new ListItemTemplate(typeof(HomePageViewModel)),
            new ListItemTemplate(typeof(ConverterPageViewModel)),
        };
        
        [RelayCommand]
        public void TriggerPaneCommand()
        {
            IsPaneOpen = !IsPaneOpen;
        }
    }

    public class ListItemTemplate
    {
        public ListItemTemplate(Type type)
        {
            ModelType = type;
            Label = type.Name.Replace("PageViewModel", "");
        }
        
        public string Label { get; }
        public Type ModelType { get; }
        
    }
}