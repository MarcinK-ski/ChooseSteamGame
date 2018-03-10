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

        public SteamWpfUi(string uid, string apiKey) : base(uid, apiKey)
        {
        }

        /* Fill content by user info */
        public void showUserInfo(Label nick, Label name, Label country, Image avatar)
        {
            XElement userInfo = this.getMyUserInfo();
            if (userInfo == null)
            {
                if(Helper.isSettFileEx())
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
            if(realname != "" && realname != null)
                name.Content = $"(vel {realname})";
            country.Content = userInfo.Element("loccountrycode")?.Value;
            avatar.Source = new BitmapImage(new Uri(userInfo.Element("avatarfull")?.Value));
        }

        /* Show random game title */
        public void showGameTitle(Label game)
        {
            string choosedGame = this.rollGames();
            if(choosedGame != null)
                game.Content = choosedGame;
        }
    }
}
