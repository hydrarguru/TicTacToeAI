using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {
    
   public Text[,] buttonListArray = new Text[3, 3];
   public Text[] buttonList;
   public GameObject gameOverPanel;
   public Text gameOverText;
   public GameObject restartButton;
   public bool playAgainstAI = true;
   //Board
   public int rows = 3;
   public int columns = 3;
   
   private int moveCount;
   private string playerSide;
   private string computerSide;

   private AI computerAI;
   private bool playerTurn;
   
   

   void Awake()
   {
       buttonListArray = CreateBoard(buttonList, rows, columns);
       SetGameControllerReferenceOnButtons();
       playerSide = "X";
       computerSide = "O";
       gameOverPanel.SetActive(false);
       moveCount = 0;
       restartButton.SetActive(false);
       computerAI = GetComponent<AI>();
       playerTurn = true;
   }

   void SetGameControllerReferenceOnButtons()
   {
       for (int i = 0; i < rows; i++)
       {
           for (int j = 0; j < columns; j++)
           {
               buttonListArray[i, j].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
           }
       }
       
       
       /*
       for (int i = 0; i < buttonList.Length; i++)
       {
           buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
       }
       */
   }

   public string GetPlayerSide()
   {
       return playerSide;
   }

   public void EndTurn()
   {
       moveCount++;
       if (GetWinState(playerSide))
           GameOver(playerSide);
       else if (GetWinState(computerSide))
           GameOver(computerSide);
       else if (moveCount >= rows*columns)
           GameOver("draw");
       else
           ChangeSides();
   }

   void ChangeSides()
   {
       //Choose between playing against AI or a player
       if (!playAgainstAI)
           playerSide = (playerSide == "X") ? "O" : "X";
       if (playAgainstAI)
       {
           playerTurn = !playerTurn;
           if (!playerTurn)
               ComputerAITurn();
       }
   }

   void GameOver(string winningPlayer)
   {
       SetBoardInteractable(false);
       if (winningPlayer == "draw")
       {
           SetGameOverText("It's a Draw!");
       } else
       {
           SetGameOverText(winningPlayer + " Wins!");
       }
       restartButton.SetActive(true);
   }

   void SetGameOverText(string value)
   {
       gameOverPanel.SetActive(true);
       gameOverText.text = value;
   }

   public void RestartGame()
   {
       playerSide = "X";
       moveCount = 0;
       gameOverPanel.SetActive(false);
       restartButton.SetActive(false);
       playerTurn = true;
       
       for (int i = 0; i < rows; i++) {
           for (int j = 0; j < columns; j++) {
               buttonListArray[i, j].text = "";
           }
       }
       
       SetBoardInteractable(true);
   }

   void SetBoardInteractable(bool toggle)
   {
       for (int i = 0; i < rows; i++)
       {
           for (int j = 0; j < columns; j++)
           {
               buttonListArray[i, j].GetComponentInParent<Button>().interactable = toggle;
           }
       }
       /*
       for (int i = 0; i < buttonList.Length; i++)
       {
           buttonList[i].GetComponentInParent<Button>().interactable = toggle;
       }
       */
   }

   void ComputerAITurn()
   {
       AI.Move computerMove = computerAI.BestMove(buttonListArray);
       buttonListArray[computerMove.row, computerMove.col].text = computerSide;
       buttonListArray[computerMove.row, computerMove.col].GetComponentInParent<Button>().interactable = false;
       
       
       EndTurn();
   }

   private bool GetWinState(string side)
   {
       int counter = 0;
       for (int row = 0; row < rows; row++) {
           counter = 0;
           for (int col = 0; col < columns; col++) {
               if (buttonListArray[row, col].text == side)
                   counter++;
           }

           if (counter == rows)
               return true;
       }

       for (int col = 0; col < columns; col++) {
           counter = 0;
           for (int row = 0; row < rows; row++) {
               if (buttonListArray[row, col].text == side)
                   counter++;
           }

           if (counter == columns)
               return true;
       }

       counter = 0;
       for (int row = 0; row < rows; row++) {
           if (buttonListArray[row, row].text == side)
               counter++;
           if (counter == rows)
               return true;
       }

       counter = 0;
       for (int row = 0; row < rows; row++) {
           if (buttonListArray[row, (rows - 1) - row].text == side)
               counter++;
           if (counter == rows)
               return true;
       }
       return false;
   }
   
   
   private Text[,] CreateBoard(Text[] board, int rows, int columns) {
       Text[,] output = new Text[rows, columns];
       for (int i = 0; i < rows; i++) {
           for (int j = 0; j < columns; j++) {
               output[i, j] = board[i * columns + j];
           }
       }
       return output;
   }
}


