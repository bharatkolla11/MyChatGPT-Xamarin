using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MyChatGPT.Styles
{
    public partial class GeneralDeviceStyles : ResourceDictionary
    {
        public static GeneralDeviceStyles SharedInstance { get; } = new GeneralDeviceStyles();

        public GeneralDeviceStyles()
        {
            InitializeComponent();
        }
    }
}

