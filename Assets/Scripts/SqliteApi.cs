using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class SqliteApi : Object
{
    protected SqliteApi() {}
    private static SqliteApi instance=null;

    public IDbConnection dbcon;

    public static SqliteApi Instance{
        get {
            if (SqliteApi.instance == null){
                SqliteApi.instance = new SqliteApi();
                string connection = "URI=file:" + Path.Combine(new string[] {Application.dataPath, "Data", "codenames_database"});
                SqliteApi.instance.dbcon = new SqliteConnection(connection);
                SqliteApi.instance.dbcon.Open();
            }
            return SqliteApi.instance;
        }
    }

    public void OnApplicationQuit(){
        SqliteApi.instance = null;
    }
}
