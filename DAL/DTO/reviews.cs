namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("public.reviews")]
    public partial class reviews
    {
        [Key]
        public int reviewid { get; set; }

        public int? userid { get; set; }

        public int? tourid { get; set; }

        public int rating { get; set; }

        public string comment { get; set; }

        public DateTime reviewdate { get; set; }

        public virtual tours tours { get; set; }

        public virtual users users { get; set; }
    }
}
