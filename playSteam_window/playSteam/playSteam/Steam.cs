using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace playSteam
{
    class Steam
    {

#region constForEasierDebuging

        const string DEFAULT_UID = "XXXXXXXXXXXXXXXXXXXXXX";
        const string DEFAULT_API_KEY = "XXXXXXXXXXXXXXXXXXXXXX";

#endregion


#region properties

        protected readonly string _apiKey;
        protected const string API_URL = "http://api.steampowered.com/";
        protected string settingsUri;

#endregion


#region get/set

        public string UserID { get; set; }
        public string M8UID { get; set; } = "";

#endregion


#region constructors

        public Steam() : this(DEFAULT_UID, DEFAULT_API_KEY)
        {
        }

        public Steam(string uid, string apiKey, string settingsUri = Helper.DEFAULT_LAST_SETTINGS_XML) 
        {
            this.UserID = !string.IsNullOrEmpty(uid) ? uid : DEFAULT_UID;

            this._apiKey = !string.IsNullOrEmpty(apiKey) ? apiKey : DEFAULT_API_KEY;

            this.settingsUri = settingsUri;
        }

#endregion


#region methods

        /*
         * Generate UID from CustomUID
         */
        public bool? generateUID(bool isM8 = false)
        {
            string customID = isM8 ? this.M8UID : this.UserID;  //Using function to generate UID for mate or user

            if (customID == "")
                return null;

            string url = $"{API_URL}ISteamUser/ResolveVanityURL/v0001/?key={this._apiKey}&vanityurl={customID}&format=xml";
            XElement xel = Helper.xRead(url);
            if (xel == null)
                return null;

            if (xel.Element("success")?.Value != "1")   //Correct result, should has "success" set to "1", other states are errors
                return false;

            string resultAsID = xel.Element("steamid").Value;

            if (isM8)
                this.M8UID = resultAsID;
            else
                this.UserID = resultAsID;


            Helper.xSettingsSave(this.UserID, this._apiKey, this.M8UID, this.settingsUri);

            return true;
        }

        /*
         * Get XElement's array with user info
         */
        protected XElement getMyUserInfo(string uid = null, bool m8 = false)
        {
            if (m8 && (uid == "" || uid == null))
                return null;


            if (uid == null || uid == "")   //if given UID is empty, we should use last known UID
                uid = this.UserID;
            

            string url = $"{API_URL}ISteamUser/GetPlayerSummaries/v0002/?key={this._apiKey}&steamids={uid}&format=xml";
            XElement xelement = Helper.xRead(url);
            if (xelement == null)
                return null;

            XElement[] usr = xelement.Elements("players")?.Elements("player")?.ToArray();   //Get array with User info
            if (usr.Length < 1)
                return null;
            

            return usr[0];
        }

        /*
         * Get basic user info as string (method created for console version)
         */
        public string userInfoToString()
        {
            XElement usr = this.getMyUserInfo();
            if (usr == null)
                return null;

            StringBuilder userInfo = new StringBuilder();

            userInfo.AppendLine("\tNick: " + usr.Element("personaname")?.Value);
            userInfo.AppendLine("\tReal name: " + usr.Element("realname")?.Value);
            userInfo.AppendLine("\tCountry code: " + usr.Element("loccountrycode")?.Value);

            return userInfo.ToString();
        }

        /*
         * Get user's or mate's XElement's array 
         */
        protected XElement[] getUserGameXArray(bool forUser = true)
        {
            string forWhomID;
            if (forUser || this.M8UID == "")
                forWhomID = this.UserID;
            else
                forWhomID = this.M8UID;

            string url = $"{API_URL}IPlayerService/GetOwnedGames/v0001/?key={this._apiKey}&steamid={forWhomID}&format=xml&include_appinfo=1";
            XElement xelement = Helper.xRead(url);
            if (xelement == null)
                return null;

            XElement[] allGames = xelement.Elements("games")?.Elements("message")?.ToArray();
            if (allGames == null)
                return null;

            return allGames;
        }

    #region oneUserGames

        /*
         * Get user's games
         */
        protected List<string> getGames()
        {
            XElement[] allGames = getUserGameXArray();

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

    #endregion

#endregion

    }
}
