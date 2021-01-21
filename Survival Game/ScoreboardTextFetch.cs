using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardTextFetch : MonoBehaviour {

    private Text scoreboardText;
    private DBController dbController;

    /// <summary>
    /// Get the necessary GameObjects and call method to update scoreboard.
    /// </summary>
	void Start () {
		scoreboardText = GameObject.Find("ScoreboardText").GetComponent<Text>();
        UpdateScores();
	}

    /// <summary>
    /// Get the DBController class necessary for scoreboard update.
    /// </summary>
    void Awake() {
        dbController = GameObject.Find("DBFetcher").GetComponent<DBController>();
    }

    /// <summary>
    /// Update the scoreboard via DBController.
    void UpdateScores() {
        scoreboardText.text = dbController.GetSQLScores(10);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
