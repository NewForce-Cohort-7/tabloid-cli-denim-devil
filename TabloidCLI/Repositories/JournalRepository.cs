using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabloidCLI.Models;
using TabloidCLI.Repositories;
using TabloidCLI;
using Microsoft.Data.SqlClient;

namespace TabloidCLI.Repositories
{
    public class JournalRepository : DatabaseConnector, IRepository<Journal>
    {
        public JournalRepository(string connectionString) : base(connectionString) { }

        public List<Journal> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT id,
                                               Title,
                                               TextContent,
                                          FROM Journal";

                    List<Journal> journals = new List<Journal>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Journal journal = new Journal()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                        };
                        journals.Add(journal);
                    }

                    reader.Close();

                    return journals;
                }

            }
        }
        public Journal Get(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(Journal journal)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Note (JournalId, Title, Content, CreateDateTime)
                                                    VALUES (@journalId, @title, @content, @createDateTime)";
                    cmd.Parameters.AddWithValue("@journalId", 1);
                    cmd.Parameters.AddWithValue("@title", journal.Title);
                    cmd.Parameters.AddWithValue("@content", journal.Content);
                    cmd.Parameters.AddWithValue("@createDateTime", DateTime.Now);

                    cmd.ExecuteNonQuery();
                }
            };
        }

        public void Update(Journal journal)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Journal 
                                           SET Title = @Title,
                                               content = @content
                                         WHERE id = @id";

                    cmd.Parameters.AddWithValue("@title", journal.Title);
                    cmd.Parameters.AddWithValue("@content", journal.Content);
                    cmd.Parameters.AddWithValue("@id", journal.Id);

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
                    cmd.CommandText = @"DELETE FROM Journal WHERE JournalId = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
