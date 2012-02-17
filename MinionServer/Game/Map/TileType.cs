using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// TODO: This is a hilariously awful way to do this. Refactor plz.
namespace MinionServer.Game.Map
{
    public static class TileType
    {
        public const int NONE = 0;

        public const int PLAIN_LIGHT = 1;
        public const int PLAIN_DARK = 22;

        public const int TOP = 2;
        public const int BOTTOM = 3;
        public const int LEFT = 4;
        public const int RIGHT = 5;

        public const int CORNER_BOTTOM_RIGHT = 6;
        public const int CORNER_BOTTOM_LEFT = 7;
        public const int CORNER_TOP_LEFT = 8;
        public const int CORNER_TOP_RIGHT = 9;

        public const int CLOSED_BOTTOM = 10;
        public const int CLOSED_TOP = 11;
        public const int CLOSED_RIGHT = 12;
        public const int CLOSED_LEFT = 13;

        public const int CORNER_FILLER_TOP_RIGHT = 14;
        public const int CORNER_FILLER_BOTTOM_LEFT = 15;
        public const int CORNER_FILLER_TOP_LEFT = 16;
        public const int CORNER_FILLER_BOTTOM_RIGHT = 17;

        public const int CORNER_FILLER_LEFT = 18;
        public const int CORNER_FILLER_TOP = 19;
        public const int CORNER_FILLER_RIGHT = 20;
        public const int CORNER_FILLER_BOTTOM = 21;
    }
}
