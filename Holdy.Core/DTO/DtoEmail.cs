using System.ComponentModel.DataAnnotations;

namespace Holdy.Holdy.Core.DTO
{
    public class DtoReportBugRequest
    {
        [Required] public string Message { get; set; } = "";
    }
}