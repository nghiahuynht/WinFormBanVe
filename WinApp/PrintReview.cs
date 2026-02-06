using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinApp
{
    public partial class PrintReview : Form
    {
        private Int64 orderId;
        string billPdfExportPath = ConfigurationManager.AppSettings["BillExportPath"];
        public PrintReview(Int64 orderId)
        {
            InitializeComponent();
            this.orderId = orderId;

        }

        private async void PrintReview_Load(object sender, EventArgs e)
        {
            await webView21.EnsureCoreWebView2Async(null);
            string outputPath = Path.Combine(billPdfExportPath, orderId.ToString() + ".pdf");
            if (File.Exists(outputPath))
            {
                string urlPath = $"file:///{outputPath.Replace('\\', '/')}";

                webView21.CoreWebView2.Navigate(urlPath);
                // Phương pháp B: In lặng (Silent Printing) ra máy in mặc định
                //
            }
        }

        private void webView21_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                //webView21.CoreWebView2.ShowPrintUI();
                webView21.CoreWebView2.PrintAsync(null);
            }
        }
    }
}
