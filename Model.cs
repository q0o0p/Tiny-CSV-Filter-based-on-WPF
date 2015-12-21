using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Windows;
using System.ComponentModel;

namespace Filters
{
    public class Model : INotifyPropertyChanged
    {
        private FileForFilter fileForFilter;
        private Filter filter;

        public FileForFilter FileForFilter
        {
            get
            {
                return this.fileForFilter;
            }
            set
            {
                this.fileForFilter = value;
                OnPropertyChanged("FileForFilter");
            }
        }

        public Filter Filter
        {
            get
            {
                return this.filter;
            }
            set
            {
                this.filter = value;
                OnPropertyChanged("Filter");
            }
        }

        public Model()
        {
            this.Filter = new Filter();
            this.FileForFilter = new FileForFilter();
        }

        private RelayCommand openFileCommand;
        public RelayCommand OpenFileCommand
        {
            get
            {
                if (null == this.openFileCommand)
                {
                    this.openFileCommand = new RelayCommand(this.OpenFile, this.CanOpenFile);
                }
                return this.openFileCommand;
            }
            set
            {
                this.openFileCommand = value;
            }
        }

        private RelayCommand exportCSVCommand;
        public RelayCommand ExportCSVCommand
        {
            get
            {
                if (null == this.exportCSVCommand)
                {
                    this.exportCSVCommand = new RelayCommand(this.ExportCSV, this.CanExportCSV);
                }
                return this.exportCSVCommand;
            }
            set
            {
                this.exportCSVCommand = value;
            }
        }

        private RelayCommand addSubfilterCommand;
        public RelayCommand AddSubfilterCommand
        {
            get
            {
                if (null == this.addSubfilterCommand)
                {
                    this.addSubfilterCommand = new RelayCommand(this.AddSubfilter,
                                                                this.CanAddSubfilter);
                }
                return this.addSubfilterCommand;
            }
            set
            {
                this.addSubfilterCommand = value;
            }
        }

        private RelayCommand applyFilterCommand;
        public RelayCommand ApplyFilterCommand
        {
            get
            {
                if (null == this.applyFilterCommand)
                {
                    this.applyFilterCommand = new RelayCommand(this.ApplyFilter,
                                                                this.CanApplyFilter);
                }
                return this.applyFilterCommand;
            }
            set
            {
                this.applyFilterCommand = value;
            }
        }

        private RelayCommand deleteSubfilterCommand;
        public RelayCommand DeleteSubfilterCommand
        {
            get
            {
                if (null == this.deleteSubfilterCommand)
                {
                    this.deleteSubfilterCommand = new RelayCommand(this.DeleteSubfilter,
                                                                           this.CanDeleteSubfilter);
                }
                return this.deleteSubfilterCommand;
            }
            set
            {
                this.deleteSubfilterCommand = value;
            }
        }

        private RelayCommand serializeFilterCommand;
        public RelayCommand SerializeFilterCommand
        {
            get
            {
                if (null == this.serializeFilterCommand)
                {
                    this.serializeFilterCommand = new RelayCommand(this.SerializeFilter,
                                                                   this.CanSerializeFilter);
                }
                return this.serializeFilterCommand;
            }
            set
            {
                this.serializeFilterCommand = value;
            }
        }

        private RelayCommand deserializeFilterCommand;
        public RelayCommand DeserializeFilterCommand
        {
            get
            {
                if (null == this.deserializeFilterCommand)
                {
                    this.deserializeFilterCommand = new RelayCommand(this.DeserializeFilter,
                                                                      this.CanDeserializeFilter);
                }
                return this.deserializeFilterCommand;
            }
            set
            {
                this.deserializeFilterCommand = value;
            }
        }

        private RelayCommand exitCommand;
        public RelayCommand ExitCommand
        {
            get
            {
                if (null == this.exitCommand)
                {
                    this.exitCommand = new RelayCommand(this.Exit,
                                                        this.CanExit);
                }
                return this.exitCommand;
            }
            set
            {
                this.exitCommand = value;
            }
        }

        private RelayCommand showFeedbackCommand;
        public RelayCommand ShowFeedbackCommand
        {
            get
            {
                if (null == this.showFeedbackCommand)
                {
                    this.showFeedbackCommand = new RelayCommand(this.ShowFeedback,
                                                                this.CanShowFeedback);
                }
                return this.showFeedbackCommand;
            }
            set
            {
                this.showFeedbackCommand = value;
            }
        }

        private RelayCommand howToUseCommand;
        public RelayCommand HowToUseCommand
        {
            get
            {
                if (null == this.howToUseCommand)
                {
                    this.howToUseCommand = new RelayCommand(this.HowToUse,
                                                            this.CanHowToUse);
                }
                return this.howToUseCommand;
            }
            set
            {
                this.howToUseCommand = value;
            }
        }

        private RelayCommand infoCommand;
        public RelayCommand InfoCommand
        {
            get
            {
                if (null == this.infoCommand)
                {
                    this.infoCommand = new RelayCommand(this.Info,
                                                            this.CanInfo);
                }
                return this.infoCommand;
            }
            set
            {
                this.infoCommand = value;
            }
        }

