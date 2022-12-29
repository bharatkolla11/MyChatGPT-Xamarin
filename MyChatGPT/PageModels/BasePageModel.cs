using System;
using FreshMvvm;
using MyChatGPT.Services.ClientInfoServices;

namespace MyChatGPT.PageModels
{
    public class BasePageModel : FreshBasePageModel
    {
        private readonly IClientInfoService _clientInfoService;

        string _pageTitle = string.Empty;
        public string PageTitle
        {
            get { return _pageTitle; }
            set
            {
                _pageTitle = value;
                RaisePropertyChanged("PageTitle");
            }
        }

        public BasePageModel(IClientInfoService clientInfoService)
        {
            _clientInfoService = clientInfoService;
        }
    }
}
