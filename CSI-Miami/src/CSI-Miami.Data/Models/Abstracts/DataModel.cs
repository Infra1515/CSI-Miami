using System;
using System.ComponentModel.DataAnnotations;

namespace CSI_Miami.Data.Models.Abstracts
{
    public class DataModel : IDeletable, IEditable
    {
        public bool IsDeleted { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeletedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CreatedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ModifiedOn { get; set; }
    }
}
