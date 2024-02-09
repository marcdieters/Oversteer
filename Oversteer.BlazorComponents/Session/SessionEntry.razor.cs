using Microsoft.AspNetCore.Components;
using Oversteer.Models.Racing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.BlazorComponents.Session
{
    public partial class SessionEntry
    {
        [Parameter]
        public Models.Racing.Session Session { get; set; } = new Models.Racing.Session();
    }
}
