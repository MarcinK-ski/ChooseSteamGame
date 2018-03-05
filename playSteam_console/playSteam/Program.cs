using System;
using System.Xml.Linq;

namespace playSteam
{
    class Program
    {
        static void Main(string[] args)
        {

#if (DEBUG)
            Console.WriteLine("START\n\n\n");
#endif

            string lastApiKey = null;
            string lastUid = null;

            XElement xSavedSettings = Helper.xRead(Helper.DEFAULT_LAST_SETTINGS_XML);

            lastApiKey = xSavedSettings.Element("api")?.Value;
            lastUid = xSavedSettings.Element("uid")?.Value;

            Console.Write("Enter APIKey: ");
            string givenApiKey = Console.ReadLine();
            string apiKey = givenApiKey == "" ? lastApiKey : givenApiKey;

            Console.Write("Enter UserID: ");
            string givenUid = Console.ReadLine();
            string uid = givenUid == "" ? lastUid : givenUid;


            /* Save to file used apiKey + userID */
            if (!string.IsNullOrEmpty(givenApiKey))
            {
                Console.WriteLine("\n\nCzy chcesz zapisać podany apiKey oraz userID? [Y/N]");
                char key = (char)Console.Read();
                if(key == 'Y' || key == 'y')
                {
                    Console.WriteLine("Trwa zapis do pliku... \n");
                    Helper.xSettingsSave(uid, apiKey);
                    Console.WriteLine("Zapisano!\n");
                }
                else
                    Console.WriteLine("UWAGA! Nie zapisano do pliku, przez wybór użytkownika!");
            }

            Console.WriteLine("\n\n");
            Steam game = new Steam(uid, apiKey);

            /* Choosed steam user info */
            string userInfo = game.userInfoToString();
            if (userInfo == null)
                Console.WriteLine("Brak informacji o userze");
            else
                Console.WriteLine("\nLosowanie gry dla usera z następującymi danymi: \n" + userInfo);

            Console.WriteLine("\n\n");

            /* Random choose game */
            string title = game.rollGames();
            if(title == null)
                Console.WriteLine("Nie można wylosować żadnej gry (brak gier w bibliotece lub problem z UserID/APIKey)");
            else
                Console.WriteLine("\nWylosowana gra, to: \n\t==> " + title + " <===");


#if (DEBUG)
            Console.WriteLine("\n\n\nKONIEC");
#endif
            Console.ReadKey();
        }

    }
}
