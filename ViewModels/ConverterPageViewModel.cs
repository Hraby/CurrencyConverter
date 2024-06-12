using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace UnitConverter.ViewModels;

public partial class ConverterPageViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _selectedCurrency;

    public ConverterPageViewModel()
    {
        Currencies = new ObservableCollection<string> { "EUR", "USD", "CZK" };
        SelectedCurrency = Currencies[0];
    }

    public ObservableCollection<string> Currencies { get; }

    public string FormattedSelectedCurrency => SelectedCurrency switch
    {
        "EUR" => "Euro",
        "USD" => "Americký dolar",
        "CZK" => "Česká koruna",
        _ => SelectedCurrency
    };

    partial void OnSelectedCurrencyChanged(string value)
    {
        OnPropertyChanged(nameof(FormattedSelectedCurrency));
    }
}