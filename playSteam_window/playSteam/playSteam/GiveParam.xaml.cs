using System.ComponentModel;
using System.Windows;

namespace playSteam
{
    /// <summary>
    /// Interaction logic for GiveParam.xaml
    /// </summary>
    public partial class GiveParam : Window
    {

#region get/set
        public bool HideWindow { get; set; } = false;   //Hide window after click close button or nope?
        public const bool defaultApiBoxState = false;   //API Key field should be false/true as default
        public bool EditableApiKey      //Set API Key field as editable or nope
        {
            set
            {
                apikeytextbox.IsEnabled = value;
            }
        }
#endregion

#region constr

        public GiveParam()
        {
            InitializeComponent();
            fillParamsGaps();
            EditableApiKey = true;      //API key field should be editable only at app start
        }

#endregion


#region methods

        /*
         * Fill textboxes with data from XML file
         */
        public void fillParamsGaps()
        {
            uidtextbox.Text = Helper.xReadSettingVal("uid");
            apikeytextbox.Text = Helper.xReadSettingVal("api");
            m8uidtextbox.Text = Helper.xReadSettingVal("m8uid");

            if (apikeytextbox.Text == "")   //Set API Key field editable only if is empty
                EditableApiKey = true;
            else
                EditableApiKey = defaultApiBoxState;    //Set default value
        }
#endregion

#region events

        /*
         * Save changes
         */
        private void saveparamsbutton_Click(object sender, RoutedEventArgs e)
        {
            if(wantm8.IsChecked == true)
                Helper.xSettingsSave(uidtextbox.Text, apikeytextbox.Text, m8uidtextbox.Text);
            else
                Helper.xSettingsSave(uidtextbox.Text, apikeytextbox.Text);

            this.Hide();
        }

        /*
         * Cancel button
         */
        private void cancelparamsbutton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        /*
         * Behavior after closing window (is using to hide window instaad of close it)
         */
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = !HideWindow;
            this.Hide();
        }
        /*
         * Enable/disable mate adding 
         */

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            m8uidtextbox.IsEnabled = m8uidtextbox.IsEnabled ? false : true;
        }
#endregion
    }
}
