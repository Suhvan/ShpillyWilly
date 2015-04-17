using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Utils
{
    class DepthHelper
    {
        public static int baseZLevel = -1;

        public static Vector3 SetBaseObjectPosition(Vector3 pos)
        {
            pos.z = baseZLevel;
            return pos;
        }

        public static Vector3 AdjustSpritePosition(Vector3 spritePos, float parentY)
        {
            spritePos.z = parentY-10;
            return spritePos;
        }
    }
}
