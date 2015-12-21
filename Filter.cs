using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using Microsoft.Win32;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

namespace Filters
{
    [Serializable]
    public class Subfilter : INotifyPropertyChanged
    {
        private List<String> features;
        private Int32 feature;
        private String fromAsString;
        private String toAsString;
        private Filter filter;

        public List<String> Features
        {
            get
            {
                return this.features;
            }
            set
            {
                this.features = value;
                OnPropertyChanged("Features");
            }
        }

        public Int32 Feature
        {
            get
            {
                return this.feature;
            }
            set
            {
                this.feature = value;
                OnPropertyChanged("Feature");
            }
        }

        public String FromAsString
        {
            get
            {
                return this.fromAsString;
            }
            set
            {
                this.fromAsString = value;
                OnPropertyChanged("FromAsString");
            }
        }

        public String ToAsString
        {
            get
            {
                return this.toAsString;
            }
            set
            {
                this.toAsString = value;
                OnPropertyChanged("ToAsString");
            }
        }

        [field: NonSerialized()]
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

        private void DeleteSubfilter()
        {
            this.filter.Subfilters.Remove(this);
        }

        private Boolean CanDeleteSubfilter()
        {
            return true;
        }

        public Subfilter() { }

        public Subfilter(List<String> features, Filter filter) 
        {
            this.filter = filter;
            this.Features = features;
            this.FromAsString = String.Empty;
            this.ToAsString = String.Empty;
        }

        [field: NonSerialized()]
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(String propertyName)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [Serializable]
    public class Filter : INotifyPropertyChanged
    {
        private List<String> features;
        private ObservableCollection<Subfilter> subfilters;

        public List<String> Features
        {
            get
            {
                return this.features;
            }
            set
            {
                this.features = value;
                OnPropertyChanged("Features");
            }
        }

        public ObservableCollection<Subfilter> Subfilters
        {
            get
            {
                return this.subfilters;
            }
            set
            {
                this.subfilters = value;
                OnPropertyChanged("Subfilters");
            }
        }

        public Filter()
        {
            this.Features = new List<String>();
            this.Subfilters = new ObservableCollection<Subfilter>();
        }

        public Filter(FileForFilter fileForFilter)
        {
            this.Features = fileForFilter.ColumnNames;
            this.Subfilters = new ObservableCollection<Subfilter>();
        }

        public void AddSubfilter()
        {
            Subfilter subfilter = new Subfilter(this.Features, this);
            this.Subfilters.Add(subfilter);
            OnPropertyChanged("Subfilters");
        }

        public Boolean CanAddSubfilter()
        {
            if (0 == this.Features.Count) return false;
            return true;
        }

        public Boolean CanApply()
        {
            foreach (Subfilter sf in this.Subfilters)
            {
                Double from, to;
                if ((sf.Feature == -1) ||
                    !(Double.TryParse(sf.FromAsString.Replace('.', ','),
                     out from)) ||
                    !(Double.TryParse(sf.ToAsString.Replace('.', ','),
                     out to)))
                {
                    return false;
                }
            }
            return true;
        }

        public void Serialize()
        {
            SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.Filter = "(*.fltr)|*.fltr";
            sfd.Title = "Save filter...";
            if (false == sfd.ShowDialog())
            {
                return;
            }
            using (Stream fStream = new FileStream(sfd.FileName,
                                                   FileMode.Create,
                                                   FileAccess.Write,
                                                   FileShare.None))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fStream, this);
            }
            MessageBox.Show("Filter is serialized successfully!");
        }

        public Boolean CanSerialize()
        {
            return (0 != this.Subfilters.Count);
        }

        public void Deserialize(List<String> columnNames)
        {
            OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "(*.fltr)|*.fltr";
            ofd.Title = "Load filter...";
            if (false == ofd.ShowDialog())
            {
                return;
            }
            Filter deserialized = new Filter();
            using (Stream fStream = new FileStream(ofd.FileName,
                                                   FileMode.Open,
                                                   FileAccess.Read,
                                                   FileShare.None))
            {
                BinaryFormatter bf = new BinaryFormatter();
                deserialized = (Filter)bf.Deserialize(fStream);
            }
            if (deserialized.Features.Count != columnNames.Count)
            {
                MessageBox.Show("Sorry, this filter isn't applicable for current file.\nFilter isn't loaded.");
                return;
            }
            for (Int32 i = 0; i < deserialized.Features.Count; ++i)
            {
                if (deserialized.Features[i] != columnNames[i])
                {
                    MessageBox.Show("Sorry, this filter isn't applicable for current file.\nFilter isn't loaded.");
                    return;
                }
            }
            this.Features = deserialized.Features;
            this.Subfilters = deserialized.Subfilters;
            MessageBox.Show("Filter is loaded successfully!");
        }

        [field: NonSerialized()]
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
