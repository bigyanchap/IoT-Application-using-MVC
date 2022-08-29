namespace ioxdemo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Pin")]
    public partial class Pin
    {
        public short PinId { get; set; }

        [Required]
        [StringLength(30)]
        public string PinName { get; set; }

        public int ThingId { get; set; }

        public int PinTypeId { get; set; }

        public bool? OnOff { get; set; }

        public int? Magnitude { get; set; }

        public DateTime? OnFrom { get; set; }

        public DateTime? OnTill { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public virtual PinType PinType { get; set; }
        public virtual Thing Thing { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if(!(obj is Pin))
            {
                return false;
            }
            return
                this.PinId == ((Pin)obj).PinId &&
                this.PinName == ((Pin)obj).PinName &&
                this.ThingId == ((Pin)obj).ThingId &&
                this.PinTypeId == ((Pin)obj).PinTypeId &&
                this.OnOff == ((Pin)obj).OnOff &&
                this.Magnitude == ((Pin)obj).Magnitude &&
                this.OnFrom == ((Pin)obj).OnFrom &&
                this.OnTill == ((Pin)obj).OnTill;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
