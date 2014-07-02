using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelGenerator
{
    interface IDraggableObject
    {
        bool MouseIsOnObject(Vector2 mousePosition);
        float DistanceFromMouse(Vector2 mousePosition);
        Vector2 OffsetFromMouse(Vector2 mousePosition);
        void UpdatePosition(Vector2 newPosition);
    }
}
