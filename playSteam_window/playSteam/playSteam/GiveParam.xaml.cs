using System.ComponentModel;
using System.Windows;
using Kalikowo;

namespace playSteam
{
    /// <summary>
    /// Interaction logic for GiveParam.xaml
    /// </summary>
    public partial class GiveParam : Window
    {

        public SteamSettings appSettings;

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

            /*
            appSettings.ApiKey = "API";
            appSettings.StemId = "SID";
            appSettings.CurrentMateId.MateId = "MID";
            appSettings.CurrentMateId.MateFriendlyName = "FriendlyMateName";
            Serialization.Serialize(appSettings);
            */

            appSettings = Serialization.Deserialize() as SteamSettings;
            if (appSettings != null)
                fillParamsGaps();
            else
                appSettings = new SteamSettings();

            EditableApiKey = true;      //API key field should be editable only at app start

            matesList.ItemsSource = appSettings.GetLastMates();
        }

#endregion


#region methods

        /*
         * Fill textboxes with data from XML file
         */
        public void fillParamsGaps()
        {
            uidtextbox.Text = appSettings.StemId;
            apikeytextbox.Text = appSettings.ApiKey;
            m8uidtextbox.Text = appSettings.CurrentMateId.MateId;
            m8textbox.Text = appSettings.CurrentMateId.MateFriendlyName;

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
            if((bool)wantm8.IsChecked)
            {
                string myUid = uidtextbox.Text;
                string apiKey = apikeytextbox.Text;

                if ((bool)mateFromListCB.IsChecked)
                {
                    var currentMate = (SteamSettings.Mates)matesList.SelectedItem;
                    setAppSettingsParams(myUid, apiKey, currentMate.MateId, currentMate.MateFriendlyName);
                }
                else
                {
                    setAppSettingsParams(myUid, apiKey, m8uidtextbox.Text, m8textbox.Text);
                    appSettings.AddMate(appSettings.CurrentMateId);
                }
            }

            Serialization.Serialize(appSettings);

            this.Hide();
        }

        private void setAppSettingsParams(string steamIdTxt, string apiKeyTxt, string mateIdTxt, string mateNameTxt)
        {
            appSettings.StemId = steamIdTxt;
            appSettings.ApiKey = apiKeyTxt;
            appSettings.CurrentMateId.MateId = mateIdTxt;
            appSettings.CurrentMateId.MateFriendlyName = mateNameTxt;
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

        private void wantm8_Click(object sender, RoutedEventArgs e)
        {
            if((bool)wantm8.IsChecked)
            {
                if ((bool)mateFromListCB.IsChecked)
                {
                    toggleMatesList(true);
                    toggleOneMate(false);
                }
                else
                {
                    toggleMatesList(false);
                    toggleOneMate(true);
                }
            }
            else
            {
                toggleMatesList(false, false);
                toggleOneMate(false);
            }
        }

        private void toggleMatesList(bool val, bool checkboxEnable = true)
        {
            matesList.IsEnabled = val;
            mateFromListCB.IsEnabled = checkboxEnable;
        }

        private void toggleOneMate(bool val)
        {
            m8textbox.IsEnabled = val;
            m8uidtextbox.IsEnabled = val;
        }



 #endregion
        
    }
}
