using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AI : MonoBehaviour
{
	public struct Move
	{
		public int row, col;
	}

	[SerializeField] private int maxDepth = 3;
	private string player = "X";
	private string computer = "O";
	private GameController controller;
	private int rows;
	private int columns;
	private const int MIN = -1000;
	private const int MAX = 1000;

	private void Awake()
	{
		controller = GetComponentInParent<GameController>();
		rows = controller.rows;
		columns = controller.columns;
	}
	
	/*MovesLeft:
	 self explanatory
    */
	bool MovesLeft(Text[,] board)
	{
		for (int i = 0; i < rows; i++)
		for (int j = 0; j < columns; j++)
			if (board[i, j].text == "")
				return true;
		return false;
	}

	/*
	 * Evaluate:
	 * Returns a value based on who is winning 
	 */
	int Evaluate(Text[,] board)
	{
		int computerMoves = 0;
		int playerMoves = 0;

		for (int row = 0; row < rows; row++)
		{

			playerMoves = 0;
			computerMoves = 0;

			for (int col = 0; col < columns; col++)
			{
				if (board[row, col].text == computer)
					computerMoves++;
				if (board[row, col].text == player)
					playerMoves++;
			}

			if (computerMoves == columns)
				return 10;
			if (playerMoves == columns)
				return -10;
		}

		for (int col = 0; col < columns; col++) 
		{
        			
        			playerMoves = 0;
        			computerMoves = 0;

                    for (int row = 0; row < rows; row++) {
        				if (board[row, col].text == computer)
        					computerMoves++;
        				if (board[row, col].text == player)
        					playerMoves++;
        			}
         
        			if (computerMoves == rows)
        				return 10;
        			if (playerMoves == rows)
        				return -10;
		}
		
		playerMoves = 0;
		computerMoves = 0;
		for (int row = 0; row < rows; row++) {
			if (board[row, row].text == computer)
				computerMoves++;
			if (board[row, row].text == player)
				playerMoves++;

			if (computerMoves == rows)
				return 10;
			if (playerMoves == rows)
				return -10;
		}
		
		playerMoves = 0;
		computerMoves = 0;
		for (int row = 0; row < rows; row++) {
			if (board[row, (rows - 1) - row].text == computer)
				computerMoves++;
			if (board[row, (rows - 1) - row].text == player)
				playerMoves++;

			if (computerMoves == rows)
				return 10;
			if (playerMoves == rows)
				return -10;
		}
		return 0;	
	}

	/*
	 * MinMax
	 * This is the minimax function. It considers all
	 * the possible ways the game can go and returns 
	 */
	int MinMax(Text[,] board, int depth, bool isMax)
	{
		int score = Evaluate(board);
		
		if (depth == maxDepth)
			return score;

		if (score == 10)
			return score;
		
		if (score == -10)
			return score;

		//Draw
		if (MovesLeft(board) == false)
			return 0;

		if (isMax) 
		{
			int best = MIN;

			for (int i = 0; i < rows; i++) {
				for (int j = 0; j < columns; j++) {
					if (board[i, j].text == "") {
						board[i, j].text = computer;
						int value = Math.Max(best, MinMax(board, depth + 1, !isMax));
						best = Math.Max(best, value);

						board[i, j].text = "";
					}
				}
			}
			return best;
		}
		else 
		{
			int best = MAX;

			for (int i = 0; i < rows; i++) {
				for (int j = 0; j < columns; j++) {
					if (board[i, j].text == "") {
						board[i, j].text = player;
						int value = Math.Min(best, MinMax(board, depth + 1, !isMax));
						best = Math.Min(best, value);
						
						board[i, j].text = "";
						
					}
				}
			}
			return best;
		}
	}
	
	
	/*
	 * BestMove:
	 * evaluate minimax function for
	 * all empty cells.
	 */
	public Move BestMove(Text[,] board)
	{
		int bestVal = MIN;// -1000
		Move bestMove;
		bestMove.row = -1;
		bestMove.col = -1;

		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < columns; j++)
			{
				if (board[i, j].text == "")
				{
					board[i, j].text = computer;
					int moveValue = MinMax(board, 0, false);
					board[i, j].text = "";

					if (moveValue > bestVal)
					{
						bestMove.row = i;
						bestMove.col = j;
						bestVal = moveValue;
					}
				}
			}
		}
		return bestMove;
	}
}
