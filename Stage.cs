using ConsoleStage.MathModel;
using ConsoleStage.Tools;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Spatial.Euclidean;
using MathNet.Spatial.Units;
using System;
using System.Threading.Tasks;

namespace ConsoleStage
{
    internal class Stage
    {
        static void Main(string[] args)
        {
            DrawCube();
            //DrawDoughnut();
        }
        static void DrawCube()
        {
            Cube cubeMathModel = new Cube(15);
            Canvas canvas = new Canvas(50, 28, 50, 5);
            Light light = new Light(1, 1, 0);//平行光的朝向
            Shader shader = new Shader();

            float xAxisRotationRadians = 0;
            float yAxisRotationRadians = 0;
            float zAxisRotationRadians = 0;

            Console.CursorVisible = false;
            int stepLength = 50;
            while (true)
            {
                canvas.CleanCanvas();

                Parallel.For(0, stepLength, xUnit =>
                {
                    Parallel.For(0, stepLength, yUnit =>
                    {
                        Parallel.For(0, 6, side =>
                        {
                            EDirection eside = (EDirection)side;

                            DenseMatrix xAxisRotationMatrix = Matrix3D.RotationAroundXAxis(Angle.FromRadians(xAxisRotationRadians));
                            DenseMatrix yAxisRotationMatrix = Matrix3D.RotationAroundYAxis(Angle.FromRadians(yAxisRotationRadians));
                            DenseMatrix zAxisRotationMatrix = (DenseMatrix)Matrix3D.RotationAroundZAxis(Angle.FromRadians(zAxisRotationRadians));

                            DenseMatrix rotationMatrix = yAxisRotationMatrix * xAxisRotationMatrix * zAxisRotationMatrix;


                            DenseMatrix surfacePoint = cubeMathModel.GetSquarePoint(((double)xUnit) / stepLength, ((double)yUnit) / stepLength, eside, out DenseMatrix normal);
                            DenseMatrix rotatedSurfacePoint = surfacePoint * rotationMatrix;
                            DenseMatrix rotatedNormal = normal * rotationMatrix;

                            DenseMatrix screenPoint = canvas.CalculateScreenPoint(rotatedSurfacePoint);

                            if (canvas.CheckPointInFonrt(screenPoint))
                            {
                                if (shader.CalculateLight(rotatedNormal, light, out char pixelChar))
                                {
                                    canvas.UpdatePixel(screenPoint, pixelChar);
                                }
                            }
                        });
                    });
                });

                canvas.Draw();

                xAxisRotationRadians += 0.03f;
                yAxisRotationRadians += 0.01f;
                zAxisRotationRadians += 0.02f;
            }
        }

        static void DrawDoughnut()
        {
            Doughnut doughnutMathModel = new Doughnut(10, 5);
            Canvas canvas = new Canvas(50, 28, 50, 5);
            //面向屏幕，xyz的正半轴分别是：左、下、垂直屏幕向内
            Light light = new Light(1, 2, 0);//平行光的朝向
            Shader shader = new Shader();

            float zAxisRotationRadians = 0;
            float xAxisRotationRadians = 0;

            Console.CursorVisible = false;

            while (true)
            {
                canvas.CleanCanvas();

                int stepLength = 4;
                Parallel.For(0, 360 / stepLength, circleEachAngle =>
                {
                    Parallel.For(0, 360, yAxisEachAngle =>
                    {
                        DenseMatrix yAxisRotationMatrix = Matrix3D.RotationAroundYAxis(Angle.FromDegrees(yAxisEachAngle));
                        DenseMatrix xAxisRotationMatrix = Matrix3D.RotationAroundXAxis(Angle.FromRadians(xAxisRotationRadians));
                        DenseMatrix zAxisRotationMatrix = (DenseMatrix)Matrix3D.RotationAroundZAxis(Angle.FromRadians(zAxisRotationRadians));

                        DenseMatrix samplePoint = doughnutMathModel.GetPointRotatingAroundAxis(yAxisRotationMatrix, Angle.FromDegrees(circleEachAngle * stepLength).Radians, out var normal);
                        DenseMatrix rotatedSamPoint = (samplePoint * xAxisRotationMatrix * zAxisRotationMatrix);
                        DenseMatrix rotatedNormal = normal * xAxisRotationMatrix * zAxisRotationMatrix;
                        DenseMatrix screenPoint = canvas.CalculateScreenPoint(rotatedSamPoint);

                        if (canvas.CheckPointInFonrt(screenPoint))
                        {
                            if (shader.CalculateLight(rotatedNormal, light, out char pixelChar))
                            {
                                canvas.UpdatePixel(screenPoint, pixelChar);
                            }
                        }
                    });
                });

                canvas.Draw();

                xAxisRotationRadians += 0.075f;
                zAxisRotationRadians += 0.025f;
            }
        }

    }
}

