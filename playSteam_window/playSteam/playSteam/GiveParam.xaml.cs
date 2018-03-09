using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace playSteam
{
    /// <summary>
    /// Interaction logic for GiveParam.xaml
    /// </summary>
    public partial class GiveParam : Window
    {
        public bool HideWindow { get; set; } = false;   //Hide window after click close button or nope?
        private bool editebleApiKey = false;
        public bool EditableApiKey
        {
            get
            {
                return this.editebleApiKey;
            }
            set
            {
                apikeytextbox.IsEnabled = value;
            }
        }

        public GiveParam()
        {
            InitializeComponent();
            fillParamsGaps();
            EditableApiKey = true;
        }
        
        public void fillParamsGaps()
        {
            uidtextbox.Text = Helper.xReadSettingVal("uid");

            apikeytextbox.Text = Helper.xReadSettingVal("api");

            if (apikeytextbox.Text == "")
                EditableApiKey = true;
            else
                EditableApiKey = false;
        }

        private void saveparamsbutton_Click(object sender, RoutedEventArgs e)
        {
            Helper.xSettingsSave(uidtextbox.Text, apikeytextbox.Text);
            this.Hide();
        }

        private void cancelparamsbutton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = !HideWindow;
            this.Hide();
        }
    }
}
