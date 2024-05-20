
namespace DataAccessLayer.DTO.Lookup
{
    public class GetMobileVersionOutput
    {
        public int? ShowLater { get; set; }
        public int? ShowIgnore { get; set; }
        public decimal? MinAppVersion { get; set; }
        public int? DurationUntilAlertAgaint { get; set; }
    }
}
