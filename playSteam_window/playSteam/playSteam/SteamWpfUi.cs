using System;
using System.Xml.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace playSteam
{
    class SteamWpfUi : Steam
    {
        public SteamWpfUi(string uid, string apiKey) : base(uid, apiKey)
        {
        }

        /* Fill content by user info */
        public void showUserInfo(Label nick, Label name, Label country, Image avatar)
        {
            XElement userInfo = this.getMyUserInfo();

            nick.Content = userInfo.Element("personaname")?.Value;
            name.Content = $"(vel {userInfo.Element("realname")?.Value})";
            country.Content = userInfo.Element("loccountrycode")?.Value;
            avatar.Source = new BitmapImage(new Uri(userInfo.Element("avatarfull")?.Value));
        }

        /* Show random game title */
        public void showGameTitle(Label game)
        {
            game.Content = this.rollGames();
        }
    }
}
