using System;
using FreshMvvm;
using MyChatGPT.PageModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyChatGPT.Services.ClientInfoServices;

namespace MyChatGPT
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            FreshIOC.Container.Register<IClientInfoService, ClientInfoService>();
            var mainPage = FreshPageModelResolver.ResolvePageModel<HomePageModel>();
            MainPage = new FreshNavigationContainer(mainPage);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
