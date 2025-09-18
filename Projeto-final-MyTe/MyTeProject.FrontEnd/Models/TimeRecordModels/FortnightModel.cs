using System.ComponentModel.DataAnnotations;

namespace MyTeProject.FrontEnd.Models.TimeRecordModels
{
    public class FortnightModel
    {
        public int Id { get; set; }

        [Required]
        public IList<TimeRecordModel>? TimeRecords { get; set; } = [];
    }
}
