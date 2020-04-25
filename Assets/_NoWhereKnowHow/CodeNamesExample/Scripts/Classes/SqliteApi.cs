using UnityEngine;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using CodeNames;

namespace CodeNames
{

    public class SqliteApi
    {
        protected SqliteApi() { }

        public static string connectionString = "URI=file:" + Path.Combine(new string[] { Application.dataPath, "Data", "codenames.db" });


        public static KeyCard GetRandomKeyCard()
        {
            Debug.Log(connectionString);
            IDbConnection dbcon = new SqliteConnection(connectionString);
            dbcon.Open();
            IDbCommand dbcmd = dbcon.CreateCommand();
            dbcmd.CommandText = Constants.GET_RANDOM_KEY_CARD;
            IDataReader reader = dbcmd.ExecuteReader();

            KeyCard keyCard = null;
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                int firstToMove = reader.GetInt32(1);
                string data = reader.GetString(2);
                int size = reader.GetInt32(3);
                keyCard = new KeyCard(id, firstToMove, data, size);
            }

            reader.Dispose();
            dbcmd.Dispose();
            dbcon.Close();



            return keyCard;
        }

        public static List<string> GetRandomWords()
        {
            IDbConnection dbcon = new SqliteConnection(connectionString);
            dbcon.Open();
            IDbCommand dbcmd = dbcon.CreateCommand();
            dbcmd.CommandText = Constants.GET_RANDOM_WORDS_SQL;
            IDataReader reader = dbcmd.ExecuteReader();

            List<string> words = new List<string>();
            while (reader.Read())
            {
                string word = reader.GetString(0);
                words.Add(word);
            }

            reader.Dispose();
            dbcmd.Dispose();
            dbcon.Close();

            return words;
        }

    }
}