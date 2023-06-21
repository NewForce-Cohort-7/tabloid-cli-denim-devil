using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using TabloidCLI.Models;

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
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Title, Content, CreateDateTime FROM Note WHERE PostId = @postId";
                    cmd.Parameters.AddWithValue("@postId", _postId);                
                    List<Note> notes = new List<Note>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Note note = new Note()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                        };
                        notes.Add(note);
                    }
                    reader.Close();

                    return notes;
                }
            }
        }

        public Note Get(int id) { return null; }
        //{
            //using (SqlConnection conn = Connection)
            //{
            //    conn.Open();
            //    using (SqlCommand cmd = conn.CreateCommand())
            //    {
            //        cmd.CommandText = @"SELECT n.Id, n.Title, n.Content, n.CreateDateTime, p.Title AS PostTitle
            //                            FROM Note n
            //                            JOIN Post p ON n.PostId = p.Id;
            //                            ";
            //        cmd.Parameters.AddWithValue("@id", id);

            //        Note note = null;

            //        SqlDataReader reader = cmd.ExecuteReader();
            //        while (reader.Read())
            //        {
            //            if (note == null)
            //            {
            //                note = new Note()
            //                {
            //                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
            //                    Title = reader.GetString(reader.GetOrdinal("Title")),
            //                    Content = reader.GetString(reader.GetOrdinal("Content")),
            //                    CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
            //                    Post = new Post()
            //                    {
            //                        Title = reader.GetString(reader.GetOrdinal("PostTitle"))
            //                    }
            //                };
            //            }
            //        }

            //        reader.Close();
            //        return note;
            //    }
            //}
        //}

        public void Insert(Note note)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Note (Title, Content, PostId, CreateDateTime)
                                                    VALUES (@title, @content, @postId, @createDateTime)";
                    cmd.Parameters.AddWithValue("@title", note.Title);
                    cmd.Parameters.AddWithValue("@content", note.Content);
                    cmd.Parameters.AddWithValue("@postId", _postId);
                    cmd.Parameters.AddWithValue("@createDateTime", DateTime.Now);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Update(Note note)
        {}

        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Note WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}