using System;

namespace CSI_Miami.Data.Models.Abstracts
{
    public interface IEditable
    {
        DateTime? CreatedOn { get; set; }

        DateTime? ModifiedOn { get; set; }
    }
}