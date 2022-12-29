using System;
using System.Collections.Generic;

namespace MyChatGPT.ServiceModels
{
    public class Datum
    {
        public string url { get; set; }
    }

    public class ImageResponseModel
    {
        public int created { get; set; }
        public List<Datum> data { get; set; }
    }
}

