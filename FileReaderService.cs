using System;
using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;

namespace RestaurantModel
{
    public class FileReaderService
    {
        public FileReaderService()
        {

        }
        
        public List<MenuItem> ReadCSV(string filePath)
        {
            var result = new List<MenuItem>();
            using (TextFieldParser csvParser = new TextFieldParser(filePath))
            {
            csvParser.SetDelimiters(new string[] { "," });
            csvParser.HasFieldsEnclosedInQuotes = true;

            // Skip the row with the column names
            csvParser.ReadLine();

            while (!csvParser.EndOfData)
            {
                // Read current line fields, pointer moves to the next line.
                string[] fields = csvParser.ReadFields();
                result.Add(new MenuItem(fields[0], Convert.ToDecimal(fields[1])));
            }
            return result;
            }
        }
    }
}
