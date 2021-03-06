﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CSI_Miami.Data.CustomAttributes;
using CSI_Miami.Data.Models.Abstracts;

namespace CSI_Miami.Data.Models
{
    public class Movie : DataModel
    {
        public int Id { get; set; }

        [MaxLength(200)]
        [Required]
        public string Title { get; set; }

        [MaxLength(100)]
        public string DirectorName { get; set; }

        [ValidateFutureDate]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ReleaseDate { get; set; }
    }
}
