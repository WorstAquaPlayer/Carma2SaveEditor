using System.Collections.Generic;
using Yarhl.IO;

namespace Carma2SaveEditor.Models
{
    public class Races
    {
        public List<string> RaceNames = new List<string>();

        public int RaceCount
        {
            get { return RaceNames.Count; }
        }

        public Races(string filePath)
        {
            using var stream = DataStreamFactory.FromFile(filePath, FileOpenMode.Read);
            var reader = new TextDataReader(stream);

            ReadLine(reader); // skip first line, since it's only debug info

            ReadLine(reader); // skip next 6 lines since it's opponent info, not necessary
            ReadLine(reader);
            ReadLine(reader);
            ReadLine(reader);
            ReadLine(reader);
            ReadLine(reader);

            while (reader.Stream.Position < reader.Stream.Length)
            {
                var line = ReadLine(reader);

                if (line == "END")
                {
                    break;
                }

                ReadLine(reader); // Text file name
                ReadLine(reader); // Name of interface element
                ReadLine(reader); // Number of opponents
                var explicitOpponents = ReadLineAndGetInt(reader); // Number of explicit opponents
                for (int i = 0; i < explicitOpponents; i++)
                {
                    ReadLine(reader);
                }

                ReadLine(reader); // Opponent nastiness level
                ReadLine(reader); // Powerup exclusions
                ReadLine(reader); // Disable time awards
                ReadLine(reader); // Boundary race (mission)
                var raceType = ReadLineAndGetInt(reader); // Race type
                ReadLine(reader); // Initial timer count for each skill level

                switch (raceType)
                {
                    case 0:
                        ReadLine(reader); // # laps
                        ReadLine(reader); // Race completed bonus (all laps raced) for each skill level
                        ReadLine(reader); // Race completed bonus (all peds killed) for each skill level
                        ReadLine(reader); // Race completed bonus (all oppos wasted) for each skill level
                        break;
                    case 1:
                        var opponentsToKill = ReadLineAndGetInt(reader); // Number of opponents that must be killed
                        if (opponentsToKill >= 0)
                        {
                            for (int i = 0; i < opponentsToKill; i++)
                            {
                                ReadLine(reader);
                            }
                        }
                        ReadLine(reader); // Race completed bonus for each skill level
                        break;
                    case 2:
                        var pedGroups = ReadLineAndGetInt(reader); // Number of ped groups
                        if (pedGroups >= 0)
                        {
                            for (int i = 0; i < pedGroups; i++)
                            {
                                ReadLine(reader);
                            }
                        }
                        ReadLine(reader); // Race completed bonus for each skill level
                        break;
                    case 3:
                        ReadLine(reader); // # laps
                        ReadLine(reader); // Race completed bonus for each skill level
                        break;
                    case 4:
                        ReadLine(reader); // Smash variable number
                        ReadLine(reader); // Smash variable target
                        ReadLine(reader); // Race completed bonus for each skill level
                        break;
                    case 5:
                        ReadLine(reader); // Smash variable number
                        ReadLine(reader); // Smash variable target
                        ReadLine(reader); // Ped group index for required extra kills
                        ReadLine(reader); // Race completed bonus for each skill level
                        break;
                }

                ReadLine(reader); // Race description
                ReadLine(reader); // Expansion

                RaceNames.Add(line);
            }
        }

        int ReadLineAndGetInt(TextDataReader reader)
        {
            var line = ReadLine(reader);

            return int.Parse(line.Split()[0]);
        }

        string ReadLine(TextDataReader reader)
        {
            var line = reader.ReadLine();

            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//"))
            {
                line = ReadLine(reader);
            }

            return line;
        }
    }
}
