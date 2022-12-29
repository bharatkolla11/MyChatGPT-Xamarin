using System;
using System.Threading.Tasks;
using MyChatGPT.ServiceModels;

namespace MyChatGPT.Services.ClientInfoServices
{
    public interface IClientInfoService
    {
        Task<ImageResponseModel> GetImages(string jSONData);
    }
}
