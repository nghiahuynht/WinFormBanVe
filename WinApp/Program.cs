using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Windows.Forms;
using GM_DAL;
using GM_DAL.IServices;
using GM_DAL.Services;
using GM_DAL.Models.User;
using Newtonsoft.Json;

namespace WinApp
{
    internal static class Program
    {
        private static IServiceProvider? ServiceProvider { get; set; }

        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            ApplicationConfiguration.Initialize();
            ConfigureServices();

            var authenExists = AuthenInfo();
            /* if (authenExists == null)
             {
                 var mainForm = ServiceProvider!.GetRequiredService<LoginForm>();
                 Application.Run(mainForm);
             }
             else
             {
                 var mainForm = ServiceProvider!.GetRequiredService<FormBanVe>();
                 Application.Run(mainForm);
             }*/

            if (authenExists == null)
            {

              var mainForm = ServiceProvider!.GetRequiredService<LoginForm>();
                var mainForm = ServiceProvider!.GetRequiredService<FormBanVe>();

                

                Application.Run(mainForm);
            }
            else
            {
              //  var mainForm = ServiceProvider!.GetRequiredService<FormBanVe>();
                var mainForm = ServiceProvider!.GetRequiredService<FormTest>();
                Application.Run(mainForm);
            }
        }

        private static void ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddTransient<SQLAdoContext>();

            services.AddSingleton<IUserInfoService, UserInfoService>();
            services.AddSingleton<IProductService, ProductService>();


            services.AddSingleton<ICustomerVIPService, CustomerVIPService>();
            services.AddTransient<FormBanVe>();
            services.AddTransient<LoginForm>();
            services.AddTransient<FormTest>();
            services.AddTransient<PrintReview>();
            services.AddTransient<change_uspass>();

            ServiceProvider = services.BuildServiceProvider();
        }

        private static AuthenSuccessModel AuthenInfo()
        {
            string loginFile = ConfigurationManager.AppSettings["LoginFile"];
            AuthenSuccessModel userObject = null;

            if (File.Exists(loginFile))
            {
                using (StreamReader readtext = new StreamReader(loginFile))
                {
                    string result = readtext.ReadLine();
                    if (!string.IsNullOrEmpty(result))
                    {
                        userObject = JsonConvert.DeserializeObject<AuthenSuccessModel>(result);
                    }
                }
            }

            return userObject;
        }
    }
}
