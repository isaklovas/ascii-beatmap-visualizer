using System;
using System.IO;


namespace BeatmapParser
{
    class Parser
    {
        public struct HitObject
        {
            public int X;
            public int Y;
            public int Time;
            public int Type;
        }

        public struct Beatmap
        {
            public List<HitObject> HitObjects;
        }

        private static List<string> GetFileContent(string FilePath)
        {
            StreamReader reader = new StreamReader(FilePath);

            List<string> Lines = new List<string>();
            List<string> CleanedLines = new List<string>();

            string FileLine;

            while ((FileLine = reader.ReadLine()) != null)
            {
                Lines.Add(FileLine);
            }
            foreach (string Line in Lines)
            {
                if (Line == "" || Line == "\n" || Line.Substring(0, 2) == "//")
                    continue;
                CleanedLines.Add(Line);
            }
            return CleanedLines;
        }

        private static List<HitObject> ParseHitObjects(List<string> Lines)
        {
            List<HitObject> HitObjects = new List<HitObject>();

            int HitObjectsIndex = Lines.IndexOf("[HitObjects]");

            for (int i = 0; i < Lines.Count(); ++i)
            {
                if (i <= HitObjectsIndex)
                    continue;

                string[] Line = Lines[i].Split(",");
                HitObject hitObject;

                hitObject.X = int.Parse(Line[0]);
                hitObject.Y = int.Parse(Line[1]);
                hitObject.Time = int.Parse(Line[2]);
                hitObject.Type = int.Parse(Line[3]);

                HitObjects.Add(hitObject);
            }
            return HitObjects;
        }

        public static Beatmap Parse(string FilePath)
        {
            List<string> File = GetFileContent(FilePath);
            List<HitObject> HitObjects = ParseHitObjects(File);

            Beatmap beatmap;
            beatmap.HitObjects = HitObjects;

            return beatmap;
        }
    }
}
