namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("public.bookings")]
    public partial class bookings
    {
        [Key]
        public int bookingid { get; set; }

        public string UserName { get; set; }
        public int? userid { get; set; }

        public int? tourid { get; set; }

        public DateTime bookingdate { get; set; }

        public int numberofseats { get; set; }

        public decimal totalprice { get; set; }

        public virtual tours tours { get; set; }

        public virtual users users { get; set; }
    }
}
