using Board;
using PlayerNameSpace;
using System;
using System.Collections.Generic;
using UIConsoleApp;
using System.Drawing;
using Game;
using System.Threading;

namespace UiManager
{
    public class UserInterfaceManager
    {
        private const int k_NumberOfPlayer = 2;
        private const string k_ExitGame = "Q";
        private const bool k_HumanPlayer = false;
        private const bool k_ComputerPlayer = true;
        private readonly Player[] m_Players = new Player[k_NumberOfPlayer];
        private bool m_IsExitWasChosen = false;
        private bool m_IsGameEnded = false;
        private GameLogic m_GameLogic;
        private Random m_Random = new Random();
        private char[] m_CardsValues;
        private readonly UserInterfaceConsoleApp r_UiConsoleApp = new UserInterfaceConsoleApp();

        public GameBoard GameBoard
        {
            get
            {
                return m_GameLogic.Board;
            }
        }
        
        public void NewGame()
        {
            bool isNewGame = true;
            List<int> winners;

            while (isNewGame == true)
            {
                initializationGame();
                runGame();
                if (m_IsGameEnded == true)
                {
                    if(m_IsExitWasChosen == false)
                    {
                        winners = m_GameLogic.GetWinnersIndex();
                        getWinnersMessage(i_winnersIndex: winners);
                    }
                    r_UiConsoleApp.PrintEndingGame();
                    isNewGame = r_UiConsoleApp.AskIfWantToPlayAgain();
                }
            }

            r_UiConsoleApp.PrintEndingGame();
        }
        
        private void initializationGame()
        {
            int boardHeight;
            int boardWidth;
            m_IsGameEnded = false;
            m_IsExitWasChosen = false;
            m_GameLogic = new GameLogic();
            setPlayers();
            getValidBoardDimensionsFromUser(o_BoardHeight: out boardHeight, o_BoardWidth: out boardWidth);
            m_GameLogic.InitializationGame(i_height: boardHeight, i_width: boardWidth, i_numberOfPlayers: k_NumberOfPlayer);
            createCardsValuesArrayForCards();
        }

        private void setPlayers()
        {
            int numberOfPlayers;

            bool isValidNmberOfPlayer;
            string playerName = r_UiConsoleApp.GetUserName();
            m_Players[0] = new Player(i_name: playerName, i_isComputer: k_HumanPlayer);
            numberOfPlayers = r_UiConsoleApp.GetNumberOfPlayers();
            isValidNmberOfPlayer = (numberOfPlayers == k_NumberOfPlayer);
            if (isValidNmberOfPlayer)
            {
                playerName = r_UiConsoleApp.GetUserName();
                m_Players[1] = new Player(i_name: playerName, i_isComputer: k_HumanPlayer);
            }
            else
            {
                m_Players[1] = new Player(i_name: "computer", i_isComputer: k_ComputerPlayer);
            }
        }

