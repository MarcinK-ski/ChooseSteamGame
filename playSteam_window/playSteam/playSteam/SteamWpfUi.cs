using System;
using System.Xml.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Newtonsoft.Json;

namespace playSteam
{
    class SteamWpfUi : Steam
    {

        public bool wasLastRedundand = false;

#region constructors
        public SteamWpfUi() : base()    //Default key and apiKey
        {
        }

        public SteamWpfUi(string uid, string apiKey, string m8uid) : base(uid, apiKey)
        {
            this.M8UID = m8uid;
        }
#endregion


#region methods

        /* 
         * Fill content by user info 
         */
        public void showUserInfo(Label nick, Label name, Label country, Image avatar, string uid = null, bool m8 = false)
        {
            XElement userInfo = this.getMyUserInfo(uid, m8);

            bool isRedundand = m8 && this.M8UID == this.UserID;     //In case "mate mode"; be shure, that given MateUID is different to UID
            this.wasLastRedundand = isRedundand;

            if (userInfo == null || isRedundand)
            {
                if ((m8 && uid == "") || isRedundand)
                {
                    /*** Default values if theres no mate given ***/
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

            string realname = userInfo.Element("realname")?.Value;  //REALNAME IS NOT ALWAYS USED!!!
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

        /* 
         * Show random game title for "single/multi mode"
         */
        public void showGameTitle(Label gameLabel, List<string> games = null)
        {
            string choosedGame = this.rollGames(games);

            if (choosedGame != null)
                gameLabel.Content = choosedGame;
            else
            {
                if(games == null)
                    gameLabel.Content = "There's no game in Yours library, you poor guy...";
                else
                    gameLabel.Content = "You haven't common games, you poor guys...";
            }
        }


    #region twoUsersGames


        /*
         * Get your and mate common games list
         */
        public List<string> getCommonGames()
        {

        #region user

            XElement[] allUserGames = getUserGameXArray();

            HashSet<double> playerGames = new HashSet<double>();
            
            /* Saving User's game IDs  */
            for (int i = 0; i < allUserGames.Count(); i++)
            {
                string xel = allUserGames[i].Element("appid")?.Value; //Get only game ID from XML
                if (xel != null)
                {
                    double res;
                    double.TryParse(xel, out res);

                    playerGames.Add(res);    //Add ID to users Set
                }
            }

        #endregion


        #region mate

            XElement[] allMateGames = getUserGameXArray(false);

            HashSet<double> commonGIDs = new HashSet<double>();

            for (int i = 0; i < allMateGames.Count(); i++)
            {
                string xel = allMateGames[i].Element("appid")?.Value; //Get only game ID from XML
                if (xel != null)
                {
                    double res;
                    double.TryParse(xel, out res);

                    if(playerGames.Contains(res))   //If user has this same game, which is in mate library
                    {
                        commonGIDs.Add(res);    //Add ID to common Set
                        //Trace.WriteLine(res + " --> " + allMateGames[i].Element("name")?.Value);
                    }
                }

            }

         #endregion

            return filterMulti(commonGIDs);
        }

        /*
         * Filter only coop games ( return titles )
         */
         protected List<string> filterMulti(HashSet<double> commonGames)
        {
            /** ID (int) => success & data (only if success==true) => categories => array (0 - n) => id (1 == multiplayer) **/

            WebClient wc = new WebClient();

            List<string> commonMultiGames = new List<string>();

            foreach (double gameID in commonGames)
            {
                string uri = $"http://store.steampowered.com/api/appdetails?appids={gameID}";   
                
                string json = wc.DownloadString(uri);       // Download json, by game ID

                var jsonResult = JToken.Parse(json)[gameID.ToString()];

                bool isSuccessfuly = jsonResult["success"].ToObject<bool>();
                if (!isSuccessfuly)
                    continue;                   //skip if ID is wrong or there's no game page (ex. like Metro 2033)

                var categories = jsonResult["data"]["categories"];    

                foreach (var item in categories)        //Category should be as "multiplayer" (id: 1)
                {
                    byte catId = item["id"].ToObject<byte>();
                    string catName = item["description"].ToObject<string>();
                    Trace.WriteLine(catId + "   -   " + catName);

                    if(catId == 1)
                        commonMultiGames.Add(jsonResult["data"]["name"].ToObject<string>());
                }
            }

            if (commonMultiGames.Count > 0)
                return commonMultiGames;

            return null;
        }

    #endregion

#endregion

    }
}
