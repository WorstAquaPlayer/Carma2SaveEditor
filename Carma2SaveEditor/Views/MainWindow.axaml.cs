using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Carma2SaveEditor.ViewModels;
using ReactiveUI;

namespace Carma2SaveEditor.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();

            // When the window is activated, registers a handler for the ShowOpenFileDialog interaction.
            this.WhenActivated(d => {
                d(ViewModel.ShowOpenFolderDialog.RegisterHandler(ShowOpenFolderDialog));
                d(ViewModel.ShowSaveArsFileDialog.RegisterHandler(ShowSaveArsFileDialog));
                d(ViewModel.ShowSaveBinFileDialog.RegisterHandler(ShowSaveBinFileDialog));
                d(ViewModel.ShowOpenFileDialog.RegisterHandler(ShowOpenFileDialog));
            });
        }

        private async Task ShowOpenFolderDialog(InteractionContext<Unit, string?> interaction)
        {
            var dialog = new OpenFolderDialog();
            dialog.Title = "Select your Carmageddon II folder";

            var pathName = await dialog.ShowAsync(this);
            interaction.SetOutput(pathName);
        }

        private async Task ShowSaveArsFileDialog(InteractionContext<Unit, string?> interaction)
        {
            var dialog = new SaveFileDialog();
            dialog.Title = "Save new Carmageddon 2 save file as...";
            dialog.Filters.Add(new FileDialogFilter() { Name = "Carmageddon 2 Save File", Extensions = { "ARS" } });
            dialog.Filters.Add(new FileDialogFilter() { Name = "All Files", Extensions = { "*" } });

            var pathName = await dialog.ShowAsync(this);
            interaction.SetOutput(pathName);
        }

        private async Task ShowSaveBinFileDialog(InteractionContext<Unit, string?> interaction)
        {
            var dialog = new SaveFileDialog();
            dialog.Title = "Save slot as...";
            dialog.Filters.Add(new FileDialogFilter() { Name = "Binary File", Extensions = { "bin" } });
            dialog.Filters.Add(new FileDialogFilter() { Name = "All Files", Extensions = { "*" } });

            var pathName = await dialog.ShowAsync(this);
            interaction.SetOutput(pathName);
        }

        private async Task ShowOpenFileDialog(InteractionContext<Unit, string?> interaction)
        {
            var dialog = new OpenFileDialog();
            dialog.Title = "Import slot as...";
            dialog.Filters.Add(new FileDialogFilter() { Name = "Binary File", Extensions = { "bin" } });
            dialog.Filters.Add(new FileDialogFilter() { Name = "All Files", Extensions = { "*" } });
            dialog.AllowMultiple = false;

            var fileName = await dialog.ShowAsync(this);
            interaction.SetOutput(fileName.FirstOrDefault());
        }
    }
}
