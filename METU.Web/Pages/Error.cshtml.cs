using Microsoft.AspNetCore.Mvc;
using METU.BasicCore.Controllers;
using System.Diagnostics;

namespace METU.Admin.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : MetuPageBase
    {
        public string Requestid { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(Requestid);

        private readonly ILogger<ErrorModel> _logger;

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            Requestid = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}
