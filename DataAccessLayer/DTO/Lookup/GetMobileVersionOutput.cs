
namespace DataAccessLayer.DTO.Lookup
{
    public class GetMobileVersionOutput
    {
        public bool? ShowLater { get; set; }
        public int? ShowIgnore { get; set; }
        public int? MinAppVersion { get; set; }
        public int? DurationUntilAlertAgaint { get; set; }
    }
}
