using System;
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
using cleanLayer.GUI.Pages;

namespace cleanLayer.GUI
{
    public partial class MainWindow : Window
    {
        private LogReader _Reader;
        public MainWindow()
        {
            InitializeComponent();

            _Reader = new LogReader("General");
            Log.AddReader(_Reader);
            textBox.DataContext = _Reader;

            AddPage(new MainPage());
            AddPage(new ScriptsPage());
        }

        public void AddPage(IGUIPage page)
        {
            TabItem item = new TabItem();
            item.Header = page.Header;
            item.Content = page;
            tabControl.Items.Add(item);
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBox.ScrollToEnd();
        }
    }
}
