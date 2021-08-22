using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FourInARow
{
    internal class Matrix
    {
        const char k_EmptyCell = ' ';
        const int k_FirstCol = 0;
        const int k_FirstRow = 0;
        const int k_InARowToWin = 4;

        private int m_Rows = 4;
        private int m_Cols = 4;
        private char[,] m_Board;

        public Matrix(int i_NumberOfRows, int i_NumberOfColumns)
        {
            m_Rows = i_NumberOfRows;
            m_Cols = i_NumberOfColumns;
            ConstructTheBoard();
        }

        internal int Rows
        {
            get { return m_Rows; }
            set { m_Rows = value; }
        }

        internal int Columns
        {
            get { return m_Cols; }
            set { m_Cols = value; }
        }

        internal char[,] MatrixBoard
        {
            get { return m_Board; }
        }

        internal void ConstructTheBoard()
        {
            m_Board = new char[m_Rows, m_Cols];
            for (int i = 0; i < m_Rows; i++)
            {
                for (int j = 0; j < m_Cols; j++)
                {
                    m_Board[i, j] = k_EmptyCell;
                }
            }
        }

        public bool IsColumnFull(int i_numOfColumn)
        {
            bool isFull = m_Board[k_FirstRow + 1, i_numOfColumn] != k_EmptyCell;
            return isFull;
        }

        public bool IsMatrixFull()
        {
            bool isFull = true;
            for (int i = 0; i < m_Cols; i++)
            {
                if (!IsColumnFull(i))
                {
                    isFull = false;
                    break;
                }
            }
            return isFull;
        }

        public void PlayYourTurn(int i_ColumnNumber, char i_CoinPrint)
        {
            if(!IsColumnFull(i_ColumnNumber))
            {
                int rowToInsert = 0;
                for(int i = 0; i < m_Rows - 1; i++)
                {
                    if(m_Board[i + 1, i_ColumnNumber] != k_EmptyCell)
                    {
                        rowToInsert = i;
                        break;
                    }
                    else if((i + 1) == (m_Rows - 1))
                        {
                            rowToInsert = i + 1;
                            break;
                        }
                }
                UpdateTheMatrix(i_ColumnNumber, rowToInsert, i_CoinPrint);
            }
        }

        public void UpdateTheMatrix(int i_ColumnsNumber, int i_RowNumber, char i_CoinPrint)
        {
            m_Board[i_RowNumber, i_ColumnsNumber] = i_CoinPrint;
        }
        
        public bool CheckIfFourInARow(char i_coinPrint, int i_currentRow, int i_currentColumn)
        {
            bool isFourInARow = false;
            bool isFourInAVerticalLine = CheckIfFourInVertical(i_coinPrint, i_currentRow, i_currentColumn);
            bool isFourInAHorizontalLine = CheckIfFourInHorizontal(i_coinPrint, i_currentColumn, i_currentRow);
            bool isFourInADiagonalLine = CheckIfFourInDiagonal(i_coinPrint, i_currentColumn, i_currentRow);
            bool isFourInAOppositeDagonalLine = CheckIfFourInOppositeDiagonal(i_coinPrint, i_currentColumn, i_currentRow);

            if (isFourInAVerticalLine || isFourInAHorizontalLine || isFourInADiagonalLine || isFourInAOppositeDagonalLine)
            {
                isFourInARow = true;
            }
            return isFourInARow;
        }
        
        public bool CheckIfFourInVertical(char i_coinPrint, int i_chosenRow, int i_chosenColumn)
        {
            int coinsCount = 0;
            bool isFourInAColumn = false;

            for(int i = 0; i < k_InARowToWin; i++)
            {
                if ((i_chosenRow + i) <= (m_Rows - 1))
                {
                    if (m_Board[i_chosenRow + i, i_chosenColumn] == i_coinPrint)
                    {
                        coinsCount++;
                    }
                    else break;
                }
                else break;
            }
            if(coinsCount == k_InARowToWin)
            {
                isFourInAColumn = true;
            }
            return isFourInAColumn;
        }

        public bool CheckIfFourInHorizontal(char i_coinPrint, int i_chosenColumn, int i_chosenRow)
        {
            int countLeftCoinMatch = GetLeftCountOfCoinPrint(i_coinPrint, i_chosenColumn - 1, i_chosenRow);
            int countRightCoinMatch = GetRightCountOfCoinPrint(i_coinPrint, i_chosenColumn + 1, i_chosenRow);
            int coinsCount = 1 + countLeftCoinMatch + countRightCoinMatch;

            return coinsCount >= k_InARowToWin;
        }

        public int GetLeftCountOfCoinPrint(char i_coinPrint, int i_chosenColumn, int i_chosenRow)
        {
            int countLeftMatchCoins = 0;
            for(int i = 0; i < k_InARowToWin - 1; i++)
            {
                if ((i_chosenColumn - i) >= k_FirstCol)
                {
                    if (m_Board[i_chosenRow, i_chosenColumn - i] == i_coinPrint)
                    {
                        countLeftMatchCoins++;
                    }
                    else break;
                }
                else break;
            }
            return countLeftMatchCoins;
        }

        public int GetRightCountOfCoinPrint(char i_coinPrint, int i_chosenColumn, int i_chosenRow)
        {
            int countRightMatchCoins = 0;
            for (int i = 0; i < k_InARowToWin - 1; i++)
            {
                if ((i_chosenColumn + 1) <= (m_Cols - 1))
                {
                    if (m_Board[i_chosenRow, i_chosenColumn + i] == i_coinPrint)
                    {
                        countRightMatchCoins++;
                    }
                    else break;
                }
                else break;
            }
            return countRightMatchCoins;
        }

        public bool CheckIfFourInDiagonal(char i_coinPrint, int i_chosenColumn, int i_chosenRow)
        {
            int countLeftDownCoinMatch = GetLeftDownCountOfCoinPrint(i_coinPrint, i_chosenColumn - 1, i_chosenRow + 1);
            int countRightUpCoinMatch = GetRightUpCountOfCoinPrint(i_coinPrint, i_chosenColumn + 1, i_chosenRow - 1);
            int coinsCount = 1 + countLeftDownCoinMatch + countRightUpCoinMatch;
            return coinsCount >= k_InARowToWin;
        }

        public int GetLeftDownCountOfCoinPrint(char i_coinPrint, int i_chosenColumn, int i_chosenRow)
        {
            int countLeftDownCoinsPrint = 0;
            for(int i = 0; i < k_InARowToWin - 1; i++)
            {
                if ((i_chosenRow + i) <= (m_Rows - 1) && (i_chosenColumn - i) >= k_FirstCol)
                {
                    if (m_Board[i_chosenRow + i, i_chosenColumn - i] == i_coinPrint)
                    {
                        countLeftDownCoinsPrint++;
                    }
                    else break;
                }
                else break;
            }
            return countLeftDownCoinsPrint;
        }

        public int GetRightUpCountOfCoinPrint(char i_coinPrint, int i_chosenColumn, int i_chosenRow)
        {
            int countRightUpCoinsPrint = 0;
            for (int i = 0; i < k_InARowToWin - 1; i++)
            {
                if ((i_chosenRow - i) >= k_FirstRow && (i_chosenColumn + i) <= (m_Cols - 1))
                {
                    if (m_Board[i_chosenRow - i, i_chosenColumn + i] == i_coinPrint)
                    {
                        countRightUpCoinsPrint++;
                    }
                    else break;
                }
                else break;
            }
            return countRightUpCoinsPrint;
        }

        public bool CheckIfFourInOppositeDiagonal(char i_coinPrint, int i_chosenColumn, int i_chosenRow)
        {
            int countLeftUpCoinMatch = GetLeftUpCountOfCoinPrint(i_coinPrint, i_chosenColumn - 1, i_chosenRow - 1);
            int countRightDownCoinMatch = GetRightDownCountOfCoinPrint(i_coinPrint, i_chosenColumn + 1, i_chosenRow + 1);
            int coinsCount = 1 + countLeftUpCoinMatch + countRightDownCoinMatch;
            return coinsCount >= k_InARowToWin;
        }

        public int GetLeftUpCountOfCoinPrint(char i_coinPrint, int i_chosenColumn, int i_chosenRow)
        {
            int countLeftUpCoinsPrint = 0;
            for (int i = 0; i < k_InARowToWin - 1; i++)
            {
                if ((i_chosenRow - i) >= k_FirstRow && (i_chosenColumn - i) >= k_FirstCol)
                {
                    if (m_Board[i_chosenRow - i, i_chosenColumn - i] == i_coinPrint)
                    {
                        countLeftUpCoinsPrint++;
                    }
                    else break;
                }
                else break;
            }
            return countLeftUpCoinsPrint;
        }

        public int GetRightDownCountOfCoinPrint(char i_coinPrint, int i_chosenColumn, int i_chosenRow)
        {
            int countRightDownCoinsPrint = 0;
            for (int i = 0; i < k_InARowToWin - 1; i++)
            {
                if ((i_chosenRow + i) <= (m_Rows - 1) && (i_chosenColumn + i) <= (m_Cols - 1))
                {
                    if (m_Board[i_chosenRow + i, i_chosenColumn + i] == i_coinPrint)
                    {
                        countRightDownCoinsPrint++;
                    }
                    else break;
                }
                else break;
            }
            return countRightDownCoinsPrint;
        }
         public bool CheckIfWin(Player i_playingPlayer, int i_chosenColumn)
        {
            int upperFullRow = GetUpperFullRow(i_chosenColumn);
            bool isFourInARow = CheckIfFourInARow(i_playingPlayer.CoinPrint, upperFullRow, i_chosenColumn);
            
            return isFourInARow;
        }

        public int GetUpperFullRow(int i_chosenColumn)
        {
            int upperFullRow = 0;
            for(int i = 0; i < m_Rows; i++)
            {
                if(m_Board[i, i_chosenColumn] != k_EmptyCell)
                {
                    upperFullRow = i;
                    return upperFullRow;
                }
            }
            return upperFullRow;
        }        
    }
}
