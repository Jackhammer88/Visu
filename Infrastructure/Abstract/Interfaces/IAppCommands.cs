using Prism.Commands;

namespace Infrastructure.Abstract.Interfaces
{
    public interface IAppCommands
    {
        CompositeCommand OpenApp { get; }
        CompositeCommand RefreshApp { get; }
        CompositeCommand CloseApp { get; }
    }
}