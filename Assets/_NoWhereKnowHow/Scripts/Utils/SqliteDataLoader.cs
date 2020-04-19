using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System.Collections;
using System.Collections.Generic; 
using CodeNames;

namespace CodeNames
{

    public class SqliteDataLoader : MonoBehaviour
    {
        [SerializeField]
        public bool LoadVocabulary = false;
        [SerializeField]
        public bool LoadKeys = false;

        // [SerializeField]
        // public bool ReadVocabulary = false;

        // Start is called before the first frame update
        void Start(){
            if(LoadVocabulary){
                LoadVocab();
            }
            // if(ReadVocabulary){
            //     ReadVocab();
            // }
            if(LoadKeys){
                LoadKeyFiles();
            }
        }

        // void ReadVocab(){
        //     List<string> vocab = new List<string>();

        //     // Connect to database
        //     string connection = "URI=file:" + Path.Combine(new string[] {Application.dataPath, "Data", "codenames.db"});
        //     IDbConnection dbcon = new SqliteConnection(connection);
        //     dbcon.Open();

        //     IDbCommand selectCmd = dbcon.CreateCommand();
        //     IDataReader selectReader;
        //     string q_select = "select * from vocab";
        //     selectCmd.CommandText = q_select;
        //     selectReader = selectCmd.ExecuteReader();
        //     while(selectReader.Read()){
        //         Debug.Log(selectReader.ToString());
        //     }
            

        // }

        void LoadVocab(){
            // List<string> vocab = new List<string>();

            // Connect to database
            string connection = "URI=file:" + Path.Combine(new string[] {Application.dataPath, "Data", "codenames.db"});
            IDbConnection dbcon = new SqliteConnection(connection);
            dbcon.Open();

            // Drop and create table
            IDbCommand dropCmd = dbcon.CreateCommand();
            string q_dropTable = "DROP TABLE IF EXISTS vocab";
            dropCmd.CommandText = q_dropTable;
            dropCmd.ExecuteReader();

            IDbCommand createCmd = dbcon.CreateCommand();
            string q_createTable = "CREATE TABLE vocab (vocab_text TEXT NOT NULL, source TEXT NOT NULL, PRIMARY KEY(vocab_text, source))";
            createCmd.CommandText = q_createTable;
            createCmd.ExecuteReader();

            using(var fileReader = new StreamReader(Path.Combine(new string[] {Application.dataPath, "Data", "Codenames.csv"})))
            {
                while (!fileReader.EndOfStream)
                {
                    string line = fileReader.ReadLine();
                    IDbCommand cmnd = dbcon.CreateCommand();
                    cmnd.CommandText = "INSERT INTO vocab (vocab_text, source) VALUES ('" + line.Trim() + "', 'codenames');";
                    Debug.Log(cmnd.CommandText);
                    cmnd.ExecuteNonQuery();

                }
            }
        
            using(var fileReader = new StreamReader(Path.Combine(new string[] {Application.dataPath, "Data", "Undercover.csv"})))
            {
                while (!fileReader.EndOfStream)
                {
                    string line = fileReader.ReadLine();
                    IDbCommand cmnd = dbcon.CreateCommand();
                    cmnd.CommandText = "INSERT INTO vocab (vocab_text, source) VALUES ('" + line.Trim() + "', 'undercover');";
                    Debug.Log(cmnd.CommandText);
                    cmnd.ExecuteNonQuery();

                }
            }

            using(var fileReader = new StreamReader(Path.Combine(new string[] {Application.dataPath, "Data", "Duet.csv"})))
            {
                while (!fileReader.EndOfStream)
                {
                    string line = fileReader.ReadLine();
                    IDbCommand cmnd = dbcon.CreateCommand();
                    cmnd.CommandText = "INSERT INTO vocab (vocab_text, source) VALUES ('" + line.Trim() + "', 'duet');";
                    Debug.Log(cmnd.CommandText);
                    cmnd.ExecuteNonQuery();

                }
            }

        }

        void LoadKeyFiles(){
            string[] files = Directory.GetFiles(Path.Combine(new string[] {Application.dataPath, "Data", "keys"}));

            // Connect to database
            string connection = "URI=file:" + Path.Combine(new string[] {Application.dataPath, "Data", "codenames.db"});
            IDbConnection dbcon = new SqliteConnection(connection);
            dbcon.Open();

            // Drop and create table
            IDbCommand dropCmd = dbcon.CreateCommand();
            string q_dropTable = "DROP TABLE IF EXISTS keys";
            dropCmd.CommandText = q_dropTable;
            dropCmd.ExecuteReader();

            IDbCommand createCmd = dbcon.CreateCommand();
            string q_createTable = "CREATE TABLE keys (id INTEGER PRIMARY KEY, firstToMove INTEGER, data TEXT, redCount INTEGER, blueCount INTEGER, blackCount INTEGER, size INTEGER)";
            createCmd.CommandText = q_createTable;
            createCmd.ExecuteReader();

            KeyCard _keyCard = new KeyCard();
            foreach(string file in files){
                if(file.Contains(".meta")){
                    continue;
                }

                _keyCard = JsonUtility.FromJson<KeyCard>(File.ReadAllText(Path.Combine(new string[] {Application.dataPath, "Data", "keys", file})));
                IDbCommand cmnd = dbcon.CreateCommand();
                cmnd.CommandText = $"INSERT INTO keys (id, firstToMove, data, size) VALUES ({_keyCard.id}, {_keyCard.firstToMove}, '{string.Join(", ", _keyCard.data)}', {_keyCard.size});";
                cmnd.ExecuteNonQuery();
            }
        }
    }
} 