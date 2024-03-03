using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsTheGame
{
    internal class Player
    {
        public List<Ship> ships = new List<Ship>();
        public HIT_BOARD[,] hitBoard = new HIT_BOARD[10,10];
        private string name;
        public Player(string name)
        {
            ships = new List<Ship>{
                /*                new Ship(new Vec2i(1,1),Direction.HORIZONTAL,5),
                                new Ship(new Vec2i(3,3),Direction.VERTICAL,1),
                                new Ship(new Vec2i(5,3),Direction.VERTICAL,3)*/
            };
            this.name = name;
        }
        public void placeShips()
        {
            Console.Clear();

            Console.WriteLine($"Gracz {name} podaje pozycje statków\nkliknij cokolwiek aby zacząć");
            Console.ReadKey();
            /*4,3,3,2,2,2,1,1,1,1*/
            List<int> ship_sizes = new List<int>{ 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };
            foreach(int ship_size in ship_sizes)
            {
                Vec2i begin = null, end = null;
                while (!checkShipPlacement(begin, end))
                {
                    Renderer.DrawShipsBoard(ships, new HIT_BOARD[10, 10]);
                    Console.WriteLine($"Podaj pozycje dla statku z {ship_size} " + (ship_size > 1 ? "masztami" : "masztem"));
                    
                    Vec2i origin = Input.GetCoords();
                    Direction direction = ship_size > 1?  Input.GetDirection() : Direction.HORIZONTAL;
                    
                    Ship _ship = new Ship(origin,direction,ship_size);
                    
                    begin = new Vec2i(_ship.fields[0].Item1, _ship.fields[0].Item2);
                    end = new Vec2i(_ship.fields[_ship.fields.Count - 1].Item1, _ship.fields[_ship.fields.Count-1].Item2);

                    Console.Clear();

                    if (!checkShipPlacement(begin, end))
                        Console.WriteLine("Statek został nie prawidłowo ustawiony");
                   
                }
                Ship ship = new Ship(begin,end);
                ships.Add(ship);
                Console.WriteLine("Koordynaty statku zostały zakodowane");
            }
            Console.WriteLine("Kliknij cokolwiek aby oddać turę \ndrugiemu graczowi");
            Console.ReadKey();
            Console.Clear();
        }
        public string getName() { return name; }

        public bool checkShipPlacement(Vec2i begin,Vec2i end)
        {
            if(begin == null || end == null)return false;

            bool[,] obtainedFields = new bool[10,10];
            if(end.x > 9 || end.y > 9)
            {
                return false;
            }
            foreach (var item in ships)
            {
                foreach (var fields in item.fields)
                {
                    obtainedFields[fields.Item1,fields.Item2] = true;
                }
            }
            for (int i = begin.x-(begin.x > 0 ? 1 : 0);i < end.x+(end.x < 9? 2 : 1); i++)
            {
                for(int j = begin.y - (begin.y > 0 ? 1 : 0); j < end.y + (end.y < 9 ? 2 : 1); j++)
                {
                    if (obtainedFields[i, j])
                        return false;
                }
            }
            return true;
        }
        //used from opponent to shoot position
        //returns does anything was hitted
        public HIT_BOARD ShotAt(Vec2i pos)
        {
            for (int i = 0; i < ships.Count; i++)
                foreach (var field in ships[i].fields)
                {
                    if (field.Item1 != pos.x || field.Item2 != pos.y)
                        continue;
                    ships[i].fields.Remove(field); // Remove the hit field
                    //create version of field what was hit 
                    ships[i].fields.Add(new Tuple<int,int,bool>(field.Item1, field.Item2,false));

                    if (!ships[i].isAlive())
                    {
                        ships.RemoveAt(i);
                        return HIT_BOARD.DESTROYED;
                    }
                    return HIT_BOARD.HIT;
                }
            
            return HIT_BOARD.MISS;
        }


    }
}
