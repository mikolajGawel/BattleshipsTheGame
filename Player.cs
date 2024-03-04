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
            };
            this.name = name;
        }
        public virtual Vec2i GetShotCoords()
        {

            Vec2i coords = null;
            while (coords == null)
            {
                Console.WriteLine("Podaj koordynaty do ataku:");
                Vec2i get = Input.GetCoords();
                if (hitBoard[get.x, get.y] != HIT_BOARD.NONE)
                {
                    Console.Write("Nie można strzelić dwa razy w to samo miejsce\ni tak tam nic nie ma");
                    continue;
                }
                coords = get;
            }
            return coords;
        }
        public virtual void PlaceShips()
        {
            Console.Clear();

            Console.WriteLine($"Gracz {name} podaje pozycje statków\nkliknij cokolwiek aby zacząć");
            Console.ReadKey();

            List<int> ship_sizes = new List<int>{ 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };
            foreach(int ship_size in ship_sizes)
            {
                Vec2i begin = null, end = null;
                while (!CheckShipPlacement(begin, end))
                {
                    Renderer.DrawShipsBoard(ships, new HIT_BOARD[10, 10]);
                    Console.WriteLine($"Podaj pozycje dla statku z {ship_size} " + (ship_size > 1 ? "masztami" : "masztem"));
                    
                    Vec2i origin = Input.GetCoords();
                    Direction direction = ship_size > 1?  Input.GetDirection() : Direction.HORIZONTAL;
                    
                    Ship _ship = new Ship(origin,direction,ship_size);
                    
                    begin = new Vec2i(_ship.fields[0].Item1, _ship.fields[0].Item2);
                    end = new Vec2i(_ship.fields[_ship.fields.Count - 1].Item1, _ship.fields[_ship.fields.Count-1].Item2);

                    Console.Clear();

                    if (!CheckShipPlacement(begin, end))
                        Console.WriteLine("Nieprawidłowe koordynaty dla statku");
                   
                }
                Ship ship = new Ship(begin,end);
                ships.Add(ship);
                Console.WriteLine("Koordynaty statku zostały zakodowane");
            }
            Console.WriteLine("Kliknij cokolwiek aby oddać turę \ndrugiemu graczowi");
            Console.ReadKey();
            Console.Clear();
        }

        //changes cells that connected vertical and horizontal
        public void ChangeToDestroyed(ref HIT_BOARD[,] board, Vec2i destroyedShipPos)
        {
            //check horizontally
            for (int i = destroyedShipPos.x > 0 ? -1 : 0;
                i <= (destroyedShipPos.x < 9 ? 1 : 0); i++)
            {
                Vec2i fieldPos = new Vec2i(destroyedShipPos.x + i, destroyedShipPos.y);
                if (board[fieldPos.x, fieldPos.y] == HIT_BOARD.HIT)
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
                for (int y = 0; y < 10; y++)
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

        public void WaitForTurn()
        {
            Console.Clear();
            Console.WriteLine(GetName() + " teraz twoja kolej\n " +
                "kliknij cokolwiek aby zacząć");
            Console.ReadKey();
        }
        public virtual void Turn(Player oponent)
        {         
            Renderer.DrawShipsBoard(ships, oponent.hitBoard);
            Renderer.DrawHitBoard(hitBoard);

            Vec2i shotCoords = GetShotCoords();
            HIT_BOARD shot = oponent.ShotAt(shotCoords);
            hitBoard[shotCoords.x, shotCoords.y] = shot;
            switch (shot)
            {
                case HIT_BOARD.HIT:
                    Console.WriteLine("Statek przeciwnika został trafiony");
                    break;
                case HIT_BOARD.MISS:
                    Console.WriteLine("Pudło");
                    break;
                case HIT_BOARD.DESTROYED:
                    ChangeToDestroyed(ref hitBoard, shotCoords);
                    Console.WriteLine("Statek przeciwnika został zatopiony");
                    break;

            }

            Renderer.DrawShipsBoard(ships, oponent.hitBoard);
            Renderer.DrawHitBoard(hitBoard);
            if(shot == HIT_BOARD.HIT ||  shot == HIT_BOARD.DESTROYED)
                Turn(oponent);
        }
        public void EndTurn()
        {
            Console.WriteLine("Kliknij któryś przycisk aby\noddać turę drugiemu graczowi");
            Console.ReadKey();
        }
        public string GetName() { return name; }

        public bool CheckShipPlacement(Vec2i begin,Vec2i end)
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

                    if (!ships[i].IsAlive())
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
