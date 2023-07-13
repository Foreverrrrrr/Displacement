using Displacement.Views;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Windows;

namespace Displacement
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt/QHNqVVhkW1pFdEBBXHxAd1p/VWJYdVt5flBPcDwsT3RfQF9iSH5WdkdmX3pbeXNQRg==;Mgo+DSMBPh8sVXJ0S0V+XE9AcVRDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS3xSd0VmWX1bdnFQT2hbUQ==;ORg4AjUWIQA/Gnt2VVhjQlFaclhJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxRd0RjXH9cdXJQQGBcUEU=;NzU0NjcyQDMyMzAyZTMzMmUzMEoyaGFwdWZIZHBGSnlKdFUwdjkyL3RkRTBhclplcXdvUXJqdVBER08xY2s9;NzU0NjczQDMyMzAyZTMzMmUzMERWTGVId1pzRnZIV25Eck9vbi9GdkNBdW4wRnY3Q3ZpaVpoL3FwbjFXSXM9;NRAiBiAaIQQuGjN/V0Z+X09EaFtFVmJLYVB3WmpQdldgdVRMZVVbQX9PIiBoS35RdERiWXpecnFTQ2ZcV0Jy;NzU0Njc1QDMyMzAyZTMzMmUzME9pa2FFRzl0OUZ3Y1lkQkU1M1Q4bGRCaUdjdkdGWGpSbDl6RUZubUk1am89;NzU0Njc2QDMyMzAyZTMzMmUzMEY2SkJ6Vnc2QmdURFdjNWhxWmlSNmNnbGF3M3dmUVJYbkdvZ1ZCVkhuMHc9;Mgo+DSMBMAY9C3t2VVhjQlFaclhJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxRd0RjXH9cdXJQQGFfVUA=;NzU0Njc4QDMyMzAyZTMzMmUzMFB4YURNR1JIM3drVytLTGxpa2dZNlZiUWNJVVpQdEtIL2RBVHRsbk9BMGM9;NzU0Njc5QDMyMzAyZTMzMmUzMGprQWszOE1Mc29FRFdENEVBNGkwV2VtUWEvM3MxQ09iUFhXNjlaOCt6UXM9;NzU0NjgwQDMyMzAyZTMzMmUzME9pa2FFRzl0OUZ3Y1lkQkU1M1Q4bGRCaUdjdkdGWGpSbDl6RUZubUk1am89");
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            //授权示例
            if (!HslCommunication.Authorization.SetAuthorizationCode("b10ac26c-cf47-4432-b124-d7ce47f1170a"))
            {
                MessageBox.Show("授权失败！当前程序只能使用24小时！");
                return;
            }
            else
            {
                // active success
                //MessageBox.Show("授权成功！");
            }
        }
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            System.IO.File.WriteAllText("hsl.txt", HslCommunication.BasicFramework.SoftBasic.GetExceptionMessage(ex));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<LogVision>();
            containerRegistry.RegisterDialog<HomeVision>();
            containerRegistry.RegisterDialog<SetPage>();
            containerRegistry.RegisterForNavigation<SqlServerVision>();
        }
    }
}
