using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System;


public class DBDataFetcher : MonoBehaviour {

    private string conn;
    private Text scorboardText;

	// Use this for initialization
	void Start () {
		conn = "URI=file:" + Application.dataPath + "/highscores.db";
        Debug.Log(conn);
        scorboardText = GameObject.Find("ScoreboardText").GetComponent<Text>();
        scorboardText.text = GetSQLScores(15);
	}
	
	// Update is called once per frame
	void Update () {
		
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