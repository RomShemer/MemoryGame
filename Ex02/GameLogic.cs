using Board;
using System;
using PlayerNameSpace;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class GameLogic
    {
        private readonly GameBoard r_GameBoard = new GameBoard();
        private bool m_IsGameOver = false;
        private bool m_IsNextPlayer = true;
        private int m_PlayerTurnIndex = 0;
        private int m_NumberOfPlayer = 0;
        private int[] m_PlayersScoreByIndex;
        private int m_CountSequencesTurnsForComputer = 0;
        private Random m_Random = new Random();
        private readonly Dictionary<Point, int> r_CardMemoryForComputer = new Dictionary<Point, int>();
        public GameBoard Board
        {
            get
            {
                return r_GameBoard;
            }
        }

        public bool NextPlayer
        {
            get
            {
                return m_IsNextPlayer;
            }
        }

        public int CurrentPlayer
        {
            get
            {
                return m_PlayerTurnIndex;
            }
        }
        
        public void InitializationGame(int i_height, int i_width, int i_numberOfPlayers)
        {
            r_GameBoard.InitializationBoard(i_height: i_height, i_width: i_width);
            m_NumberOfPlayer = i_numberOfPlayers;
            m_PlayerTurnIndex = 0;
            m_PlayersScoreByIndex = new int[i_numberOfPlayers];
        }

        public bool IsGameOver()
        {
            m_IsGameOver = r_GameBoard.DiscoveredCards == (r_GameBoard.Height * r_GameBoard.Width);

            return m_IsGameOver;
        }

        public void Turn(Player i_ioPlayer, Point[] i_chosenCards, out bool o_isCardsMatch)
        {
            bool correctChoice;
            o_isCardsMatch = false;
            int cardValue;

            cardValue = r_GameBoard.GetCardValue(i_chosenCards[0]);
            checkIfKnowCardForComputerAndSavingItIfNot(i_chosenCards[0], cardValue);
            cardValue = r_GameBoard.GetCardValue(i_chosenCards[1]);
            checkIfKnowCardForComputerAndSavingItIfNot(i_chosenCards[1], cardValue);
            correctChoice = checkingAndHandlingMatchingCards(i_chosenCards[0],i_chosenCards[1]);
            if (correctChoice)
            {
                i_ioPlayer.Score += 1;
                updatePlayerScore(i_currentPlayer: i_ioPlayer);
                o_isCardsMatch = true;
            }
        }
        
        public Point[] GetComputerCards()
        {
            int cardValue;
            Point[] chosenCards = new Point[2];
            Point matchingCardCoordinate;

            updateSequencesTurnsOfComputer();
            chosenCards[0] = computerTurn();
            cardValue = r_GameBoard.GetCardValue(i_cardCoordinate: chosenCards[0]);
            checkIfKnowCardForComputerAndSavingItIfNot(i_cardCoordinate: chosenCards[0], i_cardValue: cardValue);
            matchingCardCoordinate = r_GameBoard.GetMatchingCardCoordinate(i_cardCoordinate: chosenCards[0]);
            chosenCards[1] = computerSelectSecondCard(i_firstCard: chosenCards[0], i_matchingCardToFirstCard: matchingCardCoordinate);

            return chosenCards;
        }

        public List<int> GetWinnersIndex()
        {
            int highestScore = m_PlayersScoreByIndex.Max();
            List<int> highestScorePlayersIndexList = new List<int>();

            for (int playerIndex = 0; playerIndex < m_PlayersScoreByIndex.Length; playerIndex++)
            {
                if (m_PlayersScoreByIndex[playerIndex] == highestScore)
                {
                    highestScorePlayersIndexList.Add(item: playerIndex);
                }
            }

            return highestScorePlayersIndexList;
        }
       
        private Point computerSelectSecondCard(Point i_firstCard, Point i_matchingCardToFirstCard)
        {
            int maxSequencesAllowed = (r_GameBoard.Width * r_GameBoard.Height)/4;
            bool isMaxSequencesAllowed = (m_CountSequencesTurnsForComputer > maxSequencesAllowed);
            bool isMatchKnown = checkIfKnowCardForComputer(i_cardCoordinate: i_matchingCardToFirstCard);
            bool choseMatchingCard = (isMatchKnown == true) && (isMaxSequencesAllowed == false);

            return (choseMatchingCard == true) ? i_matchingCardToFirstCard : computerTurn(i_firstCard: i_firstCard);
        }

        private Point computerTurn()
        {
            Random random = new Random();
            Point randomCard = new Point();
            bool foundUnDiscoveredCell = false;

            while (foundUnDiscoveredCell == false)
            {
                randomCard.X = random.Next(maxValue: r_GameBoard.Height);
                randomCard.Y = random.Next(maxValue: r_GameBoard.Width);
                foundUnDiscoveredCell = r_GameBoard.CheckIfChosenCardDiscoverded(i_chosenCard: randomCard) == false;
            }

            return randomCard;
        }

        private Point computerTurn(Point i_firstCard)
        {
            Point randomCard = new Point();
            bool isSameCell = true;

            while (isSameCell)
            {
                randomCard = computerTurn();
                isSameCell = (i_firstCard.X == randomCard.X) && (i_firstCard.Y == randomCard.Y);
            }

            return randomCard;
        }

        private void checkIfKnowCardForComputerAndSavingItIfNot(Point i_cardCoordinate, int i_cardValue)
        {
            if (r_CardMemoryForComputer.ContainsKey(key: i_cardCoordinate) == false)
            {
                r_CardMemoryForComputer[i_cardCoordinate] = i_cardValue;
            }
        }

        private bool checkIfKnowCardForComputer(Point i_cardCoordinate)
        {
            bool isCardKnow = false;

            foreach (var pair in r_CardMemoryForComputer)
            {
                if (pair.Key == i_cardCoordinate)
                {
                    isCardKnow = true;
                }
            }

            return isCardKnow;
        }

        private bool checkingAndHandlingMatchingCards(Point i_card1Coordinate, Point i_card2Coordinate)
        {
            bool correctChoice = r_GameBoard.IsEqualsCards(i_card1Coordinate: i_card1Coordinate, i_card2Coordinate: i_card2Coordinate);

            if (correctChoice)
            {
                m_IsNextPlayer = false;
            }
            else
            {
                m_PlayerTurnIndex = (m_PlayerTurnIndex + 1) % m_NumberOfPlayer;
                m_IsNextPlayer = true;
            }

            return correctChoice;
        }

        private void updateSequencesTurnsOfComputer()
        {
            if (m_IsNextPlayer == false)
            {
                m_CountSequencesTurnsForComputer += 1;
            }
            else
            {
                m_CountSequencesTurnsForComputer = 0;
            }
        }

        private void updatePlayerScore(Player i_currentPlayer)
        {
            m_PlayersScoreByIndex[m_PlayerTurnIndex] = i_currentPlayer.Score;
        }
    }
}
