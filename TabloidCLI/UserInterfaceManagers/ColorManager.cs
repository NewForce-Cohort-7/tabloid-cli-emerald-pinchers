﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabloidCLI.Models;

namespace TabloidCLI.UserInterfaceManagers
{
    public class ColorManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;

        public ColorManager(IUserInterfaceManager parentUI)
        {
            _parentUI = parentUI;
        }

        public IUserInterfaceManager Execute()
        {
            Random rnd = new Random();

            Console.WriteLine("Choose a Background Color");
            Console.WriteLine(" 1) Red");
            Console.WriteLine(" 2) Blue");
            Console.WriteLine(" 3) WOO");
            Console.WriteLine(" 4) YUCK");
            Console.WriteLine(" 5) BLINDING");
            Console.WriteLine(" 6) Default");

            Console.Write("> ");
            string choice = Console.ReadLine(); // read the users input
            switch (choice)
            {
                case "1":
                    Console.BackgroundColor = ConsoleColor.Red;// set background color to red
                    Console.Clear(); // clear the console 
                    return _parentUI; // return the parent user interface manager
                case "2":
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Clear();
                    return _parentUI;
                case "3":
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Clear();
                    return _parentUI;
                case "4":
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Clear();
                    return _parentUI;
                case "5":
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Clear();
                    return _parentUI;
                case "6":
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Clear();
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }
    }
}
