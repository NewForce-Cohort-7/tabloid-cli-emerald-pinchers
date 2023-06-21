using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.Repositories
{
    public class NoteRepository : DatabaseConnector, IRepository<Note>
    {
        private int _postId;
        public NoteRepository(int postId, string connectionString) : base(connectionString)
        {
            _postId = postId;
        }

        public List<Note> GetAll()
        {
            // establish connection to database
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id,
                                               Title,
                                               Content,
                                               CreateDateTime
                                        FROM Note
                                        WHERE PostId = @postId"; // query to retrieve notes with specific PostId
                    cmd.Parameters.AddWithValue("@postId", _postId); // set parameter value for PostId
                    SqlDataReader reader = cmd.ExecuteReader(); // execute to sql command and retrieve a data reader
                    List<Note> notes = new List<Note>(); // create a list to store notes

                    while (reader.Read())
                    {
                        Note note = new Note()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime"))
                        };
                        notes.Add(note); // add note to list
                    }

                    reader.Close(); // close data reader

                    return notes; // return list of notes
                }
            }
        }

        public Note Get(int id)
        {
            return null; // placeholder method, not implemented
        }

        public void Insert(Note note)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand()) // create new sql command object
                {
                    cmd.CommandText = @"INSERT INTO Note (Title, Content, CreateDateTime, PostId)
                                        VALUES (@title, @content, @createDateTime, @postId)"; // query to insert a new note
                    cmd.Parameters.AddWithValue("@title", note.Title); // set paramters for note properties
                    cmd.Parameters.AddWithValue("@content", note.Content);
                    cmd.Parameters.AddWithValue("@createDateTime", note.CreateDateTime);
                    cmd.Parameters.AddWithValue("@postId", _postId);

                    cmd.ExecuteNonQuery();
                }    
            }
        }

        public void Update(Note note)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Note" +
                        "              WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
