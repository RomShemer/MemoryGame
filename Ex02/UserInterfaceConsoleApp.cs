using System;
using Ex02.ConsoleUtils;
using Board;
using static Board.GameBoard;
using System.Text;
using System.Drawing;
using System.Collections.Generic;
using System.Threading;

namespace UIConsoleApp
{
    public class UserInterfaceConsoleApp
    {
        private const string k_ExitGame = "Q";
        private const int k_Yes = 1;
        private const int k_No = 2;
        private const int k_MinHeight = 4;
        private const int k_MinWidth = 4;
        private const int k_MaxHeight = 6;
        private const int k_MaxWidth = 6;
        private const int k_IgnorePoint = -1;
        private char[] m_CardsValuesInBoard;

        public char[] CardsValues
        {
            set
            {
                m_CardsValuesInBoard = value;
            }
        }

        public string GetUserName()
        {
            string firstPlayerName;

            Ex02.ConsoleUtils.Screen.Clear();
            Console.WriteLine(value: "Please enter your name please:");
            firstPlayerName = Console.ReadLine();

            return firstPlayerName;
        }

        public int GetNumberOfPlayers()
        {
            int numberOfPlayers;
            string input;

            Console.WriteLine(value: "Please choose number of players \n (1) for one player vs computer \n (2) for two players");
            input = Console.ReadLine();
            numberOfPlayers = validateNumberOfPlayersInput(io_input: ref input);

            return numberOfPlayers;
        }

        public void GetBoardDimensions(out int o_boardHeight, out int o_boardWidth)
        {
            string input;
            string message = string.Format(format: "\nNote:\nboard width should be in the range {0} - {1}" +
                                                   ",\nboard height should be in the range {2} - {3},\nand sum of cards in the board should be even number\n"
                                                                   ,
                                                                   args: new object[] { k_MinWidth, k_MaxWidth, k_MinHeight, k_MaxHeight });
            Console.WriteLine(value: message);
            Console.WriteLine(value: "Please enter board width: ");
            input = Console.ReadLine();
            o_boardWidth = validateDimension(io_input: ref input, i_rowOrCol: "columns", i_upperLimit: k_MaxWidth, i_lowerLimit: k_MinWidth);
            Console.WriteLine(value: "Please enter board length: ");
            input = Console.ReadLine();
            o_boardHeight = validateDimension(io_input: ref input, i_rowOrCol: "rows", i_upperLimit: k_MaxHeight, i_lowerLimit: k_MinHeight);

        }

        public Point GetValidCardFormat(ref bool io_isExit, int i_numberOfRows, int i_numberOfColumns) 
        {
            string input;
            Point cardPosition = new Point(k_IgnorePoint,k_IgnorePoint);

            Console.WriteLine(value: "Please enter the position of the card you want to show, if you want to exit the game press 'Q':");
            input = Console.ReadLine();
            validateInputCardPosition(ref input, i_numberOfRows, i_numberOfColumns, ref io_isExit);
            if (io_isExit == false)
            {
                cardPosition = convertCardPositionToInt(i_cardPosition: input);
            }

            return cardPosition;
        }

        public void PrintErrorMessage(string i_message)
        {
            if (string.IsNullOrEmpty(value: i_message) == false)
            {
                Console.WriteLine(value: i_message);
            }
            else
            {
                return;
            }
        }

        public void PrintMatchMessage(bool i_isMatch, string i_currentPlayerName, int i_playerScore)
        {
            string message = i_isMatch ? "It's a match! :)" : "Not a match :(";
            Console.WriteLine(value: $"{i_currentPlayerName}, {message}\n{i_currentPlayerName}'s score: {i_playerScore}");
        }

        public void PrintExitMessage(string i_playerName)
        {
            Console.WriteLine(value: $"{i_playerName} has chosen to exit the game :(");
        }

        public bool AskIfWantToPlayAgain()
        {
            int integerInput = 0;
            string input;
            bool validInput = false;
            bool isNewGame;

            while (validInput == false)
            {
                Ex02.ConsoleUtils.Screen.Clear();
                Console.WriteLine(value: "Do you want to play again?\n(1) yes\n(2) no");
                input = Console.ReadLine();
                inputToInteger(io_input: ref input, o_output: out integerInput);
                validInput = validateInputForPlayAgain(i_input: integerInput);
            }

            isNewGame = (integerInput == k_Yes);

            return isNewGame;
        }

        public void PrintWinnersMessage(List<string> i_winnersNames, bool i_isTie, int i_winnerScore)
        {
            StringBuilder winnersMessage = new StringBuilder();
            string message;

            if(i_isTie == true)
            {
                winnersMessage.Append(value: "It's a tie!");
                winnersMessage.AppendLine(value: " ");
                foreach (string playerName in i_winnersNames)
                {
                    winnersMessage.Append(value: $"{playerName}, ");
                }
                message = $"are the winners with the highest score of {i_winnerScore} points!";
            }
            else
            {
                message = $"{i_winnersNames[index: 0]} is the winner! with the highest score of {i_winnerScore} points!";
            }

            winnersMessage.Append(value: message);
            Console.WriteLine(value: winnersMessage.ToString());
            Thread.Sleep(millisecondsTimeout: 2200);
        }

