using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using Microsoft.Win32;
using System.Windows;

namespace Filters
{
    public class FileForFilter : INotifyPropertyChanged
    {
        private String sourceFileName;
        private Boolean isValid;
        private List<String> columnNames;
        private List<List<String>> spreadsheet;
        private List<List<String>> filteredSpreadsheet;
        private Int32 columnsCount;

        public String SourceFileName
        {
            get
            {
                return this.sourceFileName;
            }
            set
            {
                this.sourceFileName = value;
                OnPropertyChanged("SourceFileName");
            }
        }

        public Boolean IsValid
        {
            get
            {
                return this.isValid;
            }
            set
            {
                this.isValid = value;
                OnPropertyChanged("IsValid");
            }
        }

        public List<String> ColumnNames
        {
            get
            {
                return this.columnNames;
            }
            set
            {
                this.columnNames = value;
                OnPropertyChanged("ColumnNames");
            }
        }

        public List<List<String>> Spreadsheet
        {
            get
            {
                return this.spreadsheet;
            }
            set
            {
                this.spreadsheet = value;
                OnPropertyChanged("SpreadSheet");
            }
        }

        public List<List<String>> FilteredSpreadsheet
        {
            get
            {
                return this.filteredSpreadsheet;
            }
            set
            {
                this.filteredSpreadsheet = value;
                OnPropertyChanged("FilteredSpreadsheet");
            }
        }

        public Int32 ColumnsCount
        {
            get
            {
                return this.columnsCount;
            }
            set
            {
                this.columnsCount = value;
                OnPropertyChanged("ColumnsCount");
            }
        }

        public FileForFilter()
        {
            this.SourceFileName = String.Empty;
            this.IsValid = false;
            this.ColumnNames = new List<String>();
            this.Spreadsheet = new List<List<String>>();
            this.FilteredSpreadsheet = new List<List<String>>();
            this.ColumnsCount = 0;
        }

        public void OpenFile()
        {
            OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "(*.csv)|*.csv";
            ofd.Title = "Open .csv file...";
            if (false == ofd.ShowDialog())
            {
                return;
            }
            this.SourceFileName = ofd.FileName;
            this.ColumnNames = new List<String>();
            this.Spreadsheet = new List<List<String>>();
            this.FilteredSpreadsheet = new List<List<String>>();
            this.isValid = false;
            String fullText = String.Empty;
            using (Stream fStream = new FileStream(ofd.FileName,
                                                   FileMode.Open,
                                                   FileAccess.Read,
                                                   FileShare.None))
            {
                using (StreamReader sr = new StreamReader(fStream))
                {
                    fullText = sr.ReadToEnd();
                }
            }
            using (StringReader sr = new StringReader(fullText))
            {
                String line = sr.ReadLine();
                this.ColumnNames = line.Split(',').OfType<String>().ToList();
                this.ColumnsCount = this.ColumnNames.Count;
                this.Spreadsheet.Add(this.ColumnNames);
                while (null != (line = sr.ReadLine()))
                {
                    List<String> row = line.Split(',').OfType<String>().ToList();
                    if (row.Count != this.columnsCount)
                    {
                        this.isValid = false;
                        MessageBox.Show("Something wrong with this file.\n" + 
                                        "Different cells counts per row.\n" + 
                                        "Choose another one.");
                        return;
                    }
                    this.Spreadsheet.Add(row);
                }
                this.isValid = true;
                this.FilteredSpreadsheet = this.Spreadsheet;
            }
        }

        public void FilterText(Filter filter)
        {
            List<List<String>> newFilteredSpreadSheet = new List<List<String>>();
            newFilteredSpreadSheet.Add(this.Spreadsheet[0]);
            for (Int32 row_id = 1; row_id < this.Spreadsheet.Count; ++row_id)
            {
                List<String> row = this.Spreadsheet[row_id];
                Boolean validRow = true;
                foreach (Subfilter sf in filter.Subfilters)
                {
                    Int32 feature = sf.Feature;
                    Double cellVal=0, minVal, maxVal;
                    Double.TryParse(row[feature].Replace('.', ','), out cellVal);
                    Double.TryParse(sf.FromAsString.Replace('.', ','), out minVal);
                    Double.TryParse(sf.ToAsString.Replace('.', ','), out maxVal);
                    if ((cellVal < minVal) || (cellVal > maxVal)) validRow = false;
                }
                if (validRow) newFilteredSpreadSheet.Add(row);
            }
            this.FilteredSpreadsheet = newFilteredSpreadSheet;
        }

        public void ExportFilteredCSV()
        {
            SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.Filter = "(*.csv)|*.csv";
            sfd.Title = "Save filtered file...";
            if (false == sfd.ShowDialog())
            {
                return;
            }
            using (Stream fStream = new FileStream(sfd.FileName,
                                                   FileMode.Create,
                                                   FileAccess.Write,
                                                   FileShare.None))
            {
                using (StreamWriter sr = new StreamWriter(fStream))
                {
                    foreach (List<String> row in this.FilteredSpreadsheet)
                    {
                        sr.WriteLine(String.Join(",", row.ToArray<String>()));
                    }
                }
            }
            MessageBox.Show("Filtered file is saved to\n" + sfd.FileName);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(String propertyName)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
