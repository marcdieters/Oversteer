using CurrieTechnologies.Razor.SweetAlert2;
using System.Threading.Tasks;

namespace Oversteer.Webapp.Services
{
    public class SwalService : ISwalService
    {
        SweetAlertService _sweetAlertService;

        public SwalService(SweetAlertService sweetAlertService)
        {
            _sweetAlertService = sweetAlertService;
        }

        public async Task ShowError(string text)
        {
            await _sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Error",
                Text = text,
                ShowConfirmButton = true,
                Icon = SweetAlertIcon.Error,
                ShowDenyButton = false,
                ConfirmButtonText = "Ok"
            });
        }

        public async Task ShowInfo(string title, string text)
        {
            await _sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = title,
                Text = text,
                Icon = SweetAlertIcon.Info,
                ShowConfirmButton = true,
                ShowDenyButton = false,
                ConfirmButtonText = "Ok"
            });
        }

        public async Task<SweetAlertResult> ShowInfoWithConfirm(string title, string text)
        {
            var confirm = await _sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = title,
                Text = text,
                Icon = SweetAlertIcon.Question,
                ShowConfirmButton = true,
                ShowDenyButton = true,
                ConfirmButtonText = "Yes",
                DenyButtonText = "No"
            });

            return confirm;
        }

        public async Task<SweetAlertResult> ShowInfoWithConfirmOk(string title, string text)
        {
            var confirm = await _sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = title,
                Text = text,
                Icon = SweetAlertIcon.Info,
                ShowConfirmButton = true,
                ShowDenyButton = false,
                ConfirmButtonText = "Ok"
            });

            return confirm;
        }
    }
}
