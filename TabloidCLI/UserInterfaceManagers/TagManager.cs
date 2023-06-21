using System;
using System.Collections.Generic;
using TabloidCLI.Models;

namespace TabloidCLI.UserInterfaceManagers
{
    public class TagManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private TagRepository _tagRepository;
        private string _connectionString;

        public TagManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _connectionString = connectionString;
            _tagRepository = new TagRepository(connectionString);
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Tag Menu");
            Console.WriteLine(" 1) List Tags");
            Console.WriteLine(" 2) Add Tag");
            Console.WriteLine(" 3) Edit Tag");
            Console.WriteLine(" 4) Remove Tag");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    List(); // call list of tags
                    return this;
                case "2":
                    Add(); // add tag 
                    return this;
                case "3":
                    Edit(); // edit tag
                    return this;
                case "4":
                    Remove(); // remove tag
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        private void List()
        {
            List<Tag> tags = _tagRepository.GetAll(); // get all tags
            foreach (Tag tag in tags)
            {
                Console.WriteLine(tag); // display each tag
            }
        }

        private Tag Choose(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose an Author:";
            }

            Console.WriteLine(prompt);

            List<Tag> tags = _tagRepository.GetAll();

            for (int i = 0; i < tags.Count; i++)
            {
                Tag tag = tags[i];
                Console.WriteLine($"{i + 1}) {tag.Name}");
            }
            Console.Write(">");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return tags[choice - 1]; // return chosen tag based off users input
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Invalid Selection");
                return null;
            }
        }

        private void Add()
        {
            Console.WriteLine("Add Tag");
            Tag tag = new Tag();

            Console.Write("Tag Name ");
            tag.Name = Console.ReadLine(); // read the tag name from the user

            _tagRepository.Insert(tag); // insert new tag in tag repo
        }

        private void Edit()
        {
            Tag tagToEdit = Choose("Choose tag to edit: "); // prompt user to choose a tag to edit 
            if (tagToEdit == null)
            {
                return;
            }
            Console.WriteLine();
            Console.Write("New tag name (Leave blank to cancel): ");
            string name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
            {
                tagToEdit.Name = name; // update the tag 
            }

            _tagRepository.Update(tagToEdit); // update the tag in the tag repo
        }

        private void Remove()
        {
            Tag tagToDelete = Choose("Choose tag to delete: ");
            if (tagToDelete == null)
            {
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"Delete {tagToDelete.Name}? (Y/N)");
            string response = Console.ReadLine().ToLower();
            if (response == "y")
            {
                try
                {
                    _tagRepository.Delete(tagToDelete.Id); // delete tag from tag repo
                    Console.WriteLine($"{tagToDelete.Name} deleted.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Can't delete tags related to posts, blogs or authors.");
                }
            }
            else if (response == "n")
            {
                Console.WriteLine("Delete has been Canceled.");
                return;
            }
            else if (string.IsNullOrWhiteSpace(response))
            {
                Console.WriteLine("Delete has been Canceled.");
                return;
            }
            Console.WriteLine("Press any key to return to menu...:");
            Console.ReadKey();
        }
    }
}