        private void createCardsValuesArrayForCards()
        {
            Random random = new Random();
            List<char> abcList = new List<char>(collection: "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray());
            int numberCoupleOfCards = (m_GameLogic.Board.Height * m_GameLogic.Board.Width) / 2;
            m_CardsValues = new char[numberCoupleOfCards];
            int randomCharIndex;

            for (int i = 1; i <= numberCoupleOfCards; i++)
            {
                randomCharIndex = random.Next(minValue: 0, maxValue: abcList.Count);
                m_CardsValues[i-1] = abcList[index: randomCharIndex];
                abcList.RemoveAt(index: randomCharIndex);
            }

            r_UiConsoleApp.CardsValues = m_CardsValues;
        }

        private void getValidBoardDimensionsFromUser(out int o_BoardHeight, out int o_BoardWidth)
        {
            bool validDimensions;

            r_UiConsoleApp.GetBoardDimensions(o_boardHeight: out o_BoardHeight, o_boardWidth: out o_BoardWidth);
            validDimensions = m_GameLogic.Board.IsBoardDimensionsEven(i_height: o_BoardHeight, i_width: o_BoardWidth);

            while (!validDimensions)
            {
                r_UiConsoleApp.PrintErrorMessage(i_message: "Invalid Input, total cells in the game should be even number");
                r_UiConsoleApp.GetBoardDimensions(o_boardHeight: out o_BoardHeight, o_boardWidth: out o_BoardWidth);
                validDimensions = m_GameLogic.Board.IsBoardDimensionsEven(i_height: o_BoardHeight, i_width: o_BoardWidth);
            }
        }

        private void runGame()
        {
            while (m_IsGameEnded == false)
            {
                r_UiConsoleApp.PrintBoard(i_gameBoard: m_GameLogic.Board, i_currentPlayerName: m_Players[m_GameLogic.CurrentPlayer].Name);
                turn(io_Player: m_Players[m_GameLogic.CurrentPlayer]);
                m_IsGameEnded = m_GameLogic.IsGameOver() || m_IsExitWasChosen;
            }
        }

        private void turn(Player io_Player)
        {
            Point[] chosenCards = new Point[2];
            bool isCardsMatch = false;

            if (io_Player.IsComputer == true)
            {
                chosenCards = m_GameLogic.GetComputerCards();
                r_UiConsoleApp.PrintBoard(i_gameBoard: m_GameLogic.Board, i_printContentCardPosition: chosenCards[0], i_currentPlayerName: io_Player.Name);
                Thread.Sleep(millisecondsTimeout: 2000);
                r_UiConsoleApp.PrintBoard(i_gameBoard: m_GameLogic.Board, i_printContentCardPosition1: chosenCards[0], i_printContentCardPosition2: chosenCards[1]);
            }
            else
            {
                chosenCards[0] = getValidCard();
                r_UiConsoleApp.PrintBoard(i_gameBoard: m_GameLogic.Board, i_printContentCardPosition: chosenCards[0], i_currentPlayerName: io_Player.Name);
                chosenCards[1] = getValidCard(i_prevChosenCardCoordinate: chosenCards[0]);
                r_UiConsoleApp.PrintBoard(i_gameBoard: m_GameLogic.Board, i_printContentCardPosition1: chosenCards[0], i_printContentCardPosition2: chosenCards[1]);
            }

            if (m_IsExitWasChosen == false)
            {
                m_GameLogic.Turn(i_ioPlayer: io_Player, i_chosenCards: chosenCards, o_isCardsMatch: out isCardsMatch);
                r_UiConsoleApp.PrintMatchMessage(i_isMatch: isCardsMatch, i_currentPlayerName: io_Player.Name, i_playerScore: io_Player.Score);
                Thread.Sleep(millisecondsTimeout: 2000);
                r_UiConsoleApp.PrintBoard(i_gameBoard: m_GameLogic.Board);
            }
            else
            {
                r_UiConsoleApp.PrintBoard(i_gameBoard: m_GameLogic.Board);
                r_UiConsoleApp.PrintExitMessage(i_playerName: io_Player.Name);
                Thread.Sleep(millisecondsTimeout: 2000);
            }


        }
        
        private Point getValidCard()
        {
            Point cardChoice = new Point();
            bool isChocenCardlDiscoverded = true;
            string errorMessage = null;

            while (isChocenCardlDiscoverded && !m_IsExitWasChosen)
            {
                cardChoice = r_UiConsoleApp.GetValidCardFormat(io_isExit: ref m_IsExitWasChosen, i_numberOfRows: m_GameLogic.Board.Height, i_numberOfColumns: m_GameLogic.Board.Width);
                if (m_IsExitWasChosen == false)
                {
                    isChocenCardlDiscoverded = m_GameLogic.Board.CheckIfChosenCardDiscoverded(i_chosenCard: cardChoice, o_ErrorMessage: out errorMessage);
                    r_UiConsoleApp.PrintErrorMessage(i_message: errorMessage);
                }
            }

            return cardChoice;
        }

        private Point getValidCard(Point i_prevChosenCardCoordinate)
        {
            bool isSameCardCoordinate = true;
            Point cardCoordinate = new Point();

            while (isSameCardCoordinate == true && !m_IsExitWasChosen)
            {
                cardCoordinate = getValidCard();
                if (m_IsExitWasChosen == false)
                {
                    isSameCardCoordinate = cardCoordinate.Equals(obj: i_prevChosenCardCoordinate);
                    if (isSameCardCoordinate)
                    {
                        r_UiConsoleApp.PrintErrorMessage(i_message: "Invalid Input, please enter an unselected card");
                    }
                }
            }

            return cardCoordinate;
        }

        private void getWinnersMessage(List<int> i_winnersIndex)
        {
            bool isTie = (i_winnersIndex.Count > 1);
            int highestScore = m_Players[i_winnersIndex[index: 0]].Score;
            List<string> winnersNames = new List<string>();

            foreach (int index in i_winnersIndex)
            {
                winnersNames.Add(item: m_Players[index].Name);
            }

            r_UiConsoleApp.PrintWinnersMessage(i_winnersNames: winnersNames, i_isTie: isTie, i_winnerScore: highestScore);
        }
    }
}
