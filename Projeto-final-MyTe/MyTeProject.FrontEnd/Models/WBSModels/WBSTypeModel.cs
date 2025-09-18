using System.ComponentModel.DataAnnotations;

namespace MyTeProject.FrontEnd.Models.WBSModels
{
    public class WBSTypeModel
    {

        public int Id { get; set; }

        [Required]
        public string? Description { get; set; }
    }
}
