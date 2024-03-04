using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BattleshipsTheGame
{
    internal class Bot : Player
    {
        public Bot() : base("Bot")
        {}
        public override void PlaceShips()
        {
            Random rand = new Random();
            /*4,3,3,2,2,2,1,1,1,1*/
            List<int> ship_sizes = new List<int> { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };
            foreach (int ship_size in ship_sizes)
            {
                Vec2i begin = null, end = null;
                while (!CheckShipPlacement(begin, end))
                {
                    Vec2i origin = new Vec2i(rand.Next(0,9), rand.Next(0, 9));
                    Direction direction = ship_size > 1 ? (rand.Next(0, 1) == 0 ? Direction.HORIZONTAL : Direction.VERTICAL) : Direction.HORIZONTAL;
                    Ship _ship = new Ship(origin, direction, ship_size);

                    begin = new Vec2i(_ship.fields[0].Item1, _ship.fields[0].Item2);
                    end = new Vec2i(_ship.fields[_ship.fields.Count - 1].Item1, _ship.fields[_ship.fields.Count - 1].Item2);
                }
                Ship ship = new Ship(begin, end);
                ships.Add(ship);
               
            }
            Console.Clear();
        }
        public override Vec2i GetShotCoords()
        {
            Random rand = new Random();

            Vec2i coords = null;
            while (coords == null)
            {

                Vec2i get = new Vec2i(rand.Next(0, 9), rand.Next(0, 9));
                if (hitBoard[get.x, get.y] != HIT_BOARD.NONE)
                {
                    Console.Write("Nie można strzelić dwa razy w to samo miejsce\ni tak tam nic nie ma");
                    continue;
                }
                coords = get;
            }
            return coords;
        }
        public override void Turn(Player oponent)
        {
            Vec2i shotCoords = GetShotCoords();
            HIT_BOARD shot = oponent.ShotAt(shotCoords);
            hitBoard[shotCoords.x, shotCoords.y] = shot;
        }
       
    }
}
