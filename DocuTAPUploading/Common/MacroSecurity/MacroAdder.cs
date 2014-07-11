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
    class MacroAdder
    {
        private void AddMacros(SpreadsheetDocument doc, string macro_filename)
        {
            WorkbookPart part = doc.WorkbookPart;
            int count = part.GetPartsCountOfType<VbaProjectPart>();
            VbaProjectPart vba_part = null;
            IEnumerable<VbaProjectPart> parts = part.GetPartsOfType<VbaProjectPart>();
            if (parts.Count() > 0)
            {
                vba_part = parts.First();
            }
            else
            {
                vba_part = part.AddNewPart<VbaProjectPart>();
            }
            byte[] macro_data = System.IO.File.ReadAllBytes(macro_filename);
            System.IO.Stream stream_data = new System.IO.MemoryStream(macro_data);
            vba_part.FeedData(stream_data);
            stream_data.Close();
        }

        public void AddMacros(System.IO.Stream stream, string macro_filename)
        {
            try
            {
                using (SpreadsheetDocument doc = SpreadsheetDocument.Open(stream, true))
                {
                    AddMacros(doc, macro_filename);
                }
            }
            catch (Exception e)
            {
                var str = e.Message;
            }
        }

        public void AddMacros(string file_name, string macro_filename)
        {
            try
            {
                using (SpreadsheetDocument doc = SpreadsheetDocument.Open(file_name, true))
                {
                    AddMacros(doc, macro_filename);
                }
            }
            catch (Exception e)
            {
                var str = e.Message;
            }
        }

        /// <summary>
        /// Dumps a macro exported from xlsm file to a file on the disk.
        /// Used in conjunction with AddMacro method.
        /// Use this when trying to export macros
        /// Tools Open XML Productivity Tools:
        /// Open given xlsm file
        /// Click on "Reflect Code"
        /// Click on the /xl/workbook.xml/xl/vbaProject.bin node
        /// Copy the string in partData and pass the string to this method as parameter
        /// </summary>
        /// <param name="data">Base64 encoded string</param>
        /// <param name="macro_filename"></param>
        public void DumpMacro(string data, string macro_filename)
        {
            byte[] macro_data = System.Convert.FromBase64String(data);
            System.IO.File.WriteAllBytes(macro_filename, macro_data);
        }
    }
}
