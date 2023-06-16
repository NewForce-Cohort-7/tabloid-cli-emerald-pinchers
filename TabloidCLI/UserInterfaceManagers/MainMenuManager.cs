using System;

namespace TabloidCLI.UserInterfaceManagers
{
    public class MainMenuManager : IUserInterfaceManager
    {
        private const string CONNECTION_STRING =
            @"Data Source=localhost\SQLEXPRESS;Database=TabloidCLI;Integrated Security=True; TrustServerCertificate=True";

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Pleasant Greetings");
            Console.WriteLine("                         ,.---.                                                                 \r\n               ,,,,     /    _ `.                                                              \r\n                \\\\\\\\   /      \\  )                                                          \r\n                 |||| /\\/``-.__\\/                                                                 \r\n                 ::::/\\/_                   _________                                                    \r\n {{`-.__.-'(`(^^(^^^(^ 9 `.========='    _ /_|_____|_\\ _                                    \r\n{{{{{{ { ( ( (  (   (-----:=               '. \\   / .'               \r\n {{.-'~~'-.(,(,,(,,,(__6_.'=========.        '.\\ /.'           \r\n                 ::::\\/\\                       '.'      \r\n                 |||| \\/\\  ,-'/\\                     \r\nEmerald         ////   \\ `` _/  )                                   \r\n Pinchers      ''''     \\  `   /                                \r\n                         `---''");
            Console.WriteLine("Main Menu");

            Console.WriteLine(" 1) Journal Management");
            Console.WriteLine(" 2) Blog Management");
            Console.WriteLine(" 3) Author Management");
            Console.WriteLine(" 4) Post Management");
            Console.WriteLine(" 5) Tag Management");
            Console.WriteLine(" 6) Search by Tag");
            Console.WriteLine(" 0) Exit");

            Console.Write(">Choose an option: ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1": throw new NotImplementedException();
                case "2": return new BlogManager(this, CONNECTION_STRING);
                case "3": return new AuthorManager(this, CONNECTION_STRING);
                case "4": throw new NotImplementedException();
                case "5": return new TagManager(this, CONNECTION_STRING);
                case "6": return new SearchManager(this, CONNECTION_STRING);
                case "0":
                    Console.WriteLine("Good bye");
                    return null;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }
    }
}
