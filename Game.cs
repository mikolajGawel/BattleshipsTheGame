using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsTheGame
{
    internal class Game
    {
        private Player[] players = null;
        public void Intro()
        {
            Console.Title = "Battleships The Game";
            Console.SetWindowSize(40, 34);
            Console.WriteLine("Statki\n The Game\n @Mikołaj Gaweł 2024\n(Kliknij cokolwiek aby zacząć)");
            Console.ReadKey();
            players = new Player[2];
            for (int i = 0; i < players.Length; i++)
            {
                Console.WriteLine($"Podaj nazwę gracza {i+1}");
                string name = Console.ReadLine();
                players[i] = new Player(name);
            }
        }

        //changes cells that connected vertical and horizontal
        public void ChangeToDestroyed(ref HIT_BOARD[,] board,Vec2i destroyedShipPos)
        {
            //check horizontally
            for (int i = destroyedShipPos.x > 0 ? -1 : 0; 
                i <= (destroyedShipPos.x < 9 ? 1 : 0); i++)
            {
                Vec2i fieldPos = new Vec2i(destroyedShipPos.x + i, destroyedShipPos.y);
                if (board[fieldPos.x,fieldPos.y] == HIT_BOARD.HIT)
                {
                    board[fieldPos.x, fieldPos.y] = HIT_BOARD.DESTROYED;
                    ChangeToDestroyed(ref board, fieldPos);
                }
            }
            for (int i = destroyedShipPos.y > 0 ? -1 : 0;
                i <= (destroyedShipPos.y < 9 ? 1 : 0); i++)
            {
                Vec2i fieldPos = new Vec2i(destroyedShipPos.x, destroyedShipPos.y + i);
                if (board[fieldPos.x, fieldPos.y] == HIT_BOARD.HIT)
                {
                    board[fieldPos.x, fieldPos.y] = HIT_BOARD.DESTROYED;
                    ChangeToDestroyed(ref board, fieldPos);
                }
            }

            for (int x = 0; x < 10; x++)
            {
                for(int y = 0;y < 10; y++)
                {
                    if (board[x, y] == HIT_BOARD.DESTROYED)
                    {
                        for (int i = (x > 0 ? -1 : 0);
                        i < (x < 9 ? 2 : 0); i++)
                            for (int j = y > 0 ? -1 : 0;
                                j < (y < 9 ? 2 : 0); j++)
                            {
                                if (board[x + i, y + j] == HIT_BOARD.NONE)
                                    board[x + i, y + j] = HIT_BOARD.MISS;
                            }
                               
                    }
                }
            }
        }
        public void Turn(Player player,Player oponent)
        {

            Console.Clear();
            Console.WriteLine(player.getName() + " teraz twoja kolej\n " +
                "kliknij cokolwiek aby zacząć");
            Console.ReadKey();
            Renderer.DrawShipsBoard(player.ships,oponent.hitBoard);
            Renderer.DrawHitBoard(player.hitBoard);

            Vec2i shotCoords = Input.GetShotCoords(player.hitBoard);
            HIT_BOARD shot = oponent.ShotAt(shotCoords);
            player.hitBoard[shotCoords.x, shotCoords.y] = shot;
            switch (shot)
            {
                case HIT_BOARD.HIT:
                    Console.WriteLine("Statek przeciwnika został trafiony");
                    break;
                case HIT_BOARD.MISS:
                    Console.WriteLine("Pudło");
                    break;
                case HIT_BOARD.DESTROYED:
                    ChangeToDestroyed(ref player.hitBoard, shotCoords);
                    Console.WriteLine("Statek przeciwnika został zatopiony");
                    break;
                
            }

            Renderer.DrawShipsBoard(player.ships, oponent.hitBoard);
            Renderer.DrawHitBoard(player.hitBoard);
            Console.WriteLine("Kliknij któryś przycisk aby\noddać turę drugiemu graczowi");
            Console.ReadKey();
        }
        /*startup game with current settings prompted in setOptions*/
        public void Run() {
            bool playAgain = true;
            do
            {
                for (int i = 0; i < 2; i++)
                    players[i].placeShips();
                while (players[0].ships.Count != 0 && players[1].ships.Count != 0)
                {
                    for (int i = 0; i < 2; i++)
                        Turn(players[i], players[Math.Abs(i - 1)]);
                }
               
                if (players[0].ships.Count == 0)
                 Console.WriteLine($"Gracz: {players[1].getName()} wygrał");
                else    
                    Console.WriteLine($"Gracz: {players[1].getName()} wygrał");
                

                playAgain = Input.PromptYN("Czy chcesz zagrać ponownie");
            } while (playAgain);
        }
    }
}
