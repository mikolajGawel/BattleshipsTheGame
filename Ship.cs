using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsTheGame
{
    internal class Vec2i
    {
        public int x, y;
        public Vec2i(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    enum Direction
    {
        VERTICAL,HORIZONTAL
    }
   
    internal class Ship
    {
        public Direction direction;
        public List<Tuple <int,int,bool>> fields;
        //(Item3) boolean
        //true - parts of shop that are still active
        //false parts that has been destroyed
        public bool isAlive()
        {
            foreach (var field in fields)
            {
                if (field.Item3)
                    return true;
            }         
            return false;
        }

        private Tuple<Vec2i, Vec2i> getDirection(Vec2i value1, Vec2i value2)
        {
            if (value1.x > value2.x || (value1.x == value2.x && value1.y > value2.y))
            {
                int temp = value2.x;
                value2.x = value1.x;
                value1.x = temp;

                temp = value2.y;
                value2.y = value1.y;
                value1.y = temp;
            }

            if (value1.x == value2.x)
                direction = Direction.VERTICAL;
            else if (value1.y == value2.y)
                direction = Direction.HORIZONTAL;
            else
            {
                Console.WriteLine("Statek nie może być położony po przekątnej");
                return null;
            }

            return new Tuple<Vec2i, Vec2i>(value1, value2);
        }
        public Ship(Vec2i originPos,Direction direction,int lenght)
        {
            this.direction = direction;
            fields = new List<Tuple<int, int, bool>>();

            for (int i = 0; i < lenght; i++)
            {
                Vec2i currFieldCoord = new Vec2i(originPos.x, originPos.y);
                currFieldCoord.x += direction == Direction.HORIZONTAL ? i : 0;
                currFieldCoord.y += direction == Direction.VERTICAL ? i : 0;

                fields.Add(new Tuple<int, int, bool>(currFieldCoord.x, currFieldCoord.y, true));
            }
        }
        public Ship(Vec2i startPosition,Vec2i endPosition) {
            if (startPosition.x != endPosition.x && startPosition.y != endPosition.y)
            {
                return;
            }
            

            fields = new List<Tuple<int, int, bool>>();
            int lenght = 0;

            (startPosition,endPosition) = getDirection(startPosition, endPosition);
            

            lenght = Math.Abs(direction == Direction.VERTICAL ? (startPosition.y-endPosition.y) : (startPosition.x-endPosition.x));
            for(int i = 0;i < lenght+1; i++)
                fields.Add(new Tuple<int, int, bool>(startPosition.x + ((direction == Direction.HORIZONTAL)? i : 0),
                    startPosition.y + ((direction == Direction.VERTICAL) ? i : 0), true));
        }
    }
}
