using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using MyChatGPT.Models;
using MyChatGPT.Services.ClientInfoServices;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace MyChatGPT.PageModels
{
	public class DataPageModel : BasePageModel
	{
        private readonly IClientInfoService _clientInfoService;

        //UserEnteredText
        string _userEnteredText = string.Empty;
        public string UserEnteredText
        {
            get { return _userEnteredText; }
            set
            {
                _userEnteredText = value;
                RaisePropertyChanged("UserEnteredText");
            }
        }

        //ImageViewSource
       ObservableCollection<AIImageCollection> _imageViewSource { get; set; }
       public ObservableCollection<AIImageCollection> ImageViewSource
        {
            get { return _imageViewSource; }
            set
            {
                _imageViewSource = value;
                RaisePropertyChanged("ImageViewSource");
            }
        }

        //ImageCollectionVisible
        bool _imageCollectionVisible = false;
        public bool ImageCollectionVisible
        {
            get { return _imageCollectionVisible; }
            set
            {
                _imageCollectionVisible = value;
                RaisePropertyChanged("ImageCollectionVisible");
            }
        }

        #region #Commands#

        public ICommand SubmitCommand { get; }

        #endregion
        public DataPageModel(IClientInfoService clientInfoService) : base(clientInfoService)
        {
			_clientInfoService = clientInfoService;
            SubmitCommand = new Command(async () => await DisplayData());
        }

        private async Task DisplayData()
        {
            UserDialogs.Instance.ShowLoading("Getting Images");

            if (string.IsNullOrEmpty(UserEnteredText))
            {
                return;
            }

            if (ImageViewSource.Count > 0)
            {
                ImageViewSource.Clear();
            }

        

            Dictionary<string, object> postData = new Dictionary<string, object>();
            postData.Add("prompt", UserEnteredText);
            postData.Add("n", 5);

            string postDataString = JsonConvert.SerializeObject(postData);

            var response = await _clientInfoService.GetImages(postDataString);

            if (response != null && response.data.Count > 0)
            {
                foreach(var imageData in response.data)
                {
                    ImageViewSource.Add(new AIImageCollection { AIImageSource = imageData.url });
                }
            }

            UserDialogs.Instance.HideLoading();

        }

        public override void Init(object initData)
        {
            base.Init(initData);
            string param = initData as string;

            if(param.Equals("Image", StringComparison.OrdinalIgnoreCase))
            {
                ImageCollectionVisible = true;
                ImageViewSource = new ObservableCollection<AIImageCollection>();
            }

        }

        public override void ReverseInit(object returnedData)
        {
            base.ReverseInit(returnedData);
        }
    }
}

