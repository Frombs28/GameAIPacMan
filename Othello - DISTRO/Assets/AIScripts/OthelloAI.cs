using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OthelloAI : AIScript{

    //BLACK IS CONSIDERED POSITIVE INT FOR ALL SEF FUNCTIONS
    //If a positive number is returned, it means it's advantageous for black
    //And if a negative number is returned it means good for white
    //The higher the number, the better for black it is

    //Returns int score, assumes all equal positions
    public int UniformSEF(BoardSpace[][] board) {
        int blackCount = 0;
        int whiteCount = 0;
        foreach (BoardSpace[] row in board) {
            foreach (BoardSpace space in row) {
                switch (space) {
                    case (BoardSpace.BLACK):
                        blackCount++;
                        break;
                    case (BoardSpace.WHITE):
                        whiteCount++;
                        break;
                }
            }
        }
        return blackCount - whiteCount;
    }


    int[,] sefWeights = {
        {5, 2, 3, 3, 3, 3, 2, 5},
        {2, 1, 1, 1, 1, 1, 1, 2},
        {3, 1, 1, 1, 1, 1, 1, 3},
        {3, 1, 1, 1, 1, 1, 1, 3},
        {3, 1, 1, 1, 1, 1, 1, 3},
        {3, 1, 1, 1, 1, 1, 1, 3},
        {2, 1, 1, 1, 1, 1, 1, 2},
        {5, 2, 3, 3, 3, 3, 2, 5}
    };

    public int WeightedSEF(BoardSpace[][] board) {
        int blackCount = 0;
        int whiteCount = 0;
        int i = 0, j = 0;
        foreach (BoardSpace[] row in board) {
            foreach (BoardSpace space in row) {
                switch (space) {
                    case (BoardSpace.BLACK):
                        blackCount += sefWeights[i, j];
                        break;
                    case (BoardSpace.WHITE):
                        whiteCount += sefWeights[i, j];
                        break;
                }
                j++;
            }
            j = 0;
            i++;
        }
        return blackCount - whiteCount;
    }

    public override KeyValuePair<int, int> makeMove(List<KeyValuePair<int, int>> availableMoves, BoardSpace[][] currentBoard) {

        Debug.Log(WeightedSEF(currentBoard) + ", " + UniformSEF(currentBoard));
        //Given Random AI Code
        return availableMoves[Random.Range(0, availableMoves.Count)];
    }

}
