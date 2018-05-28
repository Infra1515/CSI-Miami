using System;

namespace CSI_Miami.Data.Models.Abstracts
{
    public interface IDeletable
    {
        bool IsDeleted { get; set; }

        DateTime? DeletedOn { get; set; }
    }
}