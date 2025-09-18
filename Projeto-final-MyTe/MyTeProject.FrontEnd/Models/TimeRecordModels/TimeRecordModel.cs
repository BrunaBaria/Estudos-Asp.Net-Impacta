using System.ComponentModel.DataAnnotations;

namespace MyTeProject.FrontEnd.Models.TimeRecordModels
{
    public class TimeRecordModel
    {
        public int Id { get; set; }

        [Required]
        [Range(typeof(DateTime), "2024-01-01", "2099-12-31")]
        public DateTime? Date { get; set; }

        //[Required]
        //[Range(0,24, ErrorMessage = "the indicated time must be between 1 and 24 hours")] - inseir no controller
        public double? AppointedTime { get; set; } = null;

        //[Required]
        public int? WBSId { get; set; }

        public int? FortnightId { get; set; }

        public bool? CanAppointToday { get; set; } = true;
    }
}
