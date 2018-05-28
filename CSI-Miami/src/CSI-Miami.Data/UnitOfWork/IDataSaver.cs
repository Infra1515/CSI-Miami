using System;
using System.Collections.Generic;
using System.Text;

namespace CSI_Miami.Data.UnitOfWork
{
    public interface IDataSaver
    {
        void SaveChanges();

        void SaveChangesAsync();
    }
}
