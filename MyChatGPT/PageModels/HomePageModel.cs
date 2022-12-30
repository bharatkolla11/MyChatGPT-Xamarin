using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MyChatGPT.Services.ClientInfoServices;
using Xamarin.Forms;

namespace MyChatGPT.PageModels
{
    public class HomePageModel : BasePageModel
    {
        private readonly IClientInfoService _clientInfoService;


        #region #Commands#
        public ICommand SelectMyGPTCommand { get; }

        #endregion

        public HomePageModel(IClientInfoService clientInfoService) : base(clientInfoService)
        {
            _clientInfoService = clientInfoService;

            if(DateTime.Now.Hour < 12)
            {
                PageTitle = "Good Morning";
            }
            else if(DateTime.Now.Hour > 12 && DateTime.Now.Hour < 17)
            {
                PageTitle = "Good Afternoon";
            }
            else
            {
                PageTitle = "Good Evening";
            }

            SelectMyGPTCommand = new Command(async (obj) => await NavigateToDataPage(obj));
        }

        public override void Init(object initData)
        {
            base.Init(initData);
        }

        public override void ReverseInit(object returnedData)
        {
            base.ReverseInit(returnedData);
        }


        private async Task NavigateToDataPage(object obj)
        {
            await CoreMethods.PushPageModel<DataPageModel>(obj);
        }
    }
}
