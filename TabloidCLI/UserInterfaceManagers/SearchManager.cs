﻿using System;
using System.ComponentModel.Design;
using TabloidCLI.Models;

namespace TabloidCLI.UserInterfaceManagers
{
    internal class SearchManager : IUserInterfaceManager
    {
        private IUserInterfaceManager _parentUI;
        private TagRepository _tagRepository;

        public SearchManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _tagRepository = new TagRepository(connectionString);
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Search Menu");
            Console.WriteLine(" 1) Search Blogs");
            Console.WriteLine(" 2) Search Authors");
            Console.WriteLine(" 3) Search Posts");
            Console.WriteLine(" 4) Search All");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    SearchBlogs();
                    return this;
                case "2":
                    SearchAuthors();
                    return this;
                case "3":
                    SearchPosts();
                    return this;
                case "4":
                    SearchAll();
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        private void SearchAuthors()
        {
            Console.Write("Tag> ");
            string tagName = Console.ReadLine();

            SearchResults<Author> results = _tagRepository.SearchAuthors(tagName);

            if (results.NoResultsFound)
            {
                Console.WriteLine($"No results for {tagName}");
            }
            else
            {
                results.Display();
            }
        }
        private void SearchBlogs()
        {
            Console.Write("Tag> ");
            string tagName = Console.ReadLine();

            SearchResults<Blog> results = _tagRepository.SearchBlogs(tagName);

            if (results.NoResultsFound)
            {
                Console.WriteLine($"No results for {tagName}");
            }
            else
            {
                results.Display();
            }
        }
        private void SearchPosts()
        {
            Console.Write("Tag> ");
            string tagName = Console.ReadLine();

            SearchResults<Post> results = _tagRepository.SearchPosts(tagName);

            if (results.NoResultsFound)
            {
                Console.WriteLine($"No results for {tagName}");

            }
            else
            {
                results.Display();
            }
        }
        private void SearchAll()
        {
            Console.Write("Tag> ");
            string tagName = Console.ReadLine();

            SearchResults<Post> postResults = _tagRepository.SearchPosts(tagName);
            SearchResults<Author> authorResults = _tagRepository.SearchAuthors(tagName);
            SearchResults<Blog> blogResults = _tagRepository.SearchBlogs(tagName);
            if (postResults.NoResultsFound && authorResults.NoResultsFound && blogResults.NoResultsFound)
            {
                Console.WriteLine($"No results for {tagName}");
            }
            else
            {
                if (!postResults.NoResultsFound)
                {
                    Console.WriteLine("*-----------* Posts with matching Tag *-----------*");
                    Console.WriteLine("");
                    postResults.Display();
                    Console.WriteLine("");

                }
                if (!authorResults.NoResultsFound)
                {
                    Console.WriteLine("*----------* Authors with matching Tag *----------*");
                    Console.WriteLine("");
                    authorResults.Display();
                    Console.WriteLine("");

                }
                if (!blogResults.NoResultsFound)
                {
                    Console.WriteLine("*-----------* Blogs with matching Tag *-----------*");
                    Console.WriteLine("");
                    blogResults.Display();
                    Console.WriteLine("");
                }
            }
        }
    }
}