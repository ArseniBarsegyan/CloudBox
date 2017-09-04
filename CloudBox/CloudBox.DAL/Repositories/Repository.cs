using System;
using System.Collections.Generic;
using System.Data;
using CloudBox.DAL.Interfaces;
using MySql.Data.MySqlClient;

namespace CloudBox.DAL.Repositories
{
    public class Repository : IRepository
    {
        private readonly string _connectionString;

        public Repository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ICollection<string> GetAllFilesByUserId(int id)
        {
            ICollection<string> resultList = new List<string>();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("GetAllFilesById", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    resultList.Add(reader[0] + " " + reader[1] + " " + reader[2] + " " + reader[3] + " " + reader[4]);
                }
                reader.Close();
                return resultList;
            }
        }

        public void WriteDataToDataBase(int userId, string fileName, byte[] data, DateTime uploadTime)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("WriteDataToDataBase", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@userID", userId);
                command.Parameters.AddWithValue("@fileName", fileName);
                command.Parameters.AddWithValue("@data", data);
                command.Parameters.AddWithValue("@uploadDate", uploadTime);

                command.ExecuteNonQuery();
            }
        }

        public void RenameFileInDataBase(int userId, string oldFileName, string newFileName)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("RenameFileInDataBase", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@userID", userId);
                command.Parameters.AddWithValue("@oldName", oldFileName);
                command.Parameters.AddWithValue("@newName", newFileName);

                command.ExecuteNonQuery();
            }
        }
    }
}