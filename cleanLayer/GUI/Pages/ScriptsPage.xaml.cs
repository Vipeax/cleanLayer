using System;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;
using cleanLayer.Library;
using cleanLayer.Library.Scripts;

namespace cleanLayer.GUI.Pages
{
    /// <summary>
    /// Interaction logic for ScriptsPage.xaml
    /// </summary>
    public partial class ScriptsPage : UserControl, IGUIPage
    {
        public ScriptsPage()
        {
            InitializeComponent();
            _Readers = new ObservableCollection<LogReader>();
            foreach (var script in ScriptManager.ScriptPool)
                _Readers.Add(new ScriptLogReader(script));
            listBoxScripts.ItemsSource = _Readers;
            listBoxScripts.SelectedIndex = 0;
        }

        private LogReader _CompilerReader;
        private ObservableCollection<LogReader> _Readers;
        private Script _CurrentScript;

        #region GUI event handlers

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            _CurrentScript.Start();
        }

        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            _CurrentScript.Stop();
        }

        private void listBoxScripts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LogReader reader = e.AddedItems[0] as LogReader;
            if (reader == null)
                return;

            _CurrentScript = ((ScriptLogReader)reader).Script;
            buttonStart.IsEnabled = true;
            buttonStop.IsEnabled = true;
        }

        #endregion

        #region IGUIPage

        public string Header
        {
            get { return "Scripts"; }
        }

        #endregion
    }
}
