using Microsoft.AspNetCore.Components;
using Oversteer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.BlazorComponents.Result
{
    public partial class Upload
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        protected IRaceService RaceService { get; set; }

        [Parameter]
        public Guid RaceId { get; set; }
        [Parameter]
        public EventCallback<Guid> RaceIdChanged { get; set; }
        [Parameter]
        public string Token { get; set; } = string.Empty;

        protected Models.Racing.Race Race { get; set; } = new Models.Racing.Race();
        protected List<Models.Result.ResultDto> ResultDtos { get; set; } = new List<Models.Result.ResultDto>();

        protected override async void OnParametersSet()
        {
            if (!string.IsNullOrEmpty(Token))
            {
                Globals.Token = Token;
                RaceService = new RaceService(NavigationManager!.BaseUri, Token);
                Race = await RaceService.GetRace(RaceId);

                if (ResultDtos.Count == 0)
                {
                    Models.Result.ResultDto result = new Models.Result.ResultDto();
                    ResultDtos.Add(result);
                }

                StateHasChanged();
            }
        }

        protected void AddResult()
        {
            Models.Result.ResultDto result = new Models.Result.ResultDto();
            ResultDtos.Add(result);
        }

        protected void RemoveUpload(Guid id)
        {
            if (ResultDtos.Any(r => r.Id == id))
            {
                var resultDto = ResultDtos.First(r => r.Id == id);
                ResultDtos.Remove(resultDto);
            }
        }

        protected async void HandleValidSubmit()
        {
            foreach(var resultDto in ResultDtos)
            {
                await Helpers.Api.Post(NavigationManager.BaseUri, $"api/upload/acc/{RaceId}", resultDto, Globals.Token);
            }

            await RaceIdChanged.InvokeAsync(Race.Id);
        }
    }
}
