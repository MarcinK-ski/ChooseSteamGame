using System.Windows;

namespace playSteam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

#region globalsy

        GiveParam param = new GiveParam();  //Window with settings
        SteamWpfUi steam;   //Steam class with some features for GUI
        SteamSettings appSettings;

#endregion

#region constr

        public MainWindow()
        {
            param.ShowDialog();     //First of all we have to give parameters or accept oldone
            appSettings = param.appSettings;

            InitializeComponent();

            steam = new SteamWpfUi(ref appSettings, appSettings.StemId, appSettings.ApiKey, appSettings.CurrentMateId.MateId);
            loadData();
        }

#endregion


#region methods

        /*
         * Load user data (no matter is mate too or no)
         */
        private void loadData()
        {
            bool showNonCUIDinfo = (bool) param.nonCUIDinfo.IsChecked;      //Checkbox from settings, which disable/enable info, that CustomUID isn't correct, so app will try use it as normal UID

        #region User

            bool? isCustomID = steam.generateUID();
            if (isCustomID == false)
                if(showNonCUIDinfo)    
                MessageBox.Show("Given invalid User Custom ID, i'll use it as UID. (for you)", "INFO!", MessageBoxButton.OK, MessageBoxImage.Information);

            steam.showUserInfo(nicklabel, namelabel, fromlabel, avatar);

        #endregion


        #region Mate
            if((bool)param.wantm8.IsChecked)
            {
                isCustomID = steam.generateUID(true);
                if (isCustomID == false)
                    if (showNonCUIDinfo)
                        MessageBox.Show("Given invalid User Custom ID, i'll use it as UID. (for m8)", "INFO!", MessageBoxButton.OK, MessageBoxImage.Information);

                steam.showUserInfo(nicklabel_m8, namelabel_m8, fromlabel_m8, avatar_m8, steam.M8UID, true);

                if ((string)fromlabel_m8.Content == "")    //Clear countryLabel if country is empty
                    countryheader_m8.Content = "";
                else
                    countryheader_m8.Content = "Country code:";
            }
            else
            {
                nicklabel_m8.Content = "NO MATE CHOOSEN";
                namelabel_m8.Content = "";
                fromlabel_m8.Content = "";
                countryheader_m8.Content = "";
                avatar_m8.Source = null;
            }
            #endregion

            bool isTwoPlayers = !steam.wasLastRedundand && steam.M8UID != "" && (bool)param.wantm8.IsChecked;   //Checking to decide: "Should I choose game for two players or one?"
            selectGame(isTwoPlayers);
        }

        /*
         * Load random game title 
         */
        private void selectGame(bool isTwoPlayers = false)
        {
            choosedGame.Content = "No info";

            if (!isTwoPlayers)
            {
                steam.showGameTitle(choosedGame);
            }
            else
            {
                steam.showGameTitle(choosedGame, steam.getCommonGames());
            }
        }

#endregion

#region events

        /* 
         * Close hidden GiveParam window 
         */
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            param.HideWindow = true;
            param.Close();
        }

        /*
         * Open settings window
         */
        private void settings_Click(object sender, RoutedEventArgs e)
        {
            param.fillParamsGaps();
            param.ShowDialog();

            steam.UserID = appSettings.StemId;
            steam.M8UID = param.wantm8.IsChecked == true ? appSettings.CurrentMateId.MateId : "";            
            
            loadData(); 
        }

        /*
         * Refresh loaded data
         */
        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            loadData(); 
        }

        /*
         * About app
         */
        private void info_Click(object sender, RoutedEventArgs e)
        {
            string about = "This (unfinished for now) app was created for people like me, who has great steam library and " +
                "they don't have idea, which game choose right now. \n\n" +
                "To use this app, you have to have Steam API Key! (link: http://steamcommunity.com/dev/apikey ) \n\n" +
                "App creator: Marcin Kalinowski\n" +
                "Gdansk A.D. 2018";

            MessageBox.Show(about, "About app", MessageBoxButton.OK, MessageBoxImage.Information);
        }

#endregion

    }
}
