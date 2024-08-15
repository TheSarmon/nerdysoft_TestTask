using System.ComponentModel.DataAnnotations;

namespace nerdysoft_TestTask.Models
{
    public class AnnouncementModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
