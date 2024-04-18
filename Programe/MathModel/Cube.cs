using System;
using ConsoleStage.Tools;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ConsoleStage.MathModel
{
    public class Cube
    {
        public static readonly DenseMatrix frontNormal = new DenseMatrix(1, 3, new double[] { 0, 0, 1 });
        public static readonly DenseMatrix rightNormal = new DenseMatrix(1, 3, new double[] { 1, 0, 0 });
        public static readonly DenseMatrix upNormal = new DenseMatrix(1, 3, new double[] { 0, 1, 0 });

        public double Size { get; private set; }
        public Cube(double size)
        {
            Size = size;
        }

        public DenseMatrix GetSquarePoint(double xUnit, double yUnit, EDirection side, out DenseMatrix normal)
        {
            DenseMatrix point = default;
            normal = default;
            double halfLength = Size / 2;
            double x = MathExtendsion.Lerp( -halfLength, halfLength, xUnit);
            double y = MathExtendsion.Lerp( -halfLength, halfLength,yUnit);
            switch (side)
            {
                case EDirection.Forward:
                    point = new DenseMatrix(1, 3, new double[] { x, y, halfLength });
                    normal = frontNormal;
                    break;
                case EDirection.Right:
                    point = new DenseMatrix(1, 3, new double[] { halfLength, x, y });
                    normal = rightNormal;
                    break;
                case EDirection.Up:
                    point = new DenseMatrix(1, 3, new double[] { x, halfLength, y });
                    normal = upNormal;
                    break;
                case EDirection.Back:
                    point = new DenseMatrix(1, 3, new double[] { -x, -y, -halfLength });
                    normal = -frontNormal;
                    break;
                case EDirection.Left:
                    point = new DenseMatrix(1, 3, new double[] { -halfLength, -x, -y });
                    normal = -rightNormal;
                    break;
                case EDirection.Down:
                    point = new DenseMatrix(1, 3, new double[] { -x, -halfLength, -y });
                    normal = -upNormal;
                    break;
                default:
                    break;
            }
            return point;
        }
    }
}
