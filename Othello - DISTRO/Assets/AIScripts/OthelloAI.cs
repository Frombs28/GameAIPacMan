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


    //TODO: Improvements: It's better to have fewer pieces on the board during mid-game, figure out
    //a way to add this to the BetterSEF function
    int[,] sefWeights = {
        {20, -3, 3, 3, 3, 3, -3, 20},
        {-3, -4, -1, -1, -1, -1, -4, -3},
        {3, -1, 1, 0, 0, 1, -1, 3},
        {3, -1, 0, 1, 1, 0, -1, 3},
        {3, -1, 0, 1, 1, 0, -1, 3},
        {3, -1, 1, 0, 0, 1, -1, 3},
        {-3, -4, -1, -1, -1, -1, -4, -3},
        {20, -3, 3, 3, 3, 3, -3, 20}
    };
    /*int[,] sefWeights = {
        {5, 2, 3, 3, 3, 3, 2, 5},
        {2, 1, 1, 1, 1, 1, 1, 2},
        {3, 1, 1, 1, 1, 1, 1, 3},
        {3, 1, 1, 1, 1, 1, 1, 3},
        {3, 1, 1, 1, 1, 1, 1, 3},
        {3, 1, 1, 1, 1, 1, 1, 3},
        {2, 1, 1, 1, 1, 1, 1, 2},
        {5, 2, 3, 3, 3, 3, 2, 5}
    };*/
    public int BetterSEF(BoardSpace[][] board) {
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

    public int Negamax(BoardSpace[][] board, int depth, bool isBlack) {
        //Generate List of possible moves
        List<KeyValuePair<int, int>> possibleMoves = BoardScript.GetValidMoves(board, isBlack ? ((uint)2) : ((uint)1));

        //If hit end, bubble up
        if (depth <= 0 || possibleMoves.Count == 0) {
            if (BoardScript.betterSEF) {
                return BetterSEF(board);
            } else {
                return UniformSEF(board);
            }
        }

        if (isBlack) {
            int maxScore = -9999;
            foreach (KeyValuePair<int, int> move in possibleMoves) {
                BoardSpace[][] newBoard = createNewBoard(board, move, isBlack);
                int currentScore = Negamax(newBoard, depth - 1, false);
                if (currentScore > maxScore) maxScore = currentScore;
            }
            return maxScore;
        } else {
            int minScore = 9999;
            foreach (KeyValuePair<int, int> move in possibleMoves) {
                BoardSpace[][] newBoard = createNewBoard(board, move, isBlack);
                int currentScore = Negamax(newBoard, depth - 1, true);
                if (currentScore < minScore) minScore = currentScore;
            }
            return minScore;
        }

    }

    public int ABNegamax(BoardSpace[][] board, int depth, bool isBlack, int alpha, int beta) {
        //Generate List of possible moves
        List<KeyValuePair<int, int>> possibleMoves = BoardScript.GetValidMoves(board, isBlack ? ((uint)2) : ((uint)1));

        //If hit end, bubble up
        if (depth <= 0 || possibleMoves.Count == 0) {
            if (BoardScript.betterSEF) {
                return BetterSEF(board);
            } else {
                return UniformSEF(board);
            }
        }

        if (isBlack) {
            int maxScore = -9999;
            foreach (KeyValuePair<int, int> move in possibleMoves) {
                BoardSpace[][] newBoard = createNewBoard(board, move, isBlack);
                int currentScore = ABNegamax(newBoard, depth - 1, false, alpha, beta);
                if (currentScore > maxScore) maxScore = currentScore;
                if (currentScore > alpha) alpha = currentScore;
                if (beta <= alpha) break;
            }
            return maxScore;
        } else {
            int minScore = 9999;
            foreach (KeyValuePair<int, int> move in possibleMoves) {
                BoardSpace[][] newBoard = createNewBoard(board, move, isBlack);
                int currentScore = ABNegamax(newBoard, depth - 1, true, alpha, beta);
                if (currentScore < minScore) minScore = currentScore;
                if (currentScore < beta) currentScore = beta;
                if (beta <= alpha) break;
            }
            return minScore;
        }
    }

    public KeyValuePair<int, int> findBestMove(BoardSpace[][] board, List<KeyValuePair<int, int>> availableMoves) {
        //Tracking Values
        KeyValuePair<int, int> bestMove = new KeyValuePair<int, int>();
        int bestScore = -9999;
        //Loop through all possible moves at top level
        foreach (KeyValuePair<int, int> move in availableMoves) {
            //Create new board from available move
            BoardSpace[][] newBoard = createNewBoard(board, move, true);

            //Negamax the board
            int currentMoveScore;
            if (BoardScript.ABnegamax) {
                currentMoveScore = ABNegamax(board, BoardScript.searchDepth - 1, false, -9999, 9999);
            } else {
                currentMoveScore = Negamax(board, BoardScript.searchDepth - 1, false);
            }

            //Keep track of the best move to return
            if (currentMoveScore > bestScore) {
                bestScore = currentMoveScore;
                bestMove = move;
            }
        }

        //Return the best move to be moved
        return bestMove;
    }

    public BoardSpace[][] createNewBoard(BoardSpace[][] board, KeyValuePair<int, int> move, bool isBlack) {
        BoardSpace[][] newBoard = new BoardSpace[8][];
        //Copy board
        for (int i = 0; i < 8; i++) {
            newBoard[i] = new BoardSpace[8];
            for (int j = 0; j < 8; j++) {
                newBoard[i][j] = board[i][j];
            }
        }

        newBoard[move.Key][move.Value] = isBlack ? BoardSpace.BLACK : BoardSpace.WHITE;

        //Update Moves
        List<KeyValuePair<int, int>> changedSpots = BoardScript.GetPointsChangedFromMove(newBoard, isBlack ? ((uint) 2) : ((uint) 1), move.Value, move.Key);
        foreach (KeyValuePair<int, int> change in changedSpots) {
            newBoard[change.Key][change.Value] = isBlack ? BoardSpace.BLACK : BoardSpace.WHITE;
        }

        return newBoard;
    }

    public override KeyValuePair<int, int> makeMove(List<KeyValuePair<int, int>> availableMoves, BoardSpace[][] currentBoard) {
        return findBestMove(currentBoard, availableMoves);
    }

}
