using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    [Keyless]
    public class MobileVersion
    {
        public int? ShowLater { get; set; }
        public int? ShowIgnore { get; set; }
        public int? MinAppVersion { get; set; }
        public int? DurationUntilAlertAgaint { get; set; }
    }
}
