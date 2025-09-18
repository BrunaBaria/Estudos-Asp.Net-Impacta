using System.ComponentModel.DataAnnotations;

namespace MyTeProject.FrontEnd.Models.WBSModels
{
    public class WBSModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "The lenght of a WBS must be between 4 and 10 characters")]
        [RegularExpression(@"^[a-zA-Z0-9]+$")]
        public string ChargeCode { get; set; }

        [Required]
        public string Description { get; set; }

        public string? WBSType { get; set; }

        [Required]
        public int? WBSTypeId { get; set; }

        public bool? UserWBSActive { get; set; }

        public int? QuantityOfTimeRecords { get; set; }
    }
}
