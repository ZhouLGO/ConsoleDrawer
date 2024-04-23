using System;
using ConsoleStage.Tools;
using System.Numerics;

namespace ConsoleStage.MathModel
{
    public class Cube
    {
        public static Vector3 Front => Vector3.UnitZ;
        public static Vector3 Right => Vector3.UnitX;
        public static Vector3 Up => Vector3.UnitY;

        public float Size { get; private set; }
        public Cube(float size)
        {
            Size = size;
        }

        public Vector3 GetSquarePoint(float xUnit, float yUnit, EDirection side, out Vector3 normal)
        {
            Vector3 point = default;
            normal = default;
            float halfLength = Size / 2;
            float x = MathExtendsion.Lerp( -halfLength, halfLength, xUnit);
            float y = MathExtendsion.Lerp( -halfLength, halfLength,yUnit);
            switch (side)
            {
                case EDirection.Forward:
                    point = new Vector3(x, y, halfLength );
                    normal = Front;
                    break;
                case EDirection.Right:
                    point = new Vector3( halfLength, x, y );
                    normal = Right;
                    break;
                case EDirection.Up:
                    point = new Vector3( x, halfLength, y );
                    normal = Up;
                    break;
                case EDirection.Back:
                    point = new Vector3( -x, -y, -halfLength);
                    normal = -Front;
                    break;
                case EDirection.Left:
                    point = new Vector3( -halfLength, -x, -y );
                    normal = -Right;
                    break;
                case EDirection.Down:
                    point = new Vector3( -x, -halfLength, -y);
                    normal = -Up;
                    break;
                default:
                    break;
            }
            return point;
        }
    }
}
