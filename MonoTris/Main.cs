#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;

using AppKit;
using Foundation;
#endregion

namespace MonoTris
{
    static class Program
    {
        static void Main(string[] args)
        {
            NSApplication.Init();

            using (var game = new MonoTris())
            {
                game.Run();
            }
        }
    }
}
