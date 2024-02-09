using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using Oversteer.Enums;
using Oversteer.Models.Racing;
using Oversteer.Models.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.BlazorComponents.Result
{
    public partial class UploadEntry
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Parameter]
        public ResultDto ResultDto { get; set; } = new ResultDto();
        [Parameter]
        public Models.Racing.Race Race { get; set; } = new Models.Racing.Race();

        public Guid InputFileId { get; set; } = Guid.NewGuid();

        private async void LoadFiles(InputFileChangeEventArgs e)
        {
            long maxFileSize = 1024 * 1000;  // Maximum 10k file size

            foreach (var file in e.GetMultipleFiles(1))
            {
                try
                {
                    if (file.ContentType == "application/json")
                    {
                        var reader = await new StreamReader(file.OpenReadStream(maxFileSize), Encoding.Unicode).ReadToEndAsync();
                        ACCResult aCCResult = JsonConvert.DeserializeObject<ACCResult>(reader);

                        if (file.Name.ToUpper().Contains("FP"))
                        {
                            ResultDto.SessionType = SessionType.Practice;
                        }
                        else if (file.Name.ToUpper().Contains("Q"))
                        {
                            ResultDto.SessionType = SessionType.Qualifying;
                        }
                        else if (file.Name.ToUpper().Contains("R"))
                        {
                            ResultDto.SessionType = SessionType.Race;
                        }
                        else
                        {
                            throw new Exception("Unknown entry");
                        }

                        string[] fileNameSplit = file.Name.Split('_');
                        string filedateCheck = $"{Race.StartTime.ToString("yy")}{Race.StartTime.ToString("MM")}{Race.StartTime.ToString("dd")}";
                        if (file.Name.Contains(filedateCheck))
                        {
                            ResultDto.RaceId = Race.Id;
                            ResultDto.FileName = file.Name;
                            ResultDto.ACCResult = aCCResult;
                        }
                        else
                        {

                        }

                        StateHasChanged();
                    }
                }
                catch (Exception ex)
                {
                    InputFileId = Guid.NewGuid();
                }
            }
        }
    }
}
