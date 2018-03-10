using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        }

        /*
         * Load random game title and user data
         */
        private void loadData()
        {
            bool? isCustomID = steam.generateUID(param.getCustomID());
            if (isCustomID == false)
                MessageBox.Show("Given invalid User Custom ID, i'm using last UID.", "ERROR!", MessageBoxButton.OK, MessageBoxImage.Error);

            steam.showUserInfo(nicklabel, namelabel, fromlabel, avatar);
            steam.showGameTitle(choosedGame);
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
                "they don't have idea, what game choose now. \n\n" +
                "To use this app, you have to have Steam API Key! (link: http://steamcommunity.com/dev/apikey ) \n\n" +
                "App creator: Marcin Kalinowski\n" +
                "Gdansk A.D. 2018";

            MessageBox.Show(about, "About app", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
