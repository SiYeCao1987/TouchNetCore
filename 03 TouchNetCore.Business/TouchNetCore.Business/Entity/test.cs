using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TouchNetCore.Business.Entity
{
    [Table("test")]
    public class test
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
    }
}
