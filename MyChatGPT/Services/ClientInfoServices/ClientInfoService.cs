using System;
using System.Threading.Tasks;
using MyChatGPT.ServiceModels;
using Newtonsoft.Json;

namespace MyChatGPT.Services.ClientInfoServices
{
    public class ClientInfoService : IClientInfoService
    {
        public ClientInfoService()
        {
        }

        public async Task<ImageResponseModel> GetImages(string jSONData)
        {
            ImageResponseModel imageResponseModel = null;
            ServiceHelper serviceHelper = new ServiceHelper();
            string baseUrl = "https://api.openai.com/v1/";
            string url = "images/generations";

            var response = await serviceHelper.PostDataAsync(baseUrl, url, jSONData);

            if(response != null && JsonValidationHelper.IsValidJson(response))
            {
                imageResponseModel = JsonConvert.DeserializeObject<ImageResponseModel>(response,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.None,
                        NullValueHandling = NullValueHandling.Ignore
                    });
            }
            return imageResponseModel;
        }
    }
}
