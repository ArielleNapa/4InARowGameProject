using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FourInARow
{
    internal class UserInterface
    {

        private enum eGameSetting
        {
            playerWithPlayer = 1,
            computerWithPlayer = 2,
            minCellsInRowAndCol = 4,
            maxCellsInRowAndCol = 8,
            firstPlayer = 1,
            secondPlayer = 2
        }
        private GameRules m_CreateGame;
        private const char k_RetireFromGameWithQ = 'Q';
        private const char k_RetireFromGameWithLowerQ = 'q';

        private Random m_Random = new Random();

        internal void StartGame()
        {
            int numberOfRows;
            int numberOfColumns;
            int gameMode;

            getMatrixSize(out numberOfRows, out numberOfColumns);
            getGameMode(out gameMode);

            m_CreateGame = new GameRules(numberOfRows, numberOfColumns, gameMode);

            runGame();
        }

        private void getMatrixSize(out int o_numberOfRows, out int o_numberOfColumns)
        {
            bool isSizesAreValid = false;
            string rowsInput = null;
            string colsInput = null;

            Console.WriteLine("Please enter size for rows and columns for your next game, min:4X4, Max:8X8");

            while(!isSizesAreValid)
            {
                Console.WriteLine("Choose rows's size:");
                rowsInput = Console.ReadLine();
                Console.WriteLine("Choose columns's size:");
                colsInput = Console.ReadLine();
                isSizesAreValid = checkInputValidation(rowsInput, colsInput);
            }
            o_numberOfRows = int.Parse(rowsInput);
            o_numberOfColumns = int.Parse(colsInput);
        }

        private bool checkInputValidation(string i_rows, string i_cols)
        {
            int rows, columns;

            rows = int.Parse(i_rows);
            columns = int.Parse(i_cols);

            bool isSizesValid = rows >= ((int)eGameSetting.minCellsInRowAndCol) && rows <= ((int)eGameSetting.maxCellsInRowAndCol)
                && columns >= ((int)eGameSetting.minCellsInRowAndCol) && columns <= ((int)eGameSetting.maxCellsInRowAndCol);
            
            if(!isSizesValid)
            {
                if (rows < ((int)eGameSetting.minCellsInRowAndCol))
                {
                    Console.WriteLine("You chose invalid input for rows, please try again");
                }
                else if (columns < ((int)eGameSetting.minCellsInRowAndCol) || columns > ((int)eGameSetting.maxCellsInRowAndCol))
                {
                    Console.WriteLine("You chose invalid input for columns, please try again");
                }
                else
                    Console.WriteLine("All your choices are invalid, please try again");
            }
            return isSizesValid;
        }

        private void getGameMode(out int o_gameMode)
        {
            bool isModeValid = false;
            string gameModeInput = null;

            Console.WriteLine("Please choose game mode, write 1 to play with computer, 2 to play with player");

            while(!isModeValid)
            {
                gameModeInput = Console.ReadLine();
                isModeValid = checkModeValidation(gameModeInput);
            }
            o_gameMode = int.Parse(gameModeInput);
        }

        private bool checkModeValidation(string i_gameModeInput)
        {
            int gameModeInput;

            gameModeInput = int.Parse(i_gameModeInput);
            bool isModeValid = gameModeInput == ((int)eGameSetting.computerWithPlayer) 
                || gameModeInput == ((int)eGameSetting.playerWithPlayer);

            if (!isModeValid)
            {
                Console.WriteLine("invalid input, please try again");
            }
            return isModeValid;
        }

        private void runGame()
        {
            bool keepPlaying = true;
            int playerTurn = 1;

            Player playerOne = m_CreateGame.PlayerOne;
            Player playerTwo = m_CreateGame.PlayerTwo;

            while (keepPlaying)
            {
                PrintCurrentBoard();

                if (playerTurn == ((int)eGameSetting.firstPlayer))
                {
                    playerTurn = (int)eGameSetting.secondPlayer;
                    keepPlaying = playingPlayerMove(playerOne);
                }
                else
                {
                    playerTurn = (int)eGameSetting.firstPlayer;
                    keepPlaying = playingPlayerMove(playerTwo);
                }
            }
            System.Threading.Thread.Sleep(1000);
            endGame(playerOne, playerTwo);
        }

        public void PrintCurrentBoard()
        {
            Matrix boardToPrint = m_CreateGame.Board;
            int rowsInBoard = m_CreateGame.Board.Rows;
            int columnsInBoard = m_CreateGame.Board.Columns;
            char[,] boardForPrint = boardToPrint.MatrixBoard;

            StringBuilder converntBoardToString;

            Ex02.ConsoleUtils.Screen.Clear();
            addMatrixShield(columnsInBoard, out converntBoardToString);

            for (int i = 0; i < rowsInBoard; i++)
            {
                for (int j = 0; j < columnsInBoard; j++)
                {
                    converntBoardToString.AppendFormat("| ");
                    converntBoardToString.Append(boardForPrint[i, j]);
                    converntBoardToString.Append(" ");
                }
                converntBoardToString.Append("|");
                converntBoardToString.AppendLine("");
                converntBoardToString.Append('=', columnsInBoard * (int)eGameSetting.minCellsInRowAndCol + 1);
                converntBoardToString.AppendLine("");
            }
            Console.WriteLine(converntBoardToString);
        }

        private void addMatrixShield(int i_numOfColumns, out StringBuilder o_convertBoard)
        {
            o_convertBoard = new StringBuilder();

            for(int i = 1; i <= i_numOfColumns; i++)
            {
                o_convertBoard.AppendFormat("  {0} ", i);
            }
            o_convertBoard.AppendLine("");
        }

        private bool playingPlayerMove(Player i_playingPlayer)
        {
            bool isValidMove = false;
            int nextMovePlayerPlay;
            string nextMoveToString = null;

            while(!isValidMove)
            {
                if(i_playingPlayer.IsPlayVsComputer)
                {
                    nextMovePlayerPlay = m_Random.Next(1, m_CreateGame.Board.Columns + 1);
                    nextMoveToString = nextMovePlayerPlay.ToString();
                    isValidMove = checkIfMoveValid(nextMoveToString);
                }
                else
                {
                    Console.WriteLine("Current player is: {0}", i_playingPlayer.PlayerNumber);
                    Console.WriteLine("Please enter your next move");

                    nextMoveToString = Console.ReadLine();
                    isValidMove = checkIfMoveValid(nextMoveToString);
                }
            }
            return createPlayerMove(i_playingPlayer, nextMoveToString);
        }

        private bool checkIfMoveValid(string i_chosenColumn)
        {
            bool isInputMoveValid = true;
            int maxSizeColumns = m_CreateGame.Board.Columns;
            int chosenCol = int.Parse(i_chosenColumn);
            int chosenColumn;


            if(!i_chosenColumn.Equals(k_RetireFromGameWithLowerQ) && !i_chosenColumn.Equals(k_RetireFromGameWithQ))
            {
                isInputMoveValid = int.TryParse(i_chosenColumn, out chosenColumn);
                isInputMoveValid = isInputMoveValid && chosenColumn > 0
                    && chosenColumn <= maxSizeColumns
                    && !m_CreateGame.Board.IsColumnFull(chosenColumn - 1);
            }
            if(!isInputMoveValid)
            {
                Console.WriteLine("You can't chose {0} as your move, please choose again", chosenCol);
            }
            return isInputMoveValid;
        }

        private bool createPlayerMove(Player i_playingPlayer, string i_inputMove)
        {
            bool keepPlayAfterMove;

            if(i_inputMove.Equals(k_RetireFromGameWithLowerQ) || i_inputMove.Equals(k_RetireFromGameWithQ))
            {
                if(i_playingPlayer.PlayerNumber == (int)eGameSetting.firstPlayer)
                {
                    m_CreateGame.AddPoints((int)eGameSetting.secondPlayer);
                }
                else
                {
                    m_CreateGame.AddPoints((int)eGameSetting.firstPlayer);
                }

                keepPlayAfterMove = false;
            }
            else
            {
                int chosenColumnToPlay = int.Parse(i_inputMove) - 1;
                m_CreateGame.AddMoveToBoard(chosenColumnToPlay, i_playingPlayer);
                keepPlayAfterMove = checkIfToContinueGame(i_playingPlayer, chosenColumnToPlay);
            }
            return keepPlayAfterMove;
        }

        private bool checkIfToContinueGame(Player i_playingPlayer, int i_chosenMove)
        {
            bool checkScores = m_CreateGame.Board.CheckIfWin(i_playingPlayer, i_chosenMove);
            if (checkScores)
            {
                m_CreateGame.AddPoints(i_playingPlayer.PlayerNumber);
                PrintCurrentBoard();
                Console.WriteLine("The winner is {0}", i_playingPlayer.PlayerNumber);
                System.Threading.Thread.Sleep(1500);
            }
            else
            {
                checkScores = m_CreateGame.Board.IsMatrixFull();
            }
            return !checkScores;
        }

        private void endGame(Player i_player1, Player i_player2)
        {
            string playAgain;

            Ex02.ConsoleUtils.Screen.Clear();
            Console.WriteLine("Player1 has {0} points, and Player2 has {1} points", i_player1.Points, i_player2.Points);
            if(i_player1.Points == i_player2.Points)
            {
                Console.WriteLine("Its a tie!!!");
            }
            else if(i_player1.Points > i_player2.Points)
            {
                Console.WriteLine("The winner is Player1");
            }
            else
            {
                Console.WriteLine("The winner is Player2");
            }

            Console.WriteLine("Please enter 1 to start a rematch or 2 to exit");
            playAgain = Console.ReadLine();
            while (!playAgain.Equals("1") && !playAgain.Equals("2"))
            {
                Console.WriteLine("Invalid choose, please try again");
                playAgain = Console.ReadLine();
            }
            if(playAgain.Equals("1"))
            {
                rematch();
            }
            else
            {
                Console.WriteLine("Thanks for playing! hope you enjoyed.. Goodbye");
                System.Threading.Thread.Sleep(1500);
            }
        }

        private void rematch()
        {
            m_CreateGame.Board.ConstructTheBoard();
            runGame();
        }
    }
}
