using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SendInputToDB : MonoBehaviour {

    private Button inputButton;
    private Text inputText;
    private Text inputScoreText;
    private DBController dbController;

	/// <summary>
    /// Get UI components necessary for score upload.
    /// </summary>
	void Start () {
	    inputButton = GameObject.Find("SendSQLInputButton").GetComponent<Button>();
        inputButton.onClick.AddListener(()=> (SendData()));
        inputText = GameObject.Find("NameInputText").GetComponent<Text>();
        inputScoreText = GameObject.Find("TextTimer").GetComponent<Text>();
        dbController = GameObject.Find("DBHandler").GetComponent<DBController>();
	}

    /// <summary>
    /// Get data from UI components and send them via DBController.
    /// </summary>
    void SendData() {
        Debug.Log("jou" + dbController);
        string nameToSend = inputText.text;
        int scoreToSend = Int32.Parse(inputScoreText.text);
        dbController.SendSQLCommand(nameToSend, scoreToSend);
        Time.timeScale = 1;
        SceneManager.LoadScene("Score");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
