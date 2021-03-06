// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VerificationWebApp.DbModels
{
    [Table("Verified")]
    public partial class Verified
    {
        [Key]
        [Column("VerifyID")]
        public int VerifyId { get; set; }
        /// <summary>
        /// account number
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string AcctNo { get; set; }
        /// <summary>
        /// Ghana Card or TIN Number
        /// </summary>
        /// 
        public string AcctName { get; set; }

        [Column("NationalID")]
        [StringLength(50)]
        [Unicode(false)]
        public string NationalId { get; set; }
        /// <summary>
        /// The short code from the NIA verification
        /// </summary>
        [StringLength(100)]
        [Unicode(false)]
        public string ShortCode { get; set; }
        /// <summary>
        /// The telephone number 
        /// </summary>
        [StringLength(50)]
        [Unicode(false)]
        public string Telephone { get; set; }

        public string frontPicture { get; set; }

        public string backPicture { get; set; }

        public string rawData { get; set; }
    }
}