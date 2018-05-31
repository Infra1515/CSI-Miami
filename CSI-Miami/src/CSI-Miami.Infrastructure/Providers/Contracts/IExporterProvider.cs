using System;
using System.Collections.Generic;
using System.Text;

namespace CSI_Miami.Infrastructure.Providers.Contracts
{
    public interface IExporterProvider
    {
        string ExportDataAsJson(string rawSqlCommand);
        void WriteDataAsJson(string text);
    }
}
