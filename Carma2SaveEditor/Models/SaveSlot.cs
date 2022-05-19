using System;
using System.Globalization;
using Yarhl.IO;

namespace Carma2SaveEditor.Models
{
    public class SaveSlot
    {
        public static int SaveSlotSize = 0x328; // vanilla value, can be modified if race count is modified?
        public static int AvailableCarsLimit = 0x3C; // vanilla hardcoded value

        public string PlayerName { get; set; }
        public string CarTxtName { get; set; }
        public string SaveDate { get; set; }
        public string SaveHour { get; set; }
        public int[] RaceCompleted { get; set; }
        public int Credits { get; set; }
        public int Difficulty { get; set; }
        public bool GameCompleted { get; set; }
        public int AvailableCarsToSelect { get; set; }
        public int[] AvailableCarIndex { get; set; }
        public int CurrentCar { get; set; }
        public int CurrentRace { get; set; }
        public bool MissionEnabled { get; set; }
        public int CurrentArmor { get; set; }
        public int CurrentPower { get; set; }
        public int CurrentOffensive { get; set; }
        public int CurrentMaxArmor { get; set; }
        public int CurrentMaxPower { get; set; }
        public int CurrentMaxOffensive { get; set; }

        public SaveSlot(int raceCount)
        {
            PlayerName = "MAX";
            CarTxtName = "EAGLE3.TXT";

            var date = DateTime.Now;

            SaveDate = date.ToString("dd/MM/yyyy");
            SaveHour = $"{date.ToString("hh:mm:ss t", CultureInfo.InvariantCulture).ToLower()}. m.";

            RaceCompleted = new int[raceCount];
            Credits = 10000;
            Difficulty = 0;

            GameCompleted = false;
            AvailableCarsToSelect = 1;
            AvailableCarIndex = new int[AvailableCarsLimit];

            CurrentCar = 0;
            CurrentRace = 0;
            MissionEnabled = false;

            CurrentArmor = 1;
            CurrentPower = 1;
            CurrentOffensive = 1;

            CurrentMaxArmor = 1;
            CurrentMaxPower = 1;
            CurrentMaxOffensive = 1;
        }

        public SaveSlot(DataStream dataStream, int raceCount)
        {
            var reader = new DataReader(dataStream);

            var checksum = reader.ReadInt32();

            if (checksum != 0x12345678)
            {
                throw new FormatException($"Corrupted or invalid Carmageddon II saveslot.");
            }

            PlayerName = reader.ReadString();

            reader.Stream.Position = 0x12;
            CarTxtName = reader.ReadString();

            reader.Stream.Position = 0x32;
            SaveDate = reader.ReadString();

            reader.Stream.Position = 0x52;
            SaveHour = reader.ReadString();

            reader.Stream.Position = 0x74;
            RaceCompleted = new int[raceCount];
            for (int j = 0; j < raceCount; j++)
            {
                RaceCompleted[j] = reader.ReadInt32();
            }

            reader.Stream.Position = 0x204;
            Credits = reader.ReadInt32();
            Difficulty = reader.ReadInt32();
            GameCompleted = reader.ReadInt32() > 0;
            AvailableCarsToSelect = reader.ReadInt32();

            AvailableCarIndex = new int[AvailableCarsLimit];
            for (int j = 0; j < AvailableCarsLimit; j++)
            {
                AvailableCarIndex[j] = reader.ReadInt32();
            }

            CurrentCar = reader.ReadInt32();
            CurrentRace = reader.ReadInt32();
            MissionEnabled = reader.ReadInt32() > 0;

            CurrentArmor = reader.ReadInt32();
            CurrentPower = reader.ReadInt32();
            CurrentOffensive = reader.ReadInt32();

            CurrentMaxArmor = reader.ReadInt32();
            CurrentMaxPower = reader.ReadInt32();
            CurrentMaxOffensive = reader.ReadInt32();
        }

        public DataStream CreateBinary()
        {
            var dataStream = new DataStream();
            var writer = new DataWriter(dataStream);

            writer.Write(0x12345678); // Header
            writer.Write(PlayerName);

            WriteUntilPosition(writer, 0x12);
            writer.Write(CarTxtName);

            WriteUntilPosition(writer, 0x32);
            writer.Write(SaveDate);

            WriteUntilPosition(writer, 0x52);
            writer.Write(SaveHour);

            WriteUntilPosition(writer, 0x74);
            for (int i = 0; i < RaceCompleted.Length; i++)
            {
                writer.Write(RaceCompleted[i]);
            }

            WriteUntilPosition(writer, 0x204);
            writer.Write(Credits);
            writer.Write(Difficulty);
            writer.Write(GameCompleted ? 1 : 0);
            writer.Write(AvailableCarsToSelect);
            for (int i = 0; i < AvailableCarIndex.Length; i++)
            {
                writer.Write(AvailableCarIndex[i]);
            }
            writer.Write(CurrentCar);
            writer.Write(CurrentRace);
            writer.Write(MissionEnabled ? 1 : 0);
            writer.Write(CurrentArmor);
            writer.Write(CurrentPower);
            writer.Write(CurrentOffensive);
            writer.Write(CurrentMaxArmor);
            writer.Write(CurrentMaxPower);
            writer.Write(CurrentMaxOffensive);

            return dataStream;
        }

        void WriteUntilPosition(DataWriter writer, int position)
        {
            while (writer.Stream.Position != position)
            {
                writer.Write((byte)0x0);
            }
        }
    }
}
