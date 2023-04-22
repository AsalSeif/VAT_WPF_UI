using CommonException;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfTaxApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly string baseApiUrl = "https://localhost:5001/api/Home/";
        public MainWindow()
        {
            InitializeComponent();
            FillCountryCombobox();
        }
      
        private async void FillCountryCombobox()
        {
            var countries = await GetAllCoutriesAsync();
            CountryComboBox.ItemsSource = countries;
            CountryComboBox.DisplayMemberPath = "Name";
            CountryComboBox.SelectedValuePath = "Value";

        }

        private async Task<List<NameValue>> GetAllCoutriesAsync()
        {
            using (HttpClient client = new HttpClient())
            {

                var response = await client.GetAsync(baseApiUrl + "Countries/all");
                if (response.IsSuccessStatusCode)
                {
                    var taxRateString = response.Content.ReadAsAsync<List<NameValue>>();
                    return taxRateString.Result;
                }
                else
                {
                    var errorMessage = response.Content.ReadAsStringAsync().Result;
                    throw new BusinessException("ServerSide Errors:" + errorMessage, UIBusinessExceptionEnum.ServerSideErrors);

                }
                return new List<NameValue>();
            }
        }


        private async Task<List<double>> GetTaxRatesByCoutryIdAsync(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(baseApiUrl + $"TaxRates/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var taxRateString = response.Content.ReadAsAsync<List<double>>();
                    return taxRateString.Result;
                }
                else
                {
                    var errorMessage = response.Content.ReadAsStringAsync().Result;
                    throw new BusinessException("ServerSide Errors:" + errorMessage, UIBusinessExceptionEnum.ServerSideErrors);

                }
                return new List<double>();
            }
        }

        private void Radiobutton_Checked(object sender, RoutedEventArgs e)
        {
            switch ((sender as RadioButton).Name)
            {
                case "lb_Gross":
                    SetTextBoxReadOnlyStyle(tb_GrossVlaue, false);
                    SetTextBoxReadOnlyStyle(tb_NetValue, true);
                    SetTextBoxReadOnlyStyle(tb_VATValue, true);

                    break;
                case "lb_VAT":
                    SetTextBoxReadOnlyStyle(tb_GrossVlaue, true);
                    SetTextBoxReadOnlyStyle(tb_NetValue, true);
                    SetTextBoxReadOnlyStyle(tb_VATValue, false);
                    break;
                case "lb_Net":
                    SetTextBoxReadOnlyStyle(tb_GrossVlaue, true);
                    SetTextBoxReadOnlyStyle(tb_NetValue, false);
                    SetTextBoxReadOnlyStyle(tb_VATValue, true);
                    break;
            }
        }

        private void SetTextBoxReadOnlyStyle(TextBox textBox, bool readOnly)
        {
            textBox.IsReadOnly = readOnly;
            textBox.Background = readOnly ? Brushes.Beige : Brushes.White;
        }



        private void Tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CalculateVATValues();
            }
        }

        private async void CalculateVATValues()
        {
            var result = new TaxResult();
            var taxRate = GetSelectedTaxRates();
            if (lb_Gross.IsChecked == true)
            {
                result = await CalculateTaxBaseOnGross(taxRate);
            }
            else if (lb_Net.IsChecked == true)
            {
                result = await CalculateTaxBaseOnNet(taxRate);
            }
            else if (lb_VAT.IsChecked == true)
            {
                result = await CalculateTaxBaseOnVAT(taxRate);
            }
            else
            {
                throw new Exception("Please select type of calculation");
            }
            FillVATResultAfterCalculation(result);
        }

        int selectedCountryId;
        #region calculation methods
        private async Task<TaxResult> CalculateTaxBaseOnGross(double taxRate)
        {

            if (double.TryParse(tb_GrossVlaue.Text, out double grossValue))
            {
                var urlString = CreateUrlAsString("TaxBaseOnGross/{0}/{1}/{2}", taxRate, grossValue);
                return await ConsumeApiMethod(urlString);

            }
            else
            {
                throw new BusinessException("Please enter valid value for price with Tax", UIBusinessExceptionEnum.CalculationBaseValueIsNotValid);
            }

        }
        private async Task<TaxResult> CalculateTaxBaseOnNet(double taxRate)
        {

            if (double.TryParse(tb_NetValue.Text, out double netValue))
            {
                var urlString = CreateUrlAsString("TaxBaseOnNet/{0}/{1}/{2}", taxRate, netValue);
                return await ConsumeApiMethod(urlString);

            }
            else
            {
                throw new BusinessException("Please enter valid value for price without Tax", UIBusinessExceptionEnum.CalculationBaseValueIsNotValid);
            }

        }
        private async Task<TaxResult> CalculateTaxBaseOnVAT(double taxRate)
        {

            if (double.TryParse(tb_VATValue.Text, out double vatValue))
            {
                var urlString = CreateUrlAsString("TaxBaseOnVAT/{0}/{1}/{2}", taxRate, vatValue);
                return await ConsumeApiMethod(urlString);

            }
            else
            {
                throw new BusinessException("Please enter valid value for Value-Added Tax", UIBusinessExceptionEnum.CalculationBaseValueIsNotValid);
            }

        }
        private static async Task<TaxResult> ConsumeApiMethod(string urlString)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(urlString);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TaxResult>(responseBody);
                }
                else
                {
                    var errorMessage = response.Content.ReadAsStringAsync().Result;
                    throw new BusinessException("ServerSide Errors:" + errorMessage, UIBusinessExceptionEnum.ServerSideErrors);
                }
                return new TaxResult();
            }
        }


        private string CreateUrlAsString(string methodRoute, double taxRate, double value)
        {

            string methodUrl = string.Format(methodRoute,
                                             selectedCountryId,
                                             taxRate,
                                             value);
            return baseApiUrl + methodUrl;
        }
        #endregion
        private void FillVATResultAfterCalculation(TaxResult result)
        {
            tb_NetValue.Text = result.Net.ToString();
            tb_VATValue.Text = result.VAT.ToString();
            tb_GrossVlaue.Text = result.Gross.ToString();

        }

        private double GetSelectedTaxRates()
        {
            var selectedTaxRate_rb = TaxRates.Children
                .Cast<RadioButton>()
                .ToList()
                .SingleOrDefault(t => t.IsChecked == (bool?)true);
            if (selectedTaxRate_rb is null)
                throw new BusinessException("Please select a VAT Rate. ( If you Can not see VATRates, first select your country.)", UIBusinessExceptionEnum.TaxRangeIsNotSpecified);
            if (double.TryParse(selectedTaxRate_rb.Content.ToString().TrimEnd('%', ' ', '\n', '\r'), out double taxRate))
                return taxRate;
            throw new BusinessException("VAT Rate is not valid", UIBusinessExceptionEnum.TaxRateIsNotValid);

        }




        private async void CountryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedCountryId = (int)(sender as ComboBox).SelectedValue;
            var texRateValues = await GetTaxRatesByCoutryIdAsync(selectedCountryId);
            TaxRates.Children.Clear();
            foreach (var item in texRateValues)
            {
                RadioButton rb = new RadioButton() { Content = item.ToString() + "% ", };
                TaxRates.Children.Add(rb);

            }
        }
    }

    internal class NameValue
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
