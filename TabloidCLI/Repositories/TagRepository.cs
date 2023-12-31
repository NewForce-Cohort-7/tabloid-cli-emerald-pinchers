﻿using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using TabloidCLI.Models;
using TabloidCLI.Repositories;
using TabloidCLI.UserInterfaceManagers;

namespace TabloidCLI
{
    public class TagRepository : DatabaseConnector, IRepository<Tag>
    {
        public TagRepository(string connectionString) : base(connectionString) { }

        public List<Tag> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT id, Name FROM Tag";
                    List<Tag> tags = new List<Tag>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Tag tag = new Tag() // create new tag object and populate its properties
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                        };
                        tags.Add(tag); // add new tag to the list
                    }

                    reader.Close();

                    return tags;
                }
            }
        }

        public Tag Get(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(Tag tag)
        {
            using (SqlConnection conn = Connection)
                {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Tag (Name)
                                        OUTPUT INSERTED.Id
                                        VALUES (@name)"; // query to insert new tag
                    cmd.Parameters.AddWithValue("@name", tag.Name); // set parameter value for tag name
                    int id = (int)cmd.ExecuteScalar();

                    tag.Id = id;
                }
            }
        }

        public void Update(Tag tag)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Tag
                                        SET Name = @name
                                        WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@name", tag.Name);
                    cmd.Parameters.AddWithValue("@id", tag.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Tag 
                                        WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public SearchResults<Author> SearchAuthors(string tagName)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT a.id,
                                               a.FirstName,
                                               a.LastName,
                                               a.Bio
                                          FROM Author a
                                               LEFT JOIN AuthorTag at on a.Id = at.AuthorId
                                               LEFT JOIN Tag t on t.Id = at.TagId
                                         WHERE t.Name LIKE @name"; // sql query to search authors by tag name
                    cmd.Parameters.AddWithValue("@name", $"%{tagName}%");// set parameter value for tag name
                    SqlDataReader reader = cmd.ExecuteReader();

                    SearchResults<Author> results = new SearchResults<Author>(); // create new SearchResults object for authors
                    while (reader.Read())
                    {
                        Author author = new Author() // create new author and populate its properties
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            Bio = reader.GetString(reader.GetOrdinal("Bio")),
                        };
                        results.Add(author); // add the author to the search results
                    }

                    reader.Close();

                    return results;
                }
            }
        }
        public SearchResults<Post> SearchPosts(string tagName) 
        { 
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                        {
                    cmd.CommandText = @"SELECT p.Id,
                                               p.Title,
                                               p.Url,
                                               p.PublishDateTime,
                                               t.Name
                                          FROM Post p
                                               JOIN PostTag pt on p.Id = pt.PostId
                                               JOIN Tag t on t.Id = pt.TagId
                                          WHERE t.Name LIKE @name"; // query to search Posts by tag name
                    cmd.Parameters.AddWithValue("@name", $"%{tagName}%"); // set parameter value for tag name
                    SqlDataReader reader = cmd.ExecuteReader();

                    SearchResults<Post> results = new SearchResults<Post>();
                    while (reader.Read())
                    {
                        Post post = new Post()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Url = reader.GetString(reader.GetOrdinal("URL")),
                            PublishDateTime = reader.GetDateTime(reader.GetOrdinal("PublishDateTime"))
                        };
                        results.Add(post);
                    }
                    reader.Close();

                    return results;
                }
            }
        }

        public SearchResults<Blog> SearchBlogs(string tag)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $@"SELECT b.Id,
                                                b.Title,
                                                b.URL
                                                FROM Tag t
                                                JOIN BlogTag bt ON bt.TagId = t.Id
                                                JOIN Blog b ON b.Id = bt.BlogId
                                                WHERE t.Name LIKE @TAG";
                    cmd.Parameters.AddWithValue("@tag", $"%{tag}%");
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        SearchResults<Blog> results= new SearchResults<Blog>();
                        while (reader.Read())
                        {
                            Blog blog = new Blog
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Url = reader.GetString(reader.GetOrdinal("URL"))
                            };
                            results.Add(blog);
                        }
                        return results;
                    }

                }
            }
        }
    }
}
