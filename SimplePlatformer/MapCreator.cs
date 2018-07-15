using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePlatformer
{
    public class MapCreator
    {
        public static bool Enabled { get; private set; }

        public static void StartDrawing()
        {
            Enabled = true;
        }

        public static void StopDrawing()
        {
            Enabled = false;
        }
    }
}
