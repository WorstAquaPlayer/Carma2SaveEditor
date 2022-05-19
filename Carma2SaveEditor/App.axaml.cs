using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Carma2SaveEditor.ViewModels;
using Carma2SaveEditor.Views;

namespace Carma2SaveEditor
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var vm = new MainWindowViewModel();
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = vm,
                };

                vm.OnExitButtonPressed += (s, e) => desktop.MainWindow.Close();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
