namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("public.tours")]
    public partial class tours
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tours()
        {
            bookings = new HashSet<bookings>();
            images = new HashSet<images>();
            reviews = new HashSet<reviews>();
        }

        [Key]
        public int tourid { get; set; }

        [Required]
        [StringLength(200)]
        public string name { get; set; }

        public string description { get; set; }

        public decimal price { get; set; }

        [Column(TypeName = "date")]
        public DateTime startdate { get; set; }

        [Column(TypeName = "date")]
        public DateTime enddate { get; set; }

        [Required]
        [StringLength(200)]
        public string destination { get; set; }

        public int? transportid { get; set; }

        public int availableseats { get; set; }

        [Required]
        [StringLength(20)]
        public string status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<bookings> bookings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<images> images { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<reviews> reviews { get; set; }

        public virtual transport transport { get; set; }
    }
}
