Memory Game - Console Application

Overview
This project is a simple memory game implemented in C#, designed as a console application. The game allows one or two players to match pairs of letters on a grid. It demonstrates object-oriented programming (OOP) principles, array manipulation, and external DLL integration.

Features:
Two-player or single-player against the computer: The game can be played solo against the computer or with two players.
Grid customization: Players can choose the grid size (e.g., 4x4, 6x6), with only even-numbered dimensions allowed.
Turn-based gameplay: Players take turns choosing two grid squares to reveal. If the letters match, they remain visible.
Memory matching mechanics: Matches are kept open, and incorrect pairs are temporarily displayed before being hidden again.
Game progression: The game continues until all pairs are matched, and the player with the most pairs wins.
External Libraries
This project makes use of an external Dynamic Link Library (DLL) and a reference to a standard .NET library:

Ex02.ConsoleUtils.dll:
This external DLL is used for clearing the console screen between moves. It provides the method Screen.Clear() to refresh the console display.
The Ex02.ConsoleUtils.dll file should be placed in the C:\Temp directory for the project to function correctly.
Add this DLL to the project via References > Add Reference > Browse, and select the DLL from the C:\Temp directory.

System.Drawing:
The System.Drawing library is referenced to utilize the existing Point class for managing coordinates of grid cells in the game. 
This class is essential for tracking the position of the player's selections on the board.

How to Play:
The game starts by asking the player to input their name and whether they want to play against the computer or with another player.
Players will then choose the grid size (e.g., 4x4, 6x6).
Each player selects two grid cells to reveal the letters behind them.
If a pair is matched, the letters remain visible. If not, they will be hidden again after a brief delay.
The game ends when all pairs have been matched, and the player with the most matches wins
