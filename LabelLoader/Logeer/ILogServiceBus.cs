using System.Threading.Tasks;

namespace LabelLoader.Logger
{
    public interface ILogServiceBus
    {
        Task SendMessagesAsync(string message);
    }
}
