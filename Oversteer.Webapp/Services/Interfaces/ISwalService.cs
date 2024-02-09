using CurrieTechnologies.Razor.SweetAlert2;
using System.Threading.Tasks;

namespace Oversteer.Webapp.Services
{
    public interface ISwalService
    {
        Task ShowInfo(string title, string text);
        Task<SweetAlertResult> ShowInfoWithConfirm(string title, string text);
        Task ShowError(string text);
        Task<SweetAlertResult> ShowInfoWithConfirmOk(string title, string text);
    }
}
