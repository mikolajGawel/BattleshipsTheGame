using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsTheGame
{
    public enum HIT_BOARD
    {
        NONE = 0, MISS, HIT,DESTROYED
    }
    internal class Renderer
    {
        private enum COLOR_MODE
        {
            NORMAL,BOARD,NUMBERS
        };
       
        private static void setColorMode(ConsoleColor foreground,ConsoleColor background)
        {
            Console.BackgroundColor = background;
            Console.ForegroundColor = foreground;
        }
        public static void DrawShipsBoard(List<Ship> ships,HIT_BOARD[,] enemyHitBoard)
        {
            Console.WriteLine("\nPlansza z statkami\n ");
            char[,] board = new char[10,10];
            for (int i = 0;i < 10; i++)
                for (int j = 0; j < 10; j++)
                {
                    board[i,j] = enemyHitBoard[i,j] == HIT_BOARD.MISS? 'O' : enemyHitBoard[i, j] == HIT_BOARD.DESTROYED ? 'x' : (char)0;
                }

            
            ships.ForEach(x => {
                x.fields.ForEach(field =>
                {
                    board[field.Item1, field.Item2] = field.Item3 ? ' ' : 'X';
                });
            });
            setColorMode(ConsoleColor.Black, ConsoleColor.White);

            Console.Write(" ");
            for (int i = 0; i < 10 ; i++)
                Console.Write(" " + i);
            Console.WriteLine(" ");
            for (int i = 0; i < 10; i++)
            {
                setColorMode(ConsoleColor.Black, ConsoleColor.White);
                Console.Write(i);
                for (int j = 0; j < 10; j++)
                {
                    setColorMode(ConsoleColor.White, ConsoleColor.Blue);
                    char c = '~';
                    if (board[j, i] != 0) { 
                        c = board[j, i]; 
                        if(c != 'O' && c != 'x')
                            setColorMode(ConsoleColor.Red, ConsoleColor.Gray);
                        else if(c == 'x')
                        {
                            c = Char.ToUpper(c);
                            setColorMode(ConsoleColor.Red, ConsoleColor.Blue);
                        }

                    }
                    Console.Write(" " + c);
                }
                Console.Write(' ');
                Console.Write("\n");
            }
            setColorMode(ConsoleColor.White,ConsoleColor.Black);

        }
        public static void DrawHitBoard(HIT_BOARD[,] hitboard)
        {
            Console.WriteLine("\nPlansza do strzelania\n ");
            setColorMode(ConsoleColor.Black, ConsoleColor.White);

            setColorMode(ConsoleColor.Black, ConsoleColor.White);

            Console.Write(" ");
            for (int i = 0; i < 10; i++)
                Console.Write(" " + i);
            Console.WriteLine(" ");
            for (int i = 0; i < 10; i++)
            {
                setColorMode(ConsoleColor.Black, ConsoleColor.White);
                Console.Write(i);
                for (int j = 0; j < 10; j++)
                {
                    char icon = ' ';
                    switch (hitboard[j, i])
                    {
                        case HIT_BOARD.NONE:
                            setColorMode(ConsoleColor.White, ConsoleColor.Gray);
                            break;
                        case HIT_BOARD.MISS:
                            icon = '~';
                            setColorMode(ConsoleColor.White, ConsoleColor.Blue);
                            break;
                        case HIT_BOARD.HIT:
                            icon = 'X';
                            setColorMode(ConsoleColor.White, ConsoleColor.Red);
                            break;
                        case HIT_BOARD.DESTROYED:
                            icon = 'X';
                            setColorMode(ConsoleColor.Red, ConsoleColor.Black);
                            break;
                    }
                   
                    Console.Write(" " + icon );
                }
                Console.Write(' ');
                Console.Write("\n");
            }
            setColorMode(ConsoleColor.White, ConsoleColor.Black);
        }
         
    
    }
}
