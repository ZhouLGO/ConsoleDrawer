using System;
using System.Numerics;

namespace ConsoleStage.Tools
{
    public class Light
    {
        public Vector3 LightDirection { get; private set; }
        public Light(float x, float y, float z)
        {
            LightDirection = new Vector3(x, y, z);
        }
    }
}
