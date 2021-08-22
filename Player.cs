using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace C21_Ex02_AvivShabi_313171050_ArielleNapadensky_313306078
{
    public class Player
    {
        private enum eComputerOrHuman
        {
            computerPlayer = 1,
            humanPlayer = 2
        }

        private int m_playerNumber;
        private bool m_playWithComputer = false;
        private int m_points = 0;
        private char m_coinPrint;

        public Player(int i_playerNumber, char i_coinPrint, int i_gameMode)
        {
            m_playerNumber = i_playerNumber;
            if(i_gameMode == ((int)eComputerOrHuman.computerPlayer))
            {
                m_playWithComputer = true;
            }
            m_coinPrint = i_coinPrint;
        }

        internal int PlayerNumber
        {
            get
            {
                return m_playerNumber;
            }
        }

        internal bool IsPlayVsComputer
        {
            get { return m_playWithComputer; }
        }

        internal int Points
        {
            set { m_points = value; }
            get { return m_points; }
        }

        internal char CoinPrint
        {
            get { return m_coinPrint; }
        }
    }
}