        public void PrintEndingGame()
        {
            Thread.Sleep(millisecondsTimeout: 400);
            Ex02.ConsoleUtils.Screen.Clear();
            Console.WriteLine(value: "Game Over! Thank you for playing.");
            Thread.Sleep(millisecondsTimeout: 2000);
        }

        private bool validateInputForPlayAgain(int i_input)
        {
            return (i_input >= k_Yes) && (i_input <= k_No);
        }

        private void inputToInteger(ref string io_input, out int o_output)
        {
            bool isInputInteger = int.TryParse(s: io_input, result: out o_output);

            while (isInputInteger == false)
            {
                Console.WriteLine(value: "Invalid Input, Input must be an integer, try again");
                io_input = Console.ReadLine();
                isInputInteger = int.TryParse(s: io_input, result: out o_output);
            }
        }

        private int validateDimension(ref string io_input, string i_rowOrCol, int i_upperLimit, int i_lowerLimit)
        {
            int dimension;
            bool isValidValue;

            inputToInteger(io_input: ref io_input, o_output: out dimension);
            isValidValue = dimension >= i_lowerLimit && dimension <= i_upperLimit;
            while (!isValidValue)
            {
                Console.WriteLine(format: "Invalid Input,{0} dimensions value should be in the range of {1} - {2}", arg0: i_rowOrCol, arg1: i_lowerLimit, arg2: i_upperLimit);
                io_input = Console.ReadLine();
                inputToInteger(io_input: ref io_input, o_output: out dimension);
                isValidValue = dimension >= i_lowerLimit && dimension <= i_upperLimit;
            }

            return dimension;
        }

        private int validateNumberOfPlayersInput(ref string io_input)
        {
            bool isValidInput;
            int numberOfPlayers;

            inputToInteger(io_input: ref io_input, o_output: out numberOfPlayers);
            isValidInput = numberOfPlayers == 1 || numberOfPlayers == 2;
            while (!isValidInput)
            {
                Console.WriteLine(value: "Invalid Input, Number of Players is 1 or 2, try again");
                io_input = Console.ReadLine();
                inputToInteger(io_input: ref io_input, o_output: out numberOfPlayers);
                isValidInput = numberOfPlayers == 1 || numberOfPlayers == 2;
            }
            return numberOfPlayers;
        }

        private bool checkIfExit(string i_input)
        {
            bool isExitPressed = (i_input).ToUpper() == k_ExitGame;
            return isExitPressed;
        }

        private Point convertCardPositionToInt(string i_cardPosition)
        {
            Point cardPosition = new Point();

            int letterValue = char.ToUpper(i_cardPosition[0]) - 'A';
            cardPosition.X = int.Parse(i_cardPosition[1].ToString()) - 1;
            cardPosition.Y = letterValue;

            return cardPosition;
        }

        private void validateInputCardPosition(ref string io_input, int i_numberOfRows, int i_numberOfColums, ref bool io_isExit)
        {
            string error;
            char letterUpperLimitColum = (char)('A' + i_numberOfColums);
            char letterUpperLimitRow = (char)('0' + i_numberOfRows);
            bool validInputLength;
            bool validColumnCardPosition; 
            bool validRowCardPosition;
            bool validInput = isInputCardPositionInPositionFormat(io_input, i_numberOfRows, i_numberOfColums, out validInputLength, out validColumnCardPosition, out validRowCardPosition);
            io_isExit = checkIfExit(io_input);

            while (!validInput)
            {
                if (io_isExit)
                {
                    break;
                }
                if (!validInputLength)
                {
                    error = "valid cardPosition input should be 2 characters";
                }
                else if (!validRowCardPosition)
                {
                    error = string.Format(format: "row card Position should be in the range of 1 - {0}", letterUpperLimitRow);
                }
                else
                {
                    error = string.Format(format: "column card Position value should be between A - {0}", (char)(letterUpperLimitColum-1));
                }
                Console.WriteLine(format: "Invalid Input, {0}, try again", arg0: error);
                io_input = Console.ReadLine();

                validInput = isInputCardPositionInPositionFormat(io_input, i_numberOfRows, i_numberOfColums, out validInputLength, out validColumnCardPosition, out validRowCardPosition);
                io_isExit = checkIfExit(i_input: io_input);
            }
        }

