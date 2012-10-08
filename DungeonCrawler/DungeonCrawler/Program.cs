using System;

namespace DungeonCrawler
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (DungeonCrawlerGame game = new DungeonCrawlerGame())
            {
                game.Run();
            }
        }
    }
#endif
}

