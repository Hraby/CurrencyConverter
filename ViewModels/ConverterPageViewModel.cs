using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;

namespace UnitConverter.ViewModels;

public partial class ConverterPageViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _selectedCurrency;

    [ObservableProperty]
    private string _lowerSelectedCurrency;

    [ObservableProperty]
    private decimal _convertedValue;

    [ObservableProperty]
    private decimal _inputValue = 100;

    public ConverterPageViewModel()
    {
        Currencies = new ObservableCollection<string> { "EUR", "USD", "CZK", "GBP", "HUF", "JPY", "PLN", "CHF", "IRR" };
        FetchExchangeRates();
        SelectedCurrency = Currencies[0];
    }
    
    partial void OnInputValueChanged(decimal value)
    {
        CalculateConvertedValue();
    }
    public ObservableCollection<string> Currencies { get; }
    public ObservableCollection<ExchangeRate> ExchangeRates { get; private set; } = new ObservableCollection<ExchangeRate>();

    public ObservableCollection<string> LowerCurrencies => new ObservableCollection<string>(Currencies.Where(c => c != SelectedCurrency));

    public string FormattedSelectedCurrency => SelectedCurrency switch
    {
        "EUR" => "Euro",
        "USD" => "Americký dolar",
        "CZK" => "Česká koruna",
        "GBP" => "Britská libra",
        "HUF" => "Maďarský forint",
        "JPY" => "Japonský jen",
        "PLN" => "Polský zlotý",
        "CHF" => "Švýcarský frank",
        "IRR" => "Iránský riál",
        _ => SelectedCurrency
    };

    public string FormattedLowerSelectedCurrency => LowerSelectedCurrency switch
    {
        "EUR" => "Euro",
        "USD" => "Americký dolar",
        "CZK" => "Česká koruna",
        "GBP" => "Britská libra",
        "HUF" => "Maďarský forint",
        "JPY" => "Japonský jen",
        "PLN" => "Polský zlotý",
        "CHF" => "Švýcarský frank",
        "IRR" => "Iránský riál",
        _ => LowerSelectedCurrency
    };

    partial void OnSelectedCurrencyChanged(string value)
    {
        OnPropertyChanged(nameof(FormattedSelectedCurrency));
        OnPropertyChanged(nameof(LowerCurrencies));
        LowerSelectedCurrency = LowerCurrencies.FirstOrDefault();
        CalculateConvertedValue();
    }

    partial void OnLowerSelectedCurrencyChanged(string value)
    {
        OnPropertyChanged(nameof(FormattedLowerSelectedCurrency));
        CalculateConvertedValue();
    }

    private async void FetchExchangeRates()
    {
        using var client = new HttpClient();
        var response = await client.GetFromJsonAsync<ExchangeRateApiResponse>("https://open.er-api.com/v6/latest/USD");

        if (response != null && response.Result == "success")
        {
            foreach (var rate in response.Rates)
            {
                ExchangeRates.Add(new ExchangeRate { Currency = rate.Key, Rate = rate.Value });
            }
            CalculateConvertedValue();
        }
    }

    private void CalculateConvertedValue()
    {
        var selectedRate = ExchangeRates.FirstOrDefault(r => r.Currency == SelectedCurrency)?.Rate ?? 1m;
        var lowerRate = ExchangeRates.FirstOrDefault(r => r.Currency == LowerSelectedCurrency)?.Rate ?? 1m;

        if (selectedRate != 0)
        {
            ConvertedValue = _inputValue * (lowerRate / selectedRate);
        }
    }
}

    public class ExchangeRate
    {
        public string Currency { get; set; }
        public decimal Rate { get; set; }
    }

    public class ExchangeRateApiResponse
    {
        public string Result { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }