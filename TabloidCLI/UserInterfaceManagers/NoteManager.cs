using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabloidCLI.Repositories;
using TabloidCLI.Models;

namespace TabloidCLI.UserInterfaceManagers
{
    public class NoteManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private NoteRepository _noteRepository;
        private string _connectionString;

        public NoteManager(IUserInterfaceManager parentUI, int postId, string connectionString)
        {
            _parentUI = parentUI;
            _connectionString = connectionString;
            _noteRepository = new NoteRepository(postId, connectionString);
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Note Menu");
            Console.WriteLine(" 1) List Notes");
            Console.WriteLine(" 2) Add Note");
            Console.WriteLine(" 3) Remove Note");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    List(); // call List() method to display list of notes
                    return this;
                case "2":
                    Add(); // add new note
                    return this;
                case "3":
                    Remove(); // remove note
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection"); // display message for an invalid selection
                    return this;
            }
        }

        private void List()
        {
            List<Note> notes = _noteRepository.GetAll(); // get all notes from note repo
            foreach (Note note in notes)
            {
                Console.WriteLine(note); // display each note
            }
        }
        private Note Choose(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose a Note:";
            }

            Console.WriteLine(prompt);

            List<Note> notes = _noteRepository.GetAll();

            for (int i = 0; i < notes.Count; i++)
            {
                Note note = notes[i];
                Console.WriteLine($" {i + 1}) {note}"); // display each note with its index
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return notes[choice - 1]; // return the chosen note based on users input
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid Selection");
                return null;
            }
        }
        private void Add()
        {
            Console.WriteLine("New Note");
            Note note = new Note();

            Console.Write("Title: ");
            note.Title = Console.ReadLine();

            Console.Write("Content: ");
            note.Content = Console.ReadLine();

            note.CreateDateTime = DateTime.Now;

            _noteRepository.Insert(note);
        }
        private void Remove()
        {
            Note noteToDelete = Choose("Which note would you like to remove?");
            if (noteToDelete != null)
            {
                _noteRepository.Delete(noteToDelete.Id);
            }
        }
    }
}
