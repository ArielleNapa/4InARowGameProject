using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FourInARow
{
    public class GameRules
    {
        private enum eGameStandarts
        {
            computerWithPlayer = 1,
            playerWithPlayer = 2,
            minCellsInRowAndCol = 4,
            maxCellsInRowAndCol = 8
        }

        private const char k_FirstPlayerCoinPrint = 'X';
        private const char k_SecondPlayerCoinPrint = 'O';
        private const int k_PlayerOne = 1;
        private const int k_PlayerTwo = 2;

        private Player m_playerOne;
        private Player m_playerTwo;
        private Matrix m_board;

        public GameRules(int i_numOfRows, int i_numOfCols, int i_gameMode)
        {
            bool gameVsComputer = i_gameMode == (int)eGameStandarts.computerWithPlayer;
            m_playerOne = new Player(k_PlayerOne, k_FirstPlayerCoinPrint, (int)eGameStandarts.playerWithPlayer);
            m_playerTwo = new Player(k_PlayerTwo, k_SecondPlayerCoinPrint, i_gameMode);
            m_board = new Matrix(i_numOfRows, i_numOfCols);
        }

        internal Player PlayerOne
        {
            get { return m_playerOne; }
        }

        internal Player PlayerTwo
        {
            get { return m_playerTwo; }
        }

        internal Matrix Board
        {
            get { return m_board; }
        }

        public void AddPoints(int i_playerNum)
        {
            if (i_playerNum == m_playerOne.PlayerNumber)
            {
                m_playerOne.Points++;
            }
            else
                m_playerTwo.Points++;
        }

        public void AddMoveToBoard(int i_move, Player i_playingPlayer)
        {
            m_board.PlayYourTurn(i_move, i_playingPlayer.CoinPrint);
        }
    }
}
