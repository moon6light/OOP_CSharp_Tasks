
using System;

class StudentGroup
{
    public int[] Grades { get; set; }

    public StudentGroup(int[] grades)
    {
        Grades = grades;
    }

    public double GetAverage()
    {
        int sum = 0;

        foreach (int grade in Grades)
        {
            sum += grade;
        }

        return (double)sum / Grades.Length;
    }

    public int GetMin()
    {
        int min = Grades[0];

        foreach (int grade in Grades)
        {
            if (grade < min)
                min = grade;
        }

        return min;
    }

    public int GetMax()
    {
        int max = Grades[0];

        foreach (int grade in Grades)
        {
            if (grade > max)
                max = grade;
        }

        return max;
    }
}

class Program
{
    static void Main()
    {
        int[][] grades =
        {
            new int[] { 85, 90, 75, 88, 92, 70, 95, 80, 87, 91 },
            new int[] { 65, 78, 82, 90, 74, 88, 93, 69, 85, 80, 77, 91 },
            new int[] { 95, 92, 88, 100, 85, 90, 97, 93, 89, 96 },
            new int[] { 70, 75, 80, 85, 90, 65, 78, 82, 88, 92, 95 }
        };

        int totalSum = 0;
        int totalCount = 0;
        int totalMin = int.MaxValue;
        int totalMax = int.MinValue;


        for (int i = 0; i < grades.Length; i++)
        {
            StudentGroup group = new StudentGroup(grades[i]);

            Console.WriteLine($"Група {i + 1}:");
            Console.WriteLine($"Кількість студентів {grades[i].Length}");
            Console.WriteLine($"Сер бал {group.GetAverage():F2}");
            Console.WriteLine($"Мін оцінка {group.GetMin()}");
            Console.WriteLine($"Макс оцінка {group.GetMax()}\n");

            foreach (int grade in grades[i])
            {
                totalSum += grade;
                totalCount++;

                if (grade < totalMin)
                    totalMin = grade;

                if (grade > totalMax)
                    totalMax = grade;
            }
        }

        double totalAverage = (double)totalSum / totalCount;

        Console.WriteLine("результати всього потоку");
        Console.WriteLine($"кількість студентів {totalCount}");
        Console.WriteLine($"Сер бал {totalAverage:F2}");
        Console.WriteLine($"мін оцінка {totalMin}");
        Console.WriteLine($"макс оцінка {totalMax}");
    }
}

