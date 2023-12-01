using System.Media;

namespace FTP_console.Misc
{
    internal class ColorConsole
    {
        /// <summary>
        /// colors a string based on the <c>ConsoleColor</c> attribute
        /// </summary>
        /// <param name="String"></param>
        /// <param name="color"></param>
        public void PrintColor(string String, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(String);
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// colors a string based on the <c>ConsoleColor</c> attribute and plays a sound
        /// </summary>
        /// <param name="String"></param>
        /// <param name="color"></param>
        /// <param name="sound"></param>
        public void PrintColor(string String, ConsoleColor color,bool sound)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(String);
            Console.ForegroundColor = ConsoleColor.White;
            if (sound)
            {
                SystemSounds.Exclamation.Play();
            }
        }

        public void SetColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }
    }
}
