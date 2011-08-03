using System;
using System.ComponentModel;

namespace Ferguson.AssetMover.Client.Model
{
    public class ContainerRegistration: INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void Notify(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string _id = "";
        public string Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    Notify("Id");
                }
            }
        }

        private DateTime _created = DateTime.Now;
        public DateTime Created
        {
            get { return _created; }
            set
            {
                if (_created != value)
                {
                    _created = value;
                    Notify("Created");
                }
            }
        }
    }
}
