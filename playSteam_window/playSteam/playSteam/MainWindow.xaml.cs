using System.Windows;

namespace playSteam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GiveParam param = new GiveParam();
        SteamWpfUi steam;

        public MainWindow()
        {
            param.ShowDialog();

            InitializeComponent();

            steam = new SteamWpfUi(Helper.xReadSettingVal("uid"), Helper.xReadSettingVal("api"), Helper.xReadSettingVal("m8uid"));
            loadData();
        }

        /*
         * Load user data
         */
        private void loadData()
        {
            bool showNonCUIDinfo = (bool) param.nonCUIDinfo.IsChecked;

            bool? isCustomID = steam.generateUID();
            if (isCustomID == false)
                if(showNonCUIDinfo)    
                MessageBox.Show("Given invalid User Custom ID, i'll use it as UID. (for you)", "INFO!", MessageBoxButton.OK, MessageBoxImage.Information);


            isCustomID = steam.generateUID(true);
            if (isCustomID == false)
                if (showNonCUIDinfo) 
                    MessageBox.Show("Given invalid User Custom ID, i'll use it as UID. (for m8)", "INFO!", MessageBoxButton.OK, MessageBoxImage.Information);
            
            steam.showUserInfo(nicklabel, namelabel, fromlabel, avatar);

            
            steam.showUserInfo(nicklabel_m8, namelabel_m8, fromlabel_m8, avatar_m8, steam.M8UID, true);

            if ((string) fromlabel_m8.Content == "")
                countryheader_m8.Content = "";
            else
                countryheader_m8.Content = "Country code:";
        }

        /*
         * Load random game title 
         */
        private void selectGame(bool isTwoPlayers = false)
        {
            if (!isTwoPlayers)
                steam.showGameTitle(choosedGame);
            else
            {
                //compare games, and randomize one
            }
        }

        /* Close hidden GiveParam window */
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            param.HideWindow = true;
            param.Close();
        }

        private void settings_Click(object sender, RoutedEventArgs e)
        {
            param.fillParamsGaps();
            param.ShowDialog();

            steam.UserID = Helper.xReadSettingVal("uid");
            steam.M8UID = param.wantm8.IsChecked == true ? Helper.xReadSettingVal("m8uid") : "";            
            
            loadData(); 
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            loadData(); 
        }

        private void info_Click(object sender, RoutedEventArgs e)
        {
            string about = "This (unfinished for now) app was created for people like me, who has great steam library and " +
                "they don't have idea, what game choose right now. \n\n" +
                "To use this app, you have to have Steam API Key! (link: http://steamcommunity.com/dev/apikey ) \n\n" +
                "App creator: Marcin Kalinowski\n" +
                "Gdansk A.D. 2018";

            MessageBox.Show(about, "About app", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
