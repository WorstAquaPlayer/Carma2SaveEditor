using System.Collections.Generic;
using Yarhl.IO;

namespace Carma2SaveEditor.Models
{
    public class Opponents
    {
        public List<Opponent> OpponentList = new List<Opponent>();

        public int OpponentCount { get; set; }

        public Opponents(string filePath)
        {
            using var stream = DataStreamFactory.FromFile(filePath, FileOpenMode.Read);
            var reader = new TextDataReader(stream);

            var opponentCount = int.Parse(ReadLine(reader).Split()[0]);

            for (int i = 0; i < opponentCount; i++)
            {
                var opponent = new Opponent();

                ReadLine(reader); // Driver name
                ReadLine(reader); // Driver short name
                opponent.CarName = ReadLine(reader).Replace('\t', ' ').TrimEnd();
                ReadLine(reader); // Strength rating
                ReadLine(reader); // Cost to buy it
                ReadLine(reader); // Network availability
                opponent.Txt = ReadLine(reader).Split()[0];
                ReadLine(reader); // Next 4 lines: vehicle description
                ReadLine(reader);
                ReadLine(reader);
                ReadLine(reader);

                OpponentList.Add(opponent);
            }
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
