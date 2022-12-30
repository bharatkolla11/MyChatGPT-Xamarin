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

        //TextViewSource
        ObservableCollection<QACollection> _textViewSource { get; set; }
        public ObservableCollection<QACollection> TextViewSource
        {
            get { return _textViewSource; }
            set
            {
                _textViewSource = value;
                RaisePropertyChanged("TextViewSource");
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

        //TextCollectionVisible
        bool _textCollectionVisible = false;
        public bool TextCollectionVisible
        {
            get { return _textCollectionVisible; }
            set
            {
                _textCollectionVisible = value;
                RaisePropertyChanged("TextCollectionVisible");
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
            try
            {
                if (string.IsNullOrEmpty(UserEnteredText))
                {
                    return;
                }

                if (ImageCollectionVisible)
                {
                    UserDialogs.Instance.ShowLoading("Getting Images");

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
                        foreach (var imageData in response.data)
                        {
                            ImageViewSource.Add(new AIImageCollection { AIImageSource = imageData.url });
                        }
                    }

                    UserDialogs.Instance.HideLoading();
                }
                else
                {
                    UserDialogs.Instance.ShowLoading("Getting Text");

                    Dictionary<string, object> postData = new Dictionary<string, object>();
                    postData.Add("prompt", UserEnteredText);
                    postData.Add("model", "text-davinci-003");
                    postData.Add("max_tokens", 2048);


                    string postDataString = JsonConvert.SerializeObject(postData);

                    var response = await _clientInfoService.GetText(postDataString);

                    if (response != null && response.choices.Count > 0)
                    {
                        foreach (var textData in response.choices)
                        {
                            TextViewSource.Add(new QACollection { Question = "Question: " + UserEnteredText, Answer = "Answer: " + textData.text });
                        }
                    }
                    UserDialogs.Instance.HideLoading();
                }

                UserEnteredText = string.Empty;
            }
            catch(Exception ex)
            {
                await CoreMethods.DisplayAlert("Error", "DisplayData Exception: " + ex.Message, "OK");
            }
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

            if(param.Equals("Text", StringComparison.OrdinalIgnoreCase))
            {
                TextCollectionVisible = true;
                TextViewSource = new ObservableCollection<QACollection>();
            }

        }

        public override void ReverseInit(object returnedData)
        {
            base.ReverseInit(returnedData);
        }
    }
}

