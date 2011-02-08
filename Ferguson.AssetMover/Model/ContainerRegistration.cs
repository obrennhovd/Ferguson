using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private string id = "";
        public string Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    Notify("Id");
                }
            }
        }

        private DateTime created = DateTime.Now;
        public DateTime Created
        {
            get { return created; }
            set
            {
                if (created != value)
                {
                    created = value;
                    Notify("Created");
                }
            }
        }
    }
}