        private void OpenFile()
        {
            this.FileForFilter.OpenFile();
            this.Filter = new Filter(fileForFilter);
        }

        private Boolean CanOpenFile()
        {
            return true;
        }

        private void ExportCSV()
        {
            this.FileForFilter.ExportFilteredCSV();
        }

        private Boolean CanExportCSV()
        {
            return (0 != this.FileForFilter.FilteredSpreadsheet.Count);
        }

        private void AddSubfilter()
        {
            this.Filter.AddSubfilter();
            OnPropertyChanged("Filter");
        }

        private Boolean CanAddSubfilter()
        {
            return this.Filter.CanAddSubfilter();
        }

        private void ApplyFilter()
        {
            this.FileForFilter.FilterText(this.Filter);
        }

        private Boolean CanApplyFilter()
        {
            return this.Filter.CanApply();
        }

        private void DeleteSubfilter()
        {
            this.Filter.AddSubfilter();
            OnPropertyChanged("Filter");
        }

        private Boolean CanDeleteSubfilter()
        {
            return this.Filter.CanAddSubfilter();
        }

        private void SerializeFilter()
        {
            this.Filter.Serialize();
        }

        private Boolean CanSerializeFilter()
        {
            return this.Filter.CanSerialize();
        }

        private void DeserializeFilter()
        {
            this.Filter.Deserialize(this.FileForFilter.ColumnNames);
        }

        private Boolean CanDeserializeFilter()
        {
            return (0 != this.FileForFilter.ColumnNames.Count);
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }

        private Boolean CanExit()
        {
            return true;
        }

        private void ShowFeedback()
        {
            MessageBox.Show("If you have any questions, comments,\n" + 
                            "additions, advices, suggestions, grievances,\n" +
                            "or commercial proposals,\n" +
                            "you can send an email to q0o0p@yandex.ru",
                            "Your feedback is very important for me!");
        }

        private Boolean CanShowFeedback()
        {
            return true;
        }

        private void HowToUse()
        {
            MessageBox.Show("How to use this program: Open .csv file. First row\n" +
                            "should contain feature names, and other rows should\n" +
                            "contain values. Only numerical values are supported.\n" +
                            "I will add support for string values, if there will\n" +
                            "be much demand. After .csv file opening you can\n" +
                            "create as manu subfilters as you want. Then you\n" +
                            "should press the button Apply Filter, and filtered\n" +
                            "spreadsheet will be showed. You can save filtered\n" +
                            "spreadsheet using the appropriate menu item. Also you\n" +
                            "can save filters themselves and load them for\n" +
                            "repeatedly using.", "I hope you enjoy this program!");
        }

        private Boolean CanHowToUse()
        {
            return true;
        }

        private void Info()
        {
            MessageBox.Show("    Though this program doesn't satisfy all strict requirements of MVVM \n" +
                            "pattern, anyway it is well-designed, I would even say perfect, for example,\n" +
                            "program logic knows nothing about the view, so we can partially or fully\n" +
                            "change view without changing behind-code. In its turn the view knows nothing\n" +
                            "about logic, except few names of properties and commands, which it is binded\n" +
                            "to, so we can change the program logic vithout changing the view. The only\n" +
                            "requirement is invariability of some names, which the view is bound to.\n" +
                            "\n" +
                            "    There are 5 classes in this program:\n" +
                            "1. FileForFilter - class responsible for operations with files: loading,\n" +
                            "checking for validity, conversion to spreadsheet, applying filters and export\n" +
                            "of filtered spreadsheets to the disk.\n" +
                            "\n" +
                            "2. Filter and Subfilter - classes responsible for filter characteristics.\n" +
                            "Each subfilter contains field defining feature, which the filtering will be\n" +
                            "performed on, and minimum and maximum bounds for filtering. In its turn\n" +
                            "Filter class contains collection of subfilters and provides operations on it. \n" +
                            "Both these classes are serializable for enabling export filters to the disk \n" +
                            "with following loading.\n" +
                            "\n" +
                            "3. Model - class responsible for interaction between FileForFilter and Filter \n" +
                            "classes. Contains almost no its own logic, just wrappers for Filter and \n" +
                            "FileForFilter methods.\n" +
                            "All above classes are equipped with INotifyPropertyChangde interface, so view \n" +
                            "always receives actual values for binding.\n" +
                            "\n" +
                            "4. The last class is RelayCommand. This class is the best-implemented, \n" +
                            "because it is borrowed from MSDN forum. To tell the truth, I haven't \n" +
                            "understand fully, why this is better than usual Command class.\n" +
                            "\n" +
                            "All classes, fields, methods and so on are named clearly and uniformly.\n" +
                            "All icons are taken from iconfinder.com, all of them are free.\n" +
                            "I hope you enjoy reading the source code!",
                            "About the source code");
        }

        private Boolean CanInfo()
        {
            return true;
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
