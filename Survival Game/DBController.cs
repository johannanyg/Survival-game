using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;

public class DBController : MonoBehaviour{

    private string conn;

    /// <summary>
    /// Create the string variable for the file path to the database.
    /// </summary>
    public void Start() {
        conn = "URI=file:" + Application.dataPath + "/highscores.db";
        Debug.Log(conn);
        CreateTable();
    }

    /// <summary>
    /// Create a table in the game's database if it doesn't already exist.
    /// </summary>
    public IEnumerable CreateTable() {
        using (IDbConnection dbconn = new SqliteConnection(conn)) {
            dbconn.Open();
            using (IDbCommand dbCmd = dbconn.CreateCommand()) {
                Debug.Log("Creating table");
                string sqlQuery = String.Format("CREATE TABLE IF NOT EXISTS scores " + 
                        "(id INTEGER PRIMARY KEY AUTOINCREMENT, name VARCHAR(20), score INT)");
                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteScalar();
                dbconn.Close();
            }
        }
        return null;
    }
    
    /// <summary>
    /// Send and SQL command to add player in to the leaderboard.
    /// </summary>
    /// <param name="nameIn">Name to input in to the leaderboard.</param>
    /// <param name="scoreIn">Score to input in to the leaderboard.</param>
    public IEnumerable SendSQLCommand(string nameIn, int scoreIn) {
        using (IDbConnection dbconn = new SqliteConnection(conn)) {
            dbconn.Open();
            using (IDbCommand dbCmd = dbconn.CreateCommand()) {
                string sqlQuery = String.Format("INSERT INTO scores (name, score) " +
                        "VALUES (\"" + nameIn + "\", " + scoreIn +")");
                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteScalar();
                dbconn.Close();
            }
        }
        return null;
    }
    /// <summary>
    /// Get the first n amount of rows from the score database, add them to a 
    /// single string and return it.
    /// </summary>
    /// <param name="limit">Limit the amount of scores to be shown.</param>
    public string GetSQLScores(int limit) {
        string temp = "";
        using (var dbconn = new SqliteConnection(conn)) {
            dbconn.Open();
            using (var dbCmd = dbconn.CreateCommand()) {
                dbCmd.CommandType = CommandType.Text;
                dbCmd.CommandText = "SELECT * FROM scores ORDER BY scores.score DESC LIMIT @Count;";

                dbCmd.Parameters.Add(new SqliteParameter {
                    ParameterName = "Count",
                    Value = limit,
                });

                int positionInBoard = 0;
                var reader = dbCmd.ExecuteReader();
                while (reader.Read()) {
                    positionInBoard++;
                    var name = reader.GetString(1);
                    var score = reader.GetInt32(2);
                    temp +=  string.Format("#{0} {1}, {2} seconds", positionInBoard, name, score);
                    temp += "\n";
                }
            }
        }
        return temp;
    }
}
