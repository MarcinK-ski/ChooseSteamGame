using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace playSteam
{
    class Steam
    {
        const string DEFAULT_UID = "XXXXXXXXXXXXXXXXXXXXXX";
        const string DEFAULT_API_KEY = "XXXXXXXXXXXXXXXXXXXXXX";

        readonly string _apiKey;
        const string API_URL = "http://api.steampowered.com/";
        string UserID { get; set; }

        public Steam() : this(DEFAULT_UID, DEFAULT_API_KEY)
        {
        }

        public Steam(string uid, string apiKey) 
        {
            this.UserID = !string.IsNullOrEmpty(uid) ? uid : DEFAULT_UID;

            this._apiKey = !string.IsNullOrEmpty(apiKey) ? apiKey : DEFAULT_API_KEY;
        }

        /*
         * Method to load XML
         */
        private XElement xRead(string url)
        {
            XElement xelement;
            try
            {
                xelement = XElement.Load(url);
                return xelement;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Wyjatek przy xRead (czytanie XML): " + ex.Message);
                return null;
            }
        }

        /*
         * Get basic user info
         */
        public string getMyUserInfo(string uid = null)
        {
            if (uid == null || uid == "")
                uid = this.UserID;

            string url = $"{API_URL}ISteamUser/GetPlayerSummaries/v0002/?key={this._apiKey}&steamids={uid}&format=xml";
            XElement xelement = xRead(url);
            if (xelement == null)
                return null;

            XElement[] usr = xelement.Elements("players")?.Elements("player")?.ToArray();
            if (usr == null || usr.Length < 1)
                return null;

            StringBuilder userInfo = new StringBuilder();
            
            userInfo.AppendLine("\tNick: " + usr[0].Element("personaname")?.Value);
            userInfo.AppendLine("\tReal name: " + usr[0].Element("realname")?.Value);
            userInfo.AppendLine("\tCountry code: " + usr[0].Element("loccountrycode")?.Value);


            return userInfo.ToString();
        }

        /*
         * Get user's games
         */
        private List<string> getGames()
        {
            string url = $"{API_URL}IPlayerService/GetOwnedGames/v0001/?key={this._apiKey}&steamid={this.UserID}&format=xml&include_appinfo=1";
            XElement xelement = xRead(url);
            if (xelement == null)
                return null;

            XElement[] allGames = xelement.Elements("games")?.Elements("message")?.ToArray();
            if (allGames == null)
                return null;

            List<string> games = new List<string>(allGames.Count());   //List with game titles

            for(int i = 0; i < allGames.Count(); i++)
            {
                string xel = allGames[i].Element("name")?.Value; //Get only game title from XML
                if(xel != null)
                    games.Add(xel);
            }

            if (games.Count < 1)
                return null;

            return games;
        }

        /*
         * Choose random game
         */
        public string rollGames()
        {
            List<string> games = getGames();
            if (games == null)
                return null;
            int countGames = games.Count;

            Random random = new Random((int) DateTime.Now.Ticks);
            int choosedGame = random.Next(0, countGames);

            return games[choosedGame];
        }
    }
}
