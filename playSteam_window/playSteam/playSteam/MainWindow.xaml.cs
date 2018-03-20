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

            steam = new SteamWpfUi(Helper.xReadSettingVal("uid"), Helper.xReadSettingVal("api"));
            loadData();
            loadData(true);
        }

        /*
         * Load user data
         */
        private void loadData(bool isSecondPlayer = false)
        {
            bool? isCustomID = steam.generateUID(isSecondPlayer);
            string forWhom = isSecondPlayer ? "(for m8)" : "(for you)";
            if (isCustomID == false)
                MessageBox.Show("Given invalid User Custom ID, i'll use it as UID." + forWhom, "INFO!", MessageBoxButton.OK, MessageBoxImage.Information);

            if (isSecondPlayer)
            {
                steam.showUserInfo(nicklabel_m8, namelabel_m8, fromlabel_m8, avatar_m8, steam.M8UID, true);

                if ((string)fromlabel_m8.Content == "")
                    countryheader_m8.Content = "";
            }
            else
                steam.showUserInfo(nicklabel, namelabel, fromlabel, avatar);
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
