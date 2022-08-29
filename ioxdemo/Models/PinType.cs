using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ioxdemo.Models
{
    [Table("PinType")]
    public class PinType
    {
        public int PinTypeId { get; set; }

        [StringLength(20)]
        public string PinTypeName { get; set; }
    }
}