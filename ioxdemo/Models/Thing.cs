namespace ioxdemo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Thing")]
    public partial class Thing
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Thing()
        {
            Pins = new HashSet<Pin>();
        }

        public int ThingId { get; set; }

        [Required]
        [StringLength(50)]
        public string ThingName { get; set; }

        [StringLength(1000)]
        public string SensorsInfo { get; set; }

        public bool? IsDeleted { get; set; }
        //public string CreatedBy { get; set; }/*Admin Person Username-Creator of Thing*/
        //public string AccessTo { get; set; }/*Access to Other Users*/
        public DateTime LastModified { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pin> Pins { get; set; }
        
    }
}
