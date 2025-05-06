namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("public.images")]
    public partial class images
    {
        [Key]
        public int imageid { get; set; }

        public int? tourid { get; set; }

        [Required]
        public string imagepath { get; set; }

        public virtual tours tours { get; set; }
    }
}
