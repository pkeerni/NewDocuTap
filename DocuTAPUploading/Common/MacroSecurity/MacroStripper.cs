using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Common.MacroSecurity
{
    class MacroStripper
    {
        public void StripMacros(string file_name)
        {
            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(file_name, true))
            {
                doc.WorkbookPart.DeletePart(doc.WorkbookPart.VbaProjectPart);
            }
        }

        public void StripMacros(System.IO.Stream stream)
        {
            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(stream, true))
            {
                doc.WorkbookPart.DeletePart(doc.WorkbookPart.VbaProjectPart);
            }
        }
    }
}
