using System;
using System.Drawing;

namespace Board
{
    public class GameBoard
    {
        private const int k_EmptyCardCoordinate = 0;
        private readonly Random m_Random = new Random();
        public struct Card
        {
            private int m_Value;
            private bool m_IsCardDiscovered;
            private Point m_MatchCardCoordinate;
            public int CardValue
            {
                get
                {
                    return m_Value;
                }
                set
                {
                    m_Value = value;
                }
            }
            public bool CardDiCovered
            {
                get
                {
                    return m_IsCardDiscovered;
                }
                set
                {
                    m_IsCardDiscovered = value;
                }
            }
            public Point Match
            {
                get
                {
                    return m_MatchCardCoordinate;
                }
                set
                {
                    m_MatchCardCoordinate.X = value.X;
                    m_MatchCardCoordinate.Y = value.Y;
                }
            }
        }

        private int m_Height = 0;
        private int m_Width = 0;
        private int m_DiscoveredCards = 0;
        private Card[,] m_GameBoard = null;

        public int Height
        {
            get
            {
                return m_Height;
            }
        }

        public int Width
        {
            get
            {
                return m_Width;
            }
        }

        public Card[,] MatrixBoard
        {
            get
            {
                return m_GameBoard;
            }
        }

        public int DiscoveredCards
        {
            get
            {
                return m_DiscoveredCards;
            }
        }

        public int GetCardValue(Point i_cardCoordinate)
        {
            return m_GameBoard[i_cardCoordinate.X, i_cardCoordinate.Y].CardValue;
        }

        public Point GetMatchingCardCoordinate(Point i_cardCoordinate)
        {
            return m_GameBoard[i_cardCoordinate.X, i_cardCoordinate.Y].Match;
        }
        
        public void InitializationBoard(int i_height, int i_width)
        {
            m_Height = i_height;
            m_Width = i_width;
            m_GameBoard = new Card[i_height, i_width];
            fillBoard();
        }

        public bool IsBoardDimensionsEven(int i_height, int i_width)
        {
            bool isBoardSizeEven = (i_height * i_width) % 2 == 0;

            return isBoardSizeEven;
        }

        public bool IsEqualsCards(Point i_card1Coordinate, Point i_card2Coordinate)
        {
            Point matchForCard1 = m_GameBoard[i_card1Coordinate.X, i_card1Coordinate.Y].Match;
            bool isEqualsCards = matchForCard1.Equals(obj: i_card2Coordinate);

            if (isEqualsCards)
            {
                m_GameBoard[i_card1Coordinate.X, i_card1Coordinate.Y].CardDiCovered = true;
                m_GameBoard[i_card2Coordinate.X, i_card2Coordinate.Y].CardDiCovered = true;
                m_DiscoveredCards += 2;
            }

            return isEqualsCards;
        }

        private void updateBoard(int i_row, int i_col)
        {
            m_GameBoard[i_row, i_col].CardDiCovered = true;
        }

        public bool CheckIfChosenCardDiscoverded(Point i_chosenCard, out string o_ErrorMessage)
        {
            bool isCardDiscovered = false;

            isCardDiscovered = (m_GameBoard[i_chosenCard.X, i_chosenCard.Y].CardDiCovered == true);
            o_ErrorMessage = (isCardDiscovered == true) ? "Invalid Input, please enter an undiscovered card" : null;

            return isCardDiscovered;
        }

        public bool CheckIfChosenCardDiscoverded(Point i_chosenCard)
        {
            return (m_GameBoard[i_chosenCard.X, i_chosenCard.Y].CardDiCovered == true);
        }

        private void fillBoard()
        {
            Point firstCardIndex = new Point();
            Point secondCardIndex = new Point();

            for (int value = 1; value <= (m_Height * m_Width) / 2; value++)
            {
                firstCardIndex = getValidCardIndexInBoard();
                m_GameBoard[firstCardIndex.X, firstCardIndex.Y].CardValue = value;
                secondCardIndex = getValidCardIndexInBoard();
                m_GameBoard[secondCardIndex.X, secondCardIndex.Y].CardValue = value;
                m_GameBoard[firstCardIndex.X, firstCardIndex.Y].Match = secondCardIndex;
                m_GameBoard[secondCardIndex.X, secondCardIndex.Y].Match = firstCardIndex;
            }
        }

        private Point getValidCardIndexInBoard()
        {
            Point cardIndexInBoard = new Point();
            bool foundValidCardIndex = false;

            while (foundValidCardIndex == false)
            {
                cardIndexInBoard = getRandomCoordinateInBoard();
                foundValidCardIndex = checkIfCardCoordinateIsEmptyInBoard(i_cardIndex: cardIndexInBoard);
            }

            return cardIndexInBoard;
        }

        private bool checkIfCardCoordinateIsEmptyInBoard(Point i_cardIndex)
        {
            return m_GameBoard[i_cardIndex.X, i_cardIndex.Y].CardValue == k_EmptyCardCoordinate;
        }

        private Point getRandomCoordinateInBoard()
        {
            Point index = new Point();
            index.X = m_Random.Next(maxValue: m_Height);
            index.Y = m_Random.Next(maxValue: m_Width);

            return index;
        }
    }
}
