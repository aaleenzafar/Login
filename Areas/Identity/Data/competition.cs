using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Login.Models
{
    public class Competition
    {
            [Key]
            public int Com_Id { get; set; }


            [Column("com_name", TypeName = "varchar(200)")]
            [Required]
            public string Name { get; set; }


            [Column("com_date", TypeName = "datetime")]
            [Required]
            public DateTime Date { get; set; }


            [Column("com_deadline", TypeName = "datetime")]
            [Required]
            public DateTime Deadline { get; set; }



        [Column("com_image", TypeName = "varchar(1000)")]
        public string Image { get; set; }

    }
}
