using Microsoft.AspNetCore.Components;
using Oversteer.Models.Racing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.BlazorComponents.Session
{
    public partial class Sessions
    {
        [Parameter]
        public List<Models.Racing.Session> SessionsSet { get; set; } = new List<Models.Racing.Session>();

        protected override void OnParametersSet()
        {
            if (SessionsSet.Count == 0)
            {
                AddSession();
            }
        }

        protected async void AddSession()
        {
            Models.Racing.Session session = new Models.Racing.Session();
            SessionsSet.Add(session);
        }

        protected async void RemoveSession(Guid id) 
        {
            if (SessionsSet.Any(s => s.Id == id))
            {
                Models.Racing.Session session = SessionsSet.First(s => s.Id == id);
                SessionsSet.Remove(session);
            }
        }
    }
}
