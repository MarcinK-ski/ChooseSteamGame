using System;
using System.Xml.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;

namespace playSteam
{
    class SteamWpfUi : Steam
    {
        public SteamWpfUi() : base()    //Default key and apiKey
        {
        }

        public SteamWpfUi(string uid, string apiKey, string m8uid) : base(uid, apiKey)
        {
            this.M8UID = m8uid;
        }

        /* Fill content by user info */
        public void showUserInfo(Label nick, Label name, Label country, Image avatar, string uid = null, bool m8 = false)
        {
            XElement userInfo = this.getMyUserInfo(uid, m8);
            bool isRedundand = m8 && this.M8UID == this.UserID;
            if (userInfo == null || isRedundand)
            {
                if ((m8 && uid == "") || isRedundand)
                {
                    nick.Content = "NO MATE CHOOSEN";
                    name.Content = "";
                    country.Content = "";
                    avatar.Source = null;

                    return;
                }

                if(Helper.isFileEx())
                {
                    MessageBoxResult error = MessageBox.Show("Problem with APIKey or UserID. Please check it.", "ERROR!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBoxResult error = MessageBox.Show("There's no XML file! Creating emptyone...", "ERROR!", MessageBoxButton.OK, MessageBoxImage.Error);
                    Helper.xSettingsSave("", "");
                }
                return;
            }

            nick.Content = userInfo.Element("personaname")?.Value;

            string realname = userInfo.Element("realname")?.Value;
            if (realname != "" && realname != null)
                name.Content = $"(vel {realname})";
            else
                name.Content = null;
            
            string countryCode = userInfo.Element("loccountrycode")?.Value;
            if (countryCode != "" && countryCode != null)
                country.Content = countryCode;
            else
                country.Content = "unknown";

            avatar.Source = new BitmapImage(new Uri(userInfo.Element("avatarfull")?.Value));
        }

        /* Show random game title */
        public void showGameTitle(Label game)
        {
            string choosedGame = this.rollGames();
            if (choosedGame != null)
                game.Content = choosedGame;
            else
                game.Content = "There's no game in Yours library, you poor guy...";
        }
    }
}
