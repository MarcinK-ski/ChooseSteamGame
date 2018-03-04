using System;

namespace playSteam
{
    class Program
    {
        static void Main(string[] args)
        {

#if (DEBUG)
            Console.WriteLine("START\n\n\n");
            Steam game = new Steam();
#else
            Console.Write("Enter UserID: ");
            string uid = Console.ReadLine();
            Console.Write("Enter APIKey: ");
            string apiKey = Console.ReadLine();
            Console.WriteLine("\n\n");
            Steam game = new Steam(uid, apiKey);
#endif

            string userInfo = game.getMyUserInfo();
            if (userInfo == null)
                Console.WriteLine("Brak informacji o userze");
            else
                Console.WriteLine("\nLosowanie gry dla usera z następującymi danymi: \n" + userInfo);

            Console.WriteLine("\n\n");

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
