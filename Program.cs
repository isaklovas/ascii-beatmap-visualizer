using System;
using System.Threading;
using System.Diagnostics;
using static BeatmapParser.Parser;


class Program
{
    private static string GetBackground()
    {
        string Text = "";
        int[] Resolution = {(int)(512 / 16) * 2, (int)(384 / 16)};

        for (int i = 0; i < Resolution[1]; ++i)
        {
            string Line = "";

            for (int j = 0; j < Resolution[0]; ++j)
            {
                Line += ".";
            }

            Line += "\n";
            Text += Line;
        }
        return Text;
    }

    private static int[] ScalePosition(HitObject hitObject)
    {
        int[] Resolution = new int[2];

        Resolution[0] = (int)(hitObject.X / 16) * 2;
        Resolution[1] = (int)(hitObject.Y / 16);

        return Resolution;
    }

    private static void NOP(double durationSeconds)
    {
        // i copied this from stackoverflow :sunglasses:
        double durationTicks = Math.Round(durationSeconds * Stopwatch.Frequency);
        Stopwatch sw = Stopwatch.StartNew();

        while (sw.ElapsedTicks < durationTicks)
        {

        }
    }

    private static void DrawHitObject(string Background, HitObject hitObject)
    {
        int[] Resolution = {(int)(512 / 16) * 2, (int)(384 / 16)};

        int[] HitObjectPosition = ScalePosition(hitObject);

        string[] BackgroundRows = Background.Split("\n");

        List<char> Row = new List<char>();

        foreach (char c in BackgroundRows[0])
        {
            Row.Add(c);
        }

        Row[HitObjectPosition[0]] = '#';

        string ConstructedRow = string.Join("", Row);

        BackgroundRows[HitObjectPosition[1]] = ConstructedRow;

        string ConstructedBackground = string.Join("\n", BackgroundRows);

        System.Console.SetCursorPosition(0, 0);
        System.Console.Write(ConstructedBackground);
    }

    static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            System.Console.WriteLine("Please provide an .osu file");
            return;
        }

        System.Console.Clear();

        Beatmap beatmap = Parse(args[0]);
        string Background = GetBackground();

        List<HitObject> hitObjects = beatmap.HitObjects;
        int HitObjectIndex = 0;

        for (int time = hitObjects[0].Time;; ++time)
        {
            if (time == hitObjects[HitObjectIndex].Time)
            {
                DrawHitObject(Background, hitObjects[HitObjectIndex]);
                ++HitObjectIndex;
            }

            if (hitObjects[^1].Time == time)
                break;

            NOP(0.000999);
        }
    }
}



