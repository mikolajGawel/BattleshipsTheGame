using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsTheGame
{
    internal class Input
    {
        public static Vec2i GetCoords()
        {
            Vec2i result = new Vec2i(-1, -1); 
            while (result.x == -1 || result.y == -1)
            {
                string prompt = "";
                if (result.x < 0) prompt = "Podaj numer kolumny:";
                else if (result.y < 0) prompt = "Podaj numer wiersza";

                Console.WriteLine(prompt);

                char c = Console.ReadKey().KeyChar;
                
                int i = 0;
                if (!int.TryParse($"{c}", out i)) continue;
                if (i < 0 || i >= 10)
                {
                    Console.WriteLine("Wartość nie możę być mniejsza od 0 i większa od 9");
                    continue;
                }
                if(result.x < 0)result.x = i;
                else if(result.y < 0)result.y = i;
                Console.WriteLine();
            }
            //Console.WriteLine($"vec2({result.x}, {result.y})");
            return result;
        }
        

        //pytanie tak czy nie
        public static bool PromptYN(string prompt)
        {
            Console.WriteLine(prompt + " T/N?");
            char anwser = ' ';
            while (anwser != 'n' && anwser != 't')
            {
                string str= Console.ReadKey().KeyChar.ToString().ToLower();
                anwser = str[0];
            }
            return (anwser == 't');
        }
        public static bool PromptBot()
        {
            Console.WriteLine("Wybierz tryb gry\n1.Gra jednoosobowa z botem\n2.Gra wieloosobowa");
            char anwser = ' ';
            while (anwser != '1' && anwser != '2')
            {
                string str = Console.ReadKey().KeyChar.ToString().ToLower();
                anwser = str[0];
            }
            return (anwser == '1');
        }
        public static Direction GetDirection()
        {
            Console.WriteLine("Położenie statku ma być\npionowe(V) czy poziome(H)");
            char anwser = ' ';
            while (anwser != 'v' && anwser != 'h')
            {
                string str = Console.ReadKey().KeyChar.ToString().ToLower();
                anwser = str[0];
            }
            return (anwser == 'v') ? Direction.VERTICAL : Direction.HORIZONTAL;
        }
    }
}
