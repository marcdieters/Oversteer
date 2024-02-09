using Microsoft.AspNetCore.Authorization;

namespace Oversteer.Webapp.Pages.Admin
{
    [Authorize(Roles = "Admins")]
    public partial class Index
    {

    }
}
