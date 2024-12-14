namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("public.transport")]
    public partial class transport
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public transport()
        {
            tours = new HashSet<tours>();
        }

        public int transportid { get; set; }

        [Required]
        [StringLength(50)]
        public string type { get; set; }

        public int capacity { get; set; }

        [StringLength(100)]
        public string company { get; set; }

        public decimal price { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tours> tours { get; set; }
    }
}
