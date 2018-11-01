using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [Header("Player settings")]
    public Sprite[] playerSprites;
    public Image playerOne;
    public Image playerTwo;
    public Text[] playerNames;
    [Header("UI settings")]
    public Text manageText;
    public GameObject gameModePanel;
    public GameObject NextRoundBtn;

    [Header("Score settings")]
    public Text scoreText;
    public Text scoreOneText;
    public Text scoreTwoText;
    public int maxScore = 4;
    private int scoreOne = 0, scoreTwo = 0;
    private static int playerOneScore = 0, playerTwoScore = 0;

    [Header("AI settings")]
    public bool vsAI = false; // activate to play versus AI;
    public float aiReactionTime = 1f;
    private float aiTimer = 0;

    private int gameStage = 0;
    private float timer = 0;
    private float bangTimer = 0;
    private bool isPlayerOneShot = false;
    private bool isPlayerTwoShot = false;

    private void Start()
    {
        scoreText.text = 0 + " : " + 0;

        //in case we forgot to setup UI settings
        gameModePanel.SetActive(true);
        NextRoundBtn.SetActive(false);
    }

    public void Trigger_StartGame()
    {
        //disable gamemode panel when game start
        gameModePanel.SetActive(false);

        manageText.text = "ready";
        playerOne.sprite = playerSprites[1];
        playerTwo.sprite = playerSprites[1];
        gameStage = 1;

    }

    // Update is called once per frame
    void Update () {
        if (gameStage == 1)
        {
            if (timer >= 1f)
            {
                manageText.text = "steady";
                bangTimer = Random.Range(1f, 3f);
                gameStage = 2;
                timer = 0;
                NextRoundBtn.SetActive(false);
                return;
            }
            else { timer += Time.deltaTime; }
        }
        else if (gameStage == 2)
        {
            if (timer >= bangTimer)
            {
                manageText.text = "!bang!";
                bangTimer = 0;
                //set the AI timer before shoot!
                aiTimer = Random.Range(0, aiReactionTime);

                gameStage = 3;
                timer = 0;
            }
            else { timer += Time.deltaTime; }
        }

        //AI "logics" goes here. if AI mode is disabled - skip this check
        if (!vsAI) { return; }
            else if (gameStage == 3)
            {
                timer += Time.deltaTime;
                if (timer >= aiTimer)
                {
                    Trigger_Shot(1);
                }
            }
	}

    public void Trigger_Shot(int player)
    {
        if (gameStage == 0) return;
        Image activePlayer = (player == 0) ? playerOne : playerTwo;
        if (gameStage == 3)
        {
            Image enemyPlayer = (player == 1) ? playerOne : playerTwo;
            
            activePlayer.sprite = playerSprites[2];
            enemyPlayer.sprite = playerSprites[3];
            manageText.text = "round over. "  + playerNames[player].text + " win";

            //if player 1 is winner then add 1 point to his score;
            if (player == 0) { scoreOne++; }
            //if player 2 is winner then add 1 point to his score;
            if (player == 1) { scoreTwo++; }

            //if player one reached the max score tell that he is the winner and reset the score;
            if (scoreOne == maxScore)
            {
                manageText.text = playerNames[player].text + " win the game";
                scoreOne = scoreTwo = 0;
                playerOneScore++;
                scoreOneText.text = "" + playerOneScore;
                NextRoundBtn.SetActive(false);
                gameModePanel.SetActive(true);

                //reenable gamemode panel ONLY when game ends
                //to prevent from gamemode change during active game session
                NextRoundBtn.SetActive(false);
                gameModePanel.SetActive(true);

            }
            else
            //if player two reached the max score tell that he is the winner and reset the score;
            if (scoreTwo == maxScore)
            {
                manageText.text = playerNames[player].text + " win the game";
                scoreOne = scoreTwo = 0;
                playerTwoScore++;
                scoreTwoText.text = "" + playerTwoScore;

                //reenable gamemode panel ONLY when game ends
                //to prevent from gamemode change during active game session
                gameModePanel.SetActive(true);
            }

            //update score after every round
            scoreText.text = scoreOne + " : " + scoreTwo;
            //reenable Next Round button
            NextRoundBtn.SetActive(true);
            gameStage = 0;
            isPlayerOneShot = false;
            isPlayerTwoShot = false;
        }
        else
        {
            if (player == 0) { isPlayerOneShot = true; }
            if (player == 1) { isPlayerTwoShot = true; }
            if (isPlayerOneShot && isPlayerTwoShot)
            {
                manageText.text = "Hold on boyz!";
                bangTimer = 0;
                gameStage = 0;
                timer = 0;
                isPlayerOneShot = false;
                isPlayerTwoShot = false;
            }
            activePlayer.sprite = playerSprites[4];
        }
    }

    public void Trigger_AI(bool activateAI)
    {
        vsAI = activateAI;

        // change players names according to gamemode
        if (activateAI)
        {
            playerNames[0].text = "Player"; playerNames[1].text = "AI";
        }
        else
        {
            playerNames[0].text = "Player One"; playerNames[1].text = "Playe Two";
        }
    }

}
