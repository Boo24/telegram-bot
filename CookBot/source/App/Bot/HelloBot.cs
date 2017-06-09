using System;
using System.Collections.Generic;

namespace source.App
{
    class HelloBot : IBot
    {
        List<string> replies = new List<string>() { "Здарова! :)", "Привет! :)", "Салют! :)" };
        private Random random = new Random();

        public string HandleCommand(string message)
        {
            int index = random.Next(replies.Count);
            return replies[index];
        }

    }
}
