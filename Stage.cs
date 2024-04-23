using ConsoleStage.MathModel;
using ConsoleStage.Tools;
using System;
using System.Threading.Tasks;
using System.Numerics;

namespace ConsoleStage
{
    internal class Stage
    {
        static void Main(string[] args)
        {
            //DrawDoughnut();
            DrawCube();
        }
        static void DrawCube()
        {
            Cube cubeMathModel = new Cube(15);
            Canvas canvas = new Canvas(50, 28, 50, 5);
            Light light = new Light(1, 0, 1);//平行光的朝向
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

                            Quaternion quaternion = Quaternion.CreateFromYawPitchRoll(yAxisRotationRadians, xAxisRotationRadians, zAxisRotationRadians); /*Matrix3D.RotationAroundXAxis(Angle.FromRadians(xAxisRotationRadians))*/;

                            Vector3 surfacePoint = cubeMathModel.GetSquarePoint(((float)xUnit) / stepLength, ((float)yUnit) / stepLength, eside, out Vector3 normal);
                            Vector3 rotatedSurfacePoint = Vector3.Transform(surfacePoint, quaternion);
                            Vector3 rotatedNormal = Vector3.Transform(normal, quaternion);

                            Vector3 screenPoint = canvas.CalculateScreenPoint(rotatedSurfacePoint);

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

                xAxisRotationRadians += 0.003f;
                yAxisRotationRadians += 0.001f;
                zAxisRotationRadians += 0.002f;
            }
        }

        static void DrawDoughnut()
        {
            Doughnut doughnutMathModel = new Doughnut(10, 5);
            Canvas canvas = new Canvas(50, 28, 50, 5);
            //面向屏幕，xyz的正半轴分别是：左、下、垂直屏幕向内
            Light light = new Light(1, 1, 0);//平行光的朝向
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
                        Quaternion yAxisRotationQ = Quaternion.CreateFromAxisAngle(Vector3.UnitY, yAxisEachAngle);
                        Quaternion xAxisRotationQ = Quaternion.CreateFromAxisAngle(Vector3.UnitX, xAxisRotationRadians);
                        Quaternion zAxisRotationQ = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, zAxisRotationRadians);

                        Vector3 samplePoint = doughnutMathModel.GetPointRotatingAroundAxis(yAxisRotationQ, MathExtendsion.ToRadians(circleEachAngle * stepLength), out var normal);
                        Vector3 rotatedSamPoint = Vector3.Transform(samplePoint, xAxisRotationQ * zAxisRotationQ);
                        Vector3 rotatedNormal = Vector3.Transform(normal, xAxisRotationQ * zAxisRotationQ);
                        Vector3 screenPoint = canvas.CalculateScreenPoint(rotatedSamPoint);

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

                xAxisRotationRadians += 0.002f;
                zAxisRotationRadians += 0.005f;
            }
        }

    }
}

