using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace cleanLayer
{
    public class LogReader : ILog, INotifyPropertyChanged
    {
        public LogReader(string name, int maxLength = 1000000)
        {
            this.name = name;
            this.maxLength = maxLength;

            text = "";
        }

        private string name;
        private int maxLength;
        private string text;

        public override string ToString()
        {
            return name;
        }

        #region ILogReader

        public void WriteLine(string line)
        {
            text += line + Environment.NewLine;

            if (text.Length > maxLength)
            {
                text = text.Substring(maxLength / 10);

                int index = text.IndexOf(Environment.NewLine);
                if (index != -1)
                    text = text.Substring(index + 1);
            }

            OnPropertyChanged("Text");
        }

        public void Clear()
        {
            text = "";

            OnPropertyChanged("Text");
        }

        #endregion

        #region INotifyPropertyChanged

        protected void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        public string Name
        {
            get { return name; }
        }

        public string Text
        {
            get { return text; }
        }

        #endregion
    }
}
