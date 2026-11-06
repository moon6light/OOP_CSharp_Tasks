
using System;

class Triangle
{
    public double A { get; set; }
    public double B { get; set; }
    public double C { get; set; }

    public Triangle(double a, double b, double c)
    {
        A = a;
        B = b;
        C = c;
    }

    public bool IsPositive()
    {
        return A > 0 && B > 0 && C > 0;
    }

    public bool IsValid()
    {
        return IsPositive() &&
               A + B > C &&
               A + C > B &&
               B + C > A;
    }

    public double GetPerimeter()
    {
        return A + B + C;
    }

    public double GetArea()
    {
        double p = GetPerimeter() / 2;
        return Math.Sqrt(p * (p - A) *
                         (p - B) * (p - C));
    }

    public string GetTriangleType()
    {
        double[] sides = { A, B, C };
        Array.Sort(sides);

        const double eps = 1e-9;

        if (Math.Abs(A - B) < eps &&
            Math.Abs(B - C) < eps)
            return "рівносторонній";

        if (Math.Abs(sides[0] * sides[0] +
                     sides[1] * sides[1] -
                     sides[2] * sides[2]) < eps)
            return "прямокутний";

        if (Math.Abs(A - B) < eps ||
            Math.Abs(A - C) < eps ||
            Math.Abs(B - C) < eps)
            return "рівнобедрений";

        return "довільний";
    }
}

class Program
{
    static void Main()
    {
        try
        {
            Console.Write("введіть сторону a");
            double a = double.Parse(Console.ReadLine()!);

            Console.Write("введіть сторону b");
            double b = double.Parse(Console.ReadLine()!);

            Console.Write("введіть сторону c");
            double c = double.Parse(Console.ReadLine()!);

            Triangle triangle = new Triangle(a, b, c);

            if (!triangle.IsPositive())
            {
                Console.WriteLine(
                    "сторони повинні бути додатними");
                return;
            }

            if (!triangle.IsValid())
            {
                Console.WriteLine(
                    "Не допустимо");
                return;
            }

            Console.WriteLine("\nрезультати");
            Console.WriteLine("периметр" +
                              triangle.GetPerimeter());
            Console.WriteLine("площа" +
                              triangle.GetArea().ToString("F2"));
            Console.WriteLine("вид трикутника" +
                              triangle.GetTriangleType());
        }
        catch (FormatException)
        {
            Console.WriteLine(
                "помилка");
        }
        catch (OverflowException)
        {
            Console.WriteLine(
                "помилка");
        }
    }
}
