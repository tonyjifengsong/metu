using METU.BasicCore.Controllers;

namespace METU.Admin.Pages
{
    public class PrivacyModel : MetuPageBase
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
