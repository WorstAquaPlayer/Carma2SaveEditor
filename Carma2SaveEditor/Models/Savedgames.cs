using System;
using System.Collections.Generic;
using Yarhl.IO;

namespace Carma2SaveEditor.Models
{
    public class Savedgames
    {
        public List<SaveSlot> Slots;

        public Savedgames(string filePath, int raceCount)
        {
            using var stream = DataStreamFactory.FromFile(filePath, FileOpenMode.Read);

            if (stream.Length % SaveSlot.SaveSlotSize != 0)
            {
                throw new FormatException("Only vanilla Carmageddon II saves are supported.");
            }

            Slots = new List<SaveSlot>();
            var saveCount = stream.Length / SaveSlot.SaveSlotSize;

            for (int i = 0; i < saveCount; i++)
            {
                var subStream = new DataStream(stream, i * SaveSlot.SaveSlotSize, SaveSlot.SaveSlotSize, false);
                var slot = new SaveSlot(subStream, raceCount);

                Slots.Add(slot);
            }
        }

        public DataStream CreateBinary()
        {
            var dataStream = new DataStream();

            for (int i = 0; i < Slots.Count; i++)
            {
                var slotBinary = Slots[i].CreateBinary();
                slotBinary.WriteTo(dataStream);
            }

            return dataStream;
        }
    }
}
