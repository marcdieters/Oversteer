using Microsoft.AspNetCore.Components;
using Oversteer.Models.Racing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.BlazorComponents.Championship
{
    public partial class Scoring
    {
        [Parameter]
        public Models.Racing.Championship Championship { get; set; } = new Models.Racing.Championship();

        protected override void OnParametersSet()
        {
            if (Championship.Points.Count == 0)
            {
                Point point = new Point();
                point.Position = 1;
                Championship.Points.Add(point);
            }
        }

        protected void AddPoints()
        {
            int lastPosition = 1;
            if (Championship.Points.Count > 0)
            {
                lastPosition = Championship.Points.Last().Position + 1;
            }

            Point point = new Point();
            point.Position = lastPosition;
            Championship.Points.Add(point);
            Championship.Points = Championship.Points.OrderBy(point => point.Position).ToList();
        }

        protected void RemovePoint(Guid id)
        {
            if (Championship.Points.Any(c => c.Id == id))
            {
                var pointToRemove = Championship.Points.First(c => c.Id == id);
                Championship.Points.Remove(pointToRemove);
                StateHasChanged();
            }
        }
    }
}
