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
        public bool bot;
        public void Intro()
        {
            Console.Title = "Battleships The Game";
            Console.SetWindowSize(40, 34);
            Console.WriteLine("Statki\n The Game\n @Mikołaj Gaweł 2024\n(Kliknij cokolwiek aby zacząć)");
            Console.ReadKey();
            bot = Input.PromptBot();
            players = new Player[2];
            for (int i = 0; i < players.Length; i++)
            {
                if (i == 1 && bot) {
                    players[i] = new Bot();
                    continue;
                }
                Console.WriteLine($"Podaj nazwę gracza {i+1}");
                string name = Console.ReadLine();
                players[i] = new Player(name);
            }
        }

     
        /*startup game with current settings prompted in setOptions*/
        public void Run() {
            bool playAgain = true;
            do
            {
                for (int i = 0; i < 2; i++)
                    players[i].PlaceShips();
                while (players[0].ships.Count != 0 && players[1].ships.Count != 0)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        if(!bot)
                            players[i].WaitForTurn();
                        players[i].Turn(players[Math.Abs(i - 1)]);
                    }
                }
               
                if (players[0].ships.Count == 0)
                 Console.WriteLine($"Gracz: {players[1].GetName()} wygrał");
                else    
                    Console.WriteLine($"Gracz: {players[1].GetName()} wygrał");
                

                playAgain = Input.PromptYN("Czy chcesz zagrać ponownie");
            } while (playAgain);
        }
    }
}
