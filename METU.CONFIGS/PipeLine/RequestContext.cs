using Microsoft.AspNetCore.Http;

namespace METU.CONFIGS.PipeLine
{
    public class RequestContext
    {
        public string RequesterName = "";

        public int Hour { get; set; }
    }
}
