using Infrastructure.Abstract.Interfaces;
using Prism.Commands;

namespace Infrastructure
{
    public class AppCommands : IAppCommands
    {
        private CompositeCommand _openApp;
        private CompositeCommand _refreshApp;
        private CompositeCommand _closeApp;

        public CompositeCommand OpenApp => _openApp ??= new CompositeCommand();
        public CompositeCommand RefreshApp => _refreshApp ??= new CompositeCommand();
        public CompositeCommand CloseApp => _closeApp ??= new CompositeCommand();
    }
}