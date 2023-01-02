using System;
using FreshMvvm;
using MyChatGPT.PageModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MyChatGPT.Services.ClientInfoServices;
using MyChatGPT.Styles;
using System.Collections.Generic;
using Xamarin.Essentials;

namespace MyChatGPT
{
    public partial class App : Application
    {
        const int smallWightResolution = 768;
        const int smallHeightResolution = 1280;

        public App()
        {
            InitializeComponent();

            LoadStyles();

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

        void LoadStyles()
        {
            if (IsASmallDevice())
            {
                MyChatGPTdictionary.MergedDictionaries.Add(SmallDeviceStyles.SharedInstance);
            }
            else
            {
                MyChatGPTdictionary.MergedDictionaries.Add(GeneralDeviceStyles.SharedInstance);
            }
        }

        public static bool IsASmallDevice()
        {
            // Get Metrics
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

            // Width (in pixels)
            var width = mainDisplayInfo.Width;

            // Height (in pixels)
            var height = mainDisplayInfo.Height;
            return (width <= smallWightResolution && height <= smallHeightResolution);
        }
    }
}
