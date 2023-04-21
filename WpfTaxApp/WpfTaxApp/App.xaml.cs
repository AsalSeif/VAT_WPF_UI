using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CommonException;

namespace WpfTaxApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (e.Exception is BusinessException)
            {
                MessageBox.Show($"ErrorCode:{ (e.Exception as BusinessException).BussinesExceptionCode.InternalValue}{Environment.NewLine}{ e.Exception.Message}", "Business Exception ", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            else
                MessageBox.Show("An unhandled exception just occurred: " + e.Exception.Message, "Exception ", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}
