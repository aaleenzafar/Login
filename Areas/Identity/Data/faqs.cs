using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Login.Models
{
    public class Faqs
    {
        [Key]
        public int Id  { get; set; }

        [Column("f_question",TypeName ="varchar(400)")]
        [Required]
        public string Question { get; set; }

        [Column("f_Answer", TypeName = "varchar(1000)")]
        [Required]
        public string Answer { get; set; }
    }
   
}
