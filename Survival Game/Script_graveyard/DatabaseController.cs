using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System;
using UnityEngine.SceneManagement;

public class DatabaseController : MonoBehaviour {

    // For score input dialogue
    // private InputField inputField;
    private Button inputButton;
    private Text inputText;
    private Text secondText;
    private Text topScoreListText;
    private GameObject scoreInputWindow;

    // Prepare variables to send in to the database.
    private string nameToSend;
    private string secondsToSend;

    private string conn;

	// Use this for initialization
	void Start () {
        // Prepare for score input dialogue
	    inputButton = GameObject.Find("SendSQLInputButton").GetComponent<Button>();
        inputButton.onClick.AddListener(()=> (sendScores()));
        // Prepare for getting user input from input field and current time, and also hiding the score input window.
        inputText = GameObject.Find("NameInputText").GetComponent<Text>();
        secondText = GameObject.Find("TextTimer").GetComponent<Text>();
        scoreInputWindow = GameObject.Find("ScoreInputWindow");
        scoreInputWindow.SetActive(false);
        // Prepare for displaying the scoreboard window and modifying the text in it.
        // topScoreListText = GameObject.Find("TopScoreListText").GetComponent<Text>();
        // scoreboardWindow = GameObject.Find("ScoreList");
        // scoreboardWindow.SetActive(false);
        
        // Get the script to input data in to high score database.

        //if (!File.Exists("URI=file" + Application.dataPath + "/Data/highscore.db")) {
        //    SQLiteConnection.CreateFile("URI=file" + Application.dataPath + "/Data/highscore.db");
        //}

        conn = "URI=file:" + Application.dataPath + "/highscores.db";
        Debug.Log(conn);
        CreateTable();
        // SendSQLCommand("\"jee\"", "\"2346\"");
	}

    /// <summary>
    /// Feed player input and score in to the SendSQLCommand() method and handle window activation.
    /// </summary>
    public void sendScores() {
        Debug.Log("button pressed");
        nameToSend = inputText.text;
        secondsToSend = secondText.text;
        SendSQLCommand("\"" + nameToSend + "\"", secondsToSend.ToString());
        scoreInputWindow.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene("Score");
        //topScoreListText.text = GetSQLScores(20);
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
    /// Create a table in the game's database if it doesn't already exist.
    /// </summary>
    public IEnumerable SendSQLCommand(string nameIn, string scoreIn) {
        using (IDbConnection dbconn = new SqliteConnection(conn)) {
            dbconn.Open();
            using (IDbCommand dbCmd = dbconn.CreateCommand()) {
                string sqlQuery = String.Format("INSERT INTO scores (name, score) " +
                        "VALUES (" + nameIn + ", " + scoreIn +")");
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

	// Update is called once per frame
	void Update () {
		
	}
}