        private bool isInputCardPositionInPositionFormat(string i_input, int i_numberOfRows, int i_numberOfColumns,
                                              out bool io_validInputLength, out bool o_validColumnCardPosition, out bool o_validRowCardPosition)
        {
            char letterUpperLimitColum = (char)('A' + i_numberOfColumns);
            char letterUpperLimitRow = (char)('0' + i_numberOfRows);
            io_validInputLength = i_input.Length == 2;

            if (io_validInputLength == true)
            {
                o_validColumnCardPosition = char.ToUpper(i_input[0]) < letterUpperLimitColum;
                o_validRowCardPosition = i_input[1] <= letterUpperLimitRow && i_input[1] > '0';
            }
            else
            {
                o_validColumnCardPosition = false;
                o_validRowCardPosition = false; 
            }

            return io_validInputLength && o_validColumnCardPosition && o_validRowCardPosition;
        }

        public void PrintBoard(GameBoard i_gameBoard, Point i_printContentCardPosition, string i_currentPlayerName)
        {
            Point defaultPoint = new Point(x: k_IgnorePoint, y: k_IgnorePoint);
            StringBuilder board = buildBoard(i_gameBoard: i_gameBoard, i_printContentCardPosition1: i_printContentCardPosition, i_printContentCardPosition2: defaultPoint);

            Ex02.ConsoleUtils.Screen.Clear();
            Console.WriteLine(value: board);
            printPlayerTurn(i_currentPlayerName: i_currentPlayerName);
        }

        public void PrintBoard(GameBoard i_gameBoard, Point i_printContentCardPosition1, Point i_printContentCardPosition2)
        {
            StringBuilder board = buildBoard(i_gameBoard: i_gameBoard, i_printContentCardPosition1: i_printContentCardPosition1, i_printContentCardPosition2: i_printContentCardPosition2);

            Ex02.ConsoleUtils.Screen.Clear();
            Console.WriteLine(value: board);
        }

        public void PrintBoard(GameBoard i_gameBoard, string i_currentPlayerName)
        {
            PrintBoard(i_gameBoard: i_gameBoard);
            printPlayerTurn(i_currentPlayerName: i_currentPlayerName);
        }

        public void PrintBoard(GameBoard i_gameBoard)
        {
            Point defaultPoint = new Point(x: k_IgnorePoint, y: k_IgnorePoint);

            StringBuilder board = buildBoard(i_gameBoard: i_gameBoard, i_printContentCardPosition1: defaultPoint, i_printContentCardPosition2: defaultPoint);
            Ex02.ConsoleUtils.Screen.Clear();
            Console.WriteLine(value: board);
        }

        private StringBuilder buildBoard(GameBoard i_gameBoard, Point i_printContentCardPosition1, Point i_printContentCardPosition2)
        {
            string cardValue;
            int rowLength = 4 * i_gameBoard.Width + 1;
            int columnLength = 2 * i_gameBoard.Height + 2;
            StringBuilder rowSeparator = new StringBuilder(capacity: rowLength);
            StringBuilder board = new StringBuilder(capacity: rowLength * columnLength);
            Card[,] matrix = i_gameBoard.MatrixBoard;
            Point position;
            bool printCard1 = isPointToPrint(i_point: i_printContentCardPosition1);
            bool printCard2 = isPointToPrint(i_point: i_printContentCardPosition2);


            rowSeparator.Append(value: "  ");
            rowSeparator.Append(value: '=', repeatCount: rowLength);
            rowSeparator.AppendLine();
            board.Append(value: "   ");
            for (int k = 0; k < i_gameBoard.Width; k++)
            {
                board.AppendFormat(format: " {0}  ", arg0: (char)('A' + k));
            }

            board.AppendLine();
            board.Append(value: rowSeparator);
            for (int i = 0; i < i_gameBoard.Height; i++)
            {
                cardValue = " ";
                board.AppendFormat(format: "{0} ", arg0: i + 1);
                for (int j = 0; j < i_gameBoard.Width; j++)
                {
                    position = new Point(x: i, y: j);
                    if (matrix[i, j].CardDiCovered)
                    {
                        cardValue = m_CardsValuesInBoard[matrix[i, j].CardValue -1].ToString();
                    }
                    if (printCard1 && position == i_printContentCardPosition1)
                    {
                        cardValue = m_CardsValuesInBoard[matrix[i, j].CardValue -1].ToString();
                        printCard1 = false;
                    }
                    else if (printCard2 && position == i_printContentCardPosition2)
                    {
                        cardValue = m_CardsValuesInBoard[matrix[i, j].CardValue -1].ToString();
                        printCard2 = false;
                    }
                    board.AppendFormat(format: "| {0} ", arg0: cardValue);
                    cardValue = " ";
                }
                board.Append(value: "|\n");
                board.Append(value: rowSeparator);
            }

            return board;
        }

        private bool isPointToPrint(Point i_point)
        {
            bool printPoint = i_point.X != k_IgnorePoint && i_point.Y != k_IgnorePoint;
            return printPoint;
        }

        private void printPlayerTurn(string i_currentPlayerName)
        {
            Console.WriteLine(value: $"{i_currentPlayerName}'s turn:\n");
        }
    }
}