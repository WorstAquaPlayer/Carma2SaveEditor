using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using Carma2SaveEditor.Models;
using System.Linq;
using System.Diagnostics;
using Yarhl.IO;

namespace Carma2SaveEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        string GamePath;

        string SavePath;
        Savedgames Savedgames;

        Races Races;
        Opponents Opponents;

        bool canModifySave = false;
        public bool CanModifySave
        {
            get => canModifySave;
            set => this.RaiseAndSetIfChanged(ref canModifySave, value);
        }

        bool canModifySlot = false;
        public bool CanModifySlot
        {
            get => canModifySlot;
            set => this.RaiseAndSetIfChanged(ref canModifySlot, value);
        }

        public ReactiveCommand<Unit, Unit> OpenFolder { get; }
        public Interaction<Unit, string?> ShowOpenFolderDialog { get; }

        List<string> saveList;
        public List<string> SaveList
        {
            get => saveList;
            set => this.RaiseAndSetIfChanged(ref saveList, value);
        }

        int saveSelectedIndex;
        public int SaveSelectedIndex
        {
            get => saveSelectedIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref saveSelectedIndex, value);
                SaveListChangeSelection();
            }
        }

        string playerNameText;
        public string PlayerNameText
        {
            get => playerNameText;
            set
            {
                this.RaiseAndSetIfChanged(ref playerNameText, value);

                if (CanModifySlot)
                {
                    var orgSelectIndex = SaveSelectedIndex;

                    Savedgames.Slots[SaveSelectedIndex].PlayerName = value;

                    var list = new List<string>(SaveList);
                    list[SaveSelectedIndex] = $"Slot {SaveSelectedIndex + 1:D2}: {value}";
                    SaveList = list;

                    SaveSelectedIndex = orgSelectIndex;
                }
            }
        }

        List<string> unlockedCarsList;
        public List<string> UnlockedCarsList
        {
            get => unlockedCarsList;
            set => this.RaiseAndSetIfChanged(ref unlockedCarsList, value);
        }

        public int maxCarValue = SaveSlot.AvailableCarsLimit;
        public int MaxCarValue
        {
            get => maxCarValue;
            set => this.RaiseAndSetIfChanged(ref maxCarValue, value);
        }

        int availableCarsValue;
        public int AvailableCarsValue
        {
            get => availableCarsValue;
            set
            {
                this.RaiseAndSetIfChanged(ref availableCarsValue, value);

                if (CanModifySlot)
                {
                    var list = new List<string>();

                    for (int i = 0; i < value; i++)
                    {
                        var saveSlot = Savedgames.Slots[SaveSelectedIndex];
                        var index = saveSlot.AvailableCarIndex[i];

                        list.Add(CarList[index]);
                    }

                    UnlockedCarsList = list;

                    Savedgames.Slots[SaveSelectedIndex].AvailableCarsToSelect = value;
                }
            }
        }

        int carSelectedIndex;
        public int CarSelectedIndex
        {
            get => carSelectedIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref carSelectedIndex, value);
                CarListChangeSelection();
            }
        }

        int carListSelectedIndex;
        public int CarListSelectedIndex
        {
            get => carListSelectedIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref carListSelectedIndex, value);

                if (CanModifyCar)
                {
                    var orgSelectIndex = CarSelectedIndex;

                    Savedgames.Slots[SaveSelectedIndex].AvailableCarIndex[CarSelectedIndex] = value;

                    var list = new List<string>(UnlockedCarsList);
                    list[CarSelectedIndex] = CarList[value];
                    UnlockedCarsList = list;

                    CarSelectedIndex = orgSelectIndex;
                }
            }
        }

        bool canModifyCar = false;
        public bool CanModifyCar
        {
            get => canModifyCar;
            set => this.RaiseAndSetIfChanged(ref canModifyCar, value);
        }

        string saveDateText;
        public string SaveDateText
        {
            get => saveDateText;
            set
            {
                this.RaiseAndSetIfChanged(ref saveDateText, value);

                if (CanModifySlot)
                {
                    Savedgames.Slots[SaveSelectedIndex].SaveDate = value;
                }
            }
        }

        string saveHourText;
        public string SaveHourText
        {
            get => saveHourText;
            set
            {
                this.RaiseAndSetIfChanged(ref saveHourText, value);

                if (CanModifySlot)
                {
                    Savedgames.Slots[SaveSelectedIndex].SaveHour = value;
                }
            }
        }

        int creditsValue;
        public int CreditsValue
        {
            get => creditsValue;
            set
            {
                this.RaiseAndSetIfChanged(ref creditsValue, value);

                if (CanModifySlot)
                {
                    Savedgames.Slots[SaveSelectedIndex].Credits = value;
                }
            }
        }

        int armorValue;
        public int ArmorValue
        {
            get => armorValue;
            set
            {
                this.RaiseAndSetIfChanged(ref armorValue, value);

                if (CanModifySlot)
                {
                    Savedgames.Slots[SaveSelectedIndex].CurrentArmor = value;
                }
            }
        }

        int powerValue;
        public int PowerValue
        {
            get => powerValue;
            set
            {
                this.RaiseAndSetIfChanged(ref powerValue, value);

                if (CanModifySlot)
                {
                    Savedgames.Slots[SaveSelectedIndex].CurrentPower = value;
                }
            }
        }

        int offensiveValue;
        public int OffensiveValue
        {
            get => offensiveValue;
            set
            {
                this.RaiseAndSetIfChanged(ref offensiveValue, value);

                if (CanModifySlot)
                {
                    Savedgames.Slots[SaveSelectedIndex].CurrentOffensive = value;
                }
            }
        }

        int maxArmorValue;
        public int MaxArmorValue
        {
            get => maxArmorValue;
            set
            {
                this.RaiseAndSetIfChanged(ref maxArmorValue, value);

                if (CanModifySlot)
                {
                    Savedgames.Slots[SaveSelectedIndex].CurrentMaxArmor = value;
                }
            }
        }

        int maxPowerValue;
        public int MaxPowerValue
        {
            get => maxPowerValue;
            set
            {
                this.RaiseAndSetIfChanged(ref maxPowerValue, value);

                if (CanModifySlot)
                {
                    Savedgames.Slots[SaveSelectedIndex].CurrentMaxPower = value;
                }
            }
        }

        int maxOffensiveValue;
        public int MaxOffensiveValue
        {
            get => maxOffensiveValue;
            set
            {
                this.RaiseAndSetIfChanged(ref maxOffensiveValue, value);

                if (CanModifySlot)
                {
                    Savedgames.Slots[SaveSelectedIndex].CurrentMaxOffensive = value;
                }
            }
        }

        int difficultySelectedIndex;
        public int DifficultySelectedIndex
        {
            get => difficultySelectedIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref difficultySelectedIndex, value);

                if (CanModifySlot)
                {
                    Savedgames.Slots[SaveSelectedIndex].Difficulty = value;
                }
            }
        }

        bool missionEnabledIsChecked;
        public bool MissionEnabledIsChecked
        {
            get => missionEnabledIsChecked;
            set
            {
                this.RaiseAndSetIfChanged(ref missionEnabledIsChecked, value);

                if (CanModifySlot)
                {
                    Savedgames.Slots[SaveSelectedIndex].MissionEnabled = value;
                }
            }
        }

        List<string> raceList;
        public List<string> RaceList
        {
            get => raceList;
            set => this.RaiseAndSetIfChanged(ref raceList, value);
        }

        int currentRaceSelectedIndex;
        public int CurrentRaceSelectedIndex
        {
            get => currentRaceSelectedIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref currentRaceSelectedIndex, value);

                if (CanModifySlot)
                {
                    Savedgames.Slots[SaveSelectedIndex].CurrentRace = value;
                }
            }
        }

        bool gameCompletedIsChecked;
        public bool GameCompletedIsChecked
        {
            get => gameCompletedIsChecked;
            set
            {
                this.RaiseAndSetIfChanged(ref gameCompletedIsChecked, value);

                if (CanModifySlot)
                {
                    Savedgames.Slots[SaveSelectedIndex].GameCompleted = value;
                }
            }
        }

        List<string> carList;
        public List<string> CarList
        {
            get => carList;
            set => this.RaiseAndSetIfChanged(ref carList, value);
        }

        int currentCarSelectedIndex;
        public int CurrentCarSelectedIndex
        {
            get => currentCarSelectedIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref currentCarSelectedIndex, value);

                if (CanModifySlot)
                {
                    Savedgames.Slots[SaveSelectedIndex].CurrentCar = value;

                    var txt = Opponents.OpponentList[CurrentCarSelectedIndex].Txt;
                    Savedgames.Slots[SaveSelectedIndex].CarTxtName = txt;
                }
            }
        }

        int raceSelectedIndex;
        public int RaceSelectedIndex
        {
            get => raceSelectedIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref raceSelectedIndex, value);

                if (value >= 0)
                {
                    CanModifyRace = false;

                    NumberOfRaceCompletions = Savedgames.Slots[SaveSelectedIndex].RaceCompleted[value];

                    CanModifyRace = true;
                }
                else
                {
                    CanModifyRace = false;
                }
            }
        }

        bool canModifyRace = false;
        public bool CanModifyRace
        {
            get => canModifyRace;
            set => this.RaiseAndSetIfChanged(ref canModifyRace, value);
        }

        int numberOfRaceCompletions;
        public int NumberOfRaceCompletions
        {
            get => numberOfRaceCompletions;
            set
            {
                this.RaiseAndSetIfChanged(ref numberOfRaceCompletions, value);

                if (CanModifyRace)
                {
                    Savedgames.Slots[SaveSelectedIndex].RaceCompleted[RaceSelectedIndex] = value;
                }
            }
        }

        List<string> compRaceList;
        public List<string> CompRaceList
        {
            get => compRaceList;
            set => this.RaiseAndSetIfChanged(ref compRaceList, value);
        }

        public ReactiveCommand<Unit, Unit> BackupSaveCommand { get; }

        public ReactiveCommand<Unit, Unit> RefreshAllCommand { get; }

        public ReactiveCommand<Unit, Unit> MaximizeApoCommand { get; }

        public ReactiveCommand<Unit, Unit> SaveCommand { get; }

        public Interaction<Unit, string?> ShowSaveArsFileDialog { get; }

        public ReactiveCommand<Unit, Unit> StartGameCommand { get; }

        public ReactiveCommand<Unit, Unit> ExportSlot { get; }

        public Interaction<Unit, string?> ShowSaveBinFileDialog { get; }

        public ReactiveCommand<Unit, Unit> ImportSlot { get; }

        public Interaction<Unit, string?> ShowOpenFileDialog { get; }

        public ReactiveCommand<Unit, Unit> RemoveSlotCommand { get; }

        public ReactiveCommand<Unit, Unit> AddSlotCommand { get; }

        public event EventHandler OnExitButtonPressed;

        public MainWindowViewModel()
        {
            OpenFolder = ReactiveCommand.CreateFromTask(OpenFolderAsync);
            ShowOpenFolderDialog = new Interaction<Unit, string?>();

            BackupSaveCommand = ReactiveCommand.Create(BackupSave);
            RefreshAllCommand = ReactiveCommand.Create(RefreshAll);
            MaximizeApoCommand = ReactiveCommand.Create(MaximizeApo);

            SaveCommand = ReactiveCommand.CreateFromTask(SaveAsync);
            ShowSaveArsFileDialog = new Interaction<Unit, string?>();

            StartGameCommand = ReactiveCommand.Create(StartGame);

            ExportSlot = ReactiveCommand.CreateFromTask(ExportSlotAsync);
            ShowSaveBinFileDialog = new Interaction<Unit, string?>();

            ImportSlot = ReactiveCommand.CreateFromTask(ImportSlotAsync);
            ShowOpenFileDialog = new Interaction<Unit, string?>();

            RemoveSlotCommand = ReactiveCommand.Create(RemoveSlot);
            AddSlotCommand = ReactiveCommand.Create(AddSlot);
        }

        void OpenGameFolder(string path)
        {
            CanModifySave = false;

            var racesPath = Path.Combine(path, "data", "RACES.TXT");
            Races = new Races(racesPath);
            CurrentRaceSelectedIndex = -1;
            RaceSelectedIndex = -1;
            RaceList = Races.RaceNames.Select((name, index) => $"{index:D2} - {name}").ToList();

            SavePath = Path.Combine(path, "data", "SAVEDGAMES.ARS");
            Savedgames = new Savedgames(SavePath, Races.RaceCount);
            SaveSelectedIndex = -1;
            SaveList = Savedgames.Slots.Select((slot, index) => $"Slot {index + 1:D2}: {slot.PlayerName}").ToList();

            var opponentsPath = Path.Combine(path, "data", "OPPONENT.TXT");
            Opponents = new Opponents(opponentsPath);
            CarSelectedIndex = -1;
            CarListSelectedIndex = -1;
            CurrentCarSelectedIndex = -1;
            CarList = Opponents.OpponentList.Select((opponent, index) => $"{index:D2} - {opponent.CarName}").ToList();

            CanModifySave = true;
        }

        async Task OpenFolderAsync()
        {
            var gamePath = await ShowOpenFolderDialog.Handle(Unit.Default);

            if (gamePath is object)
            {
                var gameFiles = new string[]
                {
                    "carma2.exe",
                    $"data{Path.DirectorySeparatorChar}RACES.TXT",
                    $"data{Path.DirectorySeparatorChar}SAVEDGAMES.ARS",
                    $"data{Path.DirectorySeparatorChar}OPPONENT.TXT"
                };

                if (gameFiles.Any(file => !File.Exists(Path.Combine(gamePath, file))))
                {
                    var missingFiles = new StringBuilder();

                    foreach (var file in gameFiles)
                    {
                        var combinedPath = Path.Combine(gamePath, file);

                        if (!File.Exists(combinedPath))
                        {
                            missingFiles.AppendLine(combinedPath);
                        }
                    }

                    var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow("Missing game files",
                    $"The following files are missing:{Environment.NewLine}{missingFiles}");

                    messageBoxStandardWindow.Show();
                }
                else
                {
                    OpenGameFolder(gamePath);
                    GamePath = gamePath;

                    SaveSelectedIndex = 0;
                }
            }
        }

        void BackupSave()
        {
            var backupPath = $"{SavePath}.{DateTime.Now.ToString("yyyyMMdd-HHmmss")}";

            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow("Information",
                $"The savegame has been backuped to{Environment.NewLine}{backupPath}");

            File.Copy(SavePath, backupPath);

            messageBoxStandardWindow.Show();
        }

        void RefreshAll()
        {
            CanModifySlot = false;
            CanModifyCar = false;

            OpenGameFolder(GamePath);
            SaveSelectedIndex = 0;
        }

        void MaximizeApo()
        {
            ArmorValue = 30;
            MaxArmorValue = 30;
            PowerValue = 30;

            MaxPowerValue = 30;
            OffensiveValue = 30;
            MaxOffensiveValue = 30;
        }

        async Task SaveAsync()
        {
            var savePath = await ShowSaveArsFileDialog.Handle(Unit.Default);

            if (savePath is object)
            {
                using var binary = Savedgames.CreateBinary();

                if (File.Exists(savePath))
                {
                    File.Delete(savePath);
                }

                binary.WriteTo(savePath);
            }
        }

        void StartGame()
        {
            Process.Start(Path.Combine(GamePath, "carma2.exe"));
        }

        async Task ExportSlotAsync()
        {
            var slotPath = await ShowSaveBinFileDialog.Handle(Unit.Default);

            if (slotPath is object)
            {
                var slot = Savedgames.Slots[SaveSelectedIndex];
                using var binary = slot.CreateBinary();

                binary.WriteTo(slotPath);
            }
        }

        async Task ImportSlotAsync()
        {
            var slotPath = await ShowOpenFileDialog.Handle(Unit.Default);

            if (slotPath is object)
            {
                using var stream = DataStreamFactory.FromFile(slotPath, FileOpenMode.Read);

                if (stream.Length % SaveSlot.SaveSlotSize != 0)
                {
                    throw new FormatException("Only vanilla Carmageddon II saves are supported.");
                }

                var slot = new SaveSlot(stream, Races.RaceCount);
                Savedgames.Slots[SaveSelectedIndex] = slot;

                SaveSelectedIndex = SaveSelectedIndex;
            }
        }

        void RemoveSlot()
        {
            var index = SaveSelectedIndex;

            SaveSelectedIndex = -1;
            Savedgames.Slots.RemoveAt(index);
            SaveList = Savedgames.Slots.Select((slot, index) => $"Slot {index + 1:D2}: {slot.PlayerName}").ToList();

            if (Savedgames.Slots.Count == 0)
            {
                SaveSelectedIndex = -1;
            }
            else
            {
                if (Savedgames.Slots.Count == index)
                {
                    SaveSelectedIndex = index - 1;
                }
                else
                {
                    SaveSelectedIndex = index;
                }
            }
        }

        void AddSlot()
        {
            var index = SaveSelectedIndex;

            SaveSelectedIndex = -1;
            Savedgames.Slots.Insert(index + 1, new SaveSlot(Races.RaceCount));
            SaveList = Savedgames.Slots.Select((slot, index) => $"Slot {index + 1:D2}: {slot.PlayerName}").ToList();

            SaveSelectedIndex = index;
        }

        void SaveListChangeSelection()
        {
            if (SaveSelectedIndex >= 0)
            {
                CanModifySlot = false;

                var slot = Savedgames.Slots[SaveSelectedIndex];
                var newList = new List<string>();

                for (int i = 0; i < slot.AvailableCarsToSelect; i++)
                {
                    newList.Add(CarList[slot.AvailableCarIndex[i]]);
                }

                UnlockedCarsList = newList;

                PlayerNameText = slot.PlayerName;
                AvailableCarsValue = slot.AvailableCarsToSelect;

                CarSelectedIndex = -1;
                CarListSelectedIndex = -1;
                RaceSelectedIndex = -1;

                SaveDateText = slot.SaveDate;
                SaveHourText = slot.SaveHour;

                CreditsValue = slot.Credits;

                ArmorValue = slot.CurrentArmor;
                MaxArmorValue = slot.CurrentMaxArmor;
                PowerValue = slot.CurrentPower;

                MaxPowerValue = slot.CurrentMaxPower;
                OffensiveValue = slot.CurrentOffensive;
                MaxOffensiveValue = slot.CurrentMaxOffensive;

                DifficultySelectedIndex = slot.Difficulty;

                MissionEnabledIsChecked = slot.MissionEnabled;
                CurrentRaceSelectedIndex = slot.CurrentRace;

                GameCompletedIsChecked = slot.GameCompleted;
                CurrentCarSelectedIndex = slot.CurrentCar;

                CompRaceList = new List<string>(RaceList);

                CanModifySlot = true;
            }
            else
            {
                CanModifySlot = false;

                UnlockedCarsList = new List<string>();

                PlayerNameText = string.Empty;
                AvailableCarsValue = 0;

                CarSelectedIndex = -1;
                CarListSelectedIndex = -1;
                RaceSelectedIndex = -1;

                SaveDateText = string.Empty;
                SaveHourText = string.Empty;

                CreditsValue = 0;

                ArmorValue = 0;
                MaxArmorValue = 0;
                PowerValue = 0;

                MaxPowerValue = 0;
                OffensiveValue = 0;
                MaxOffensiveValue = 0;

                DifficultySelectedIndex = -1;

                MissionEnabledIsChecked = false;
                CurrentRaceSelectedIndex = -1;

                GameCompletedIsChecked = false;
                CurrentCarSelectedIndex = -1;

                CompRaceList = new List<string>();
            }
        }

        void CarListChangeSelection()
        {
            if (CarSelectedIndex >= 0)
            {
                CanModifyCar = false;

                var saveSlot = Savedgames.Slots[SaveSelectedIndex];
                var index = saveSlot.AvailableCarIndex[CarSelectedIndex];

                CarListSelectedIndex = index;
                CanModifyCar = true;
            }
            else
            {
                CanModifyCar = false;
            }
        }

        void ExitButton()
        {
            OnExitButtonPressed(this, new EventArgs());
        }
    }
}
