using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TabloidCLI.Models;

namespace TabloidCLI.UserInterfaceManagers
{
    public class JournalManager : IUserInterfaceManager
    {
        //these are just us declaring variables to use later on in the code below
        private readonly IUserInterfaceManager _parentUI;
        private JournalRepository _journalRepository;
        private string _connectionString;

        public JournalManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _journalRepository = new JournalRepository(connectionString);
            _connectionString = connectionString;
        }

        public IUserInterfaceManager Execute()
        {
            //switch case that creates a menu for user
            Console.WriteLine("Journal Menu");
            Console.WriteLine(" 1) List Journals");
            Console.WriteLine(" 2) Add Journal");
            Console.WriteLine(" 3) Edit Journal");
            Console.WriteLine(" 4) Remove Journal");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    List();
                    return this;
                case "2":
                    Add();
                    return this;
                case "3":
                    Edit();
                    return this;
                case "4":
                    Remove();
                    return this;
                case "0":
                    //you could see the _parentUI as sort of the "Main Menu" for the app. It brings us back to the parent menu which happens to show the MainMenuManager
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }
        //We now create new methods to use our methods in our JournalRepository.cs to specifically run them when we choose the appropriate menu selection from our switch case above. 1 returns the method List(), 2 returns the method Add(), etc.
        private void List()
        {
            List<Journal> journals = _journalRepository.GetAll();
            foreach (Journal journal in journals)
            {
                Console.WriteLine(journal);
            }
            Console.WriteLine();
        }
        private Journal Choose(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose a Journal Entry";
            }

            Console.WriteLine(prompt);

            List<Journal> journals = _journalRepository.GetAll();
            // using a for loop and listing all of the journals to pick from is a much better way of letting someone select an item from a list than just searching it by Id.
            for (int i = 0; i < journals.Count; i++)
            {
                Journal journal = journals[i];
                //We add 1 to the index so it starts a list with 1 instead of 0. It makes more sense to humans to start at 1
                Console.WriteLine($" {i + 1}) {journal.Title}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                //when we eventually choose an entry, we -1 from the choice so the smoke and mirrors of a list starting at 1 doesnt affect the choice the user makes.
                return journals[choice - 1];
            }
            catch (Exception ex)
            {
                //If the user doesnt enter a valid choice in, throw an exception and dont return anything
                Console.WriteLine("Invalid selection");
                return null;
            }
        }
        private void Add()
        {
            Console.WriteLine("New Journal Entry");
            Journal journal = new Journal();

            Console.Write("Title: ");
            journal.Title = Console.ReadLine();

            Console.Write("Insert Journal Entry: ");
            journal.Content = Console.ReadLine();

            journal.CreateDateTime = DateTime.Now;

            _journalRepository.Insert(journal);
        }

        private void Edit()
        {
            Journal journalToEdit = Choose("Which Journal Entry would you like to edit?");
            if (journalToEdit == null)
            {
                return;
            }

            Console.WriteLine();
            Console.Write("New Title (Leave blank to keep current title): ");
            string title = Console.ReadLine();
            if (!string.IsNullOrEmpty(title))
            {
                journalToEdit.Title = title;    
            }
            Console.Write("Edit the entry (Leave blank to keep current entry): ");
            string content = Console.ReadLine();
            if (!string.IsNullOrEmpty(content))
            {
                journalToEdit.Content = content;
            }
            
            _journalRepository.Update(journalToEdit);
        }

        private void Remove()
        {
            Journal journalToDelete = Choose("Which journal entry would you like to delete?");
            if (journalToDelete != null)
            {
                _journalRepository.Delete(journalToDelete.Id);
                Console.WriteLine("");
                Console.WriteLine($"{journalToDelete.Title} Journal Entry has been deleted!");
                Console.WriteLine("");
            }
        }
    }
}
