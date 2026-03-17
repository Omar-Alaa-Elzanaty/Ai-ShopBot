using System.Runtime.CompilerServices;

namespace Ai_ShopBot.Croe.Interfaces
{
    public interface IAiChatAssistantServices
    {
        IAsyncEnumerable<string> AIChattingAsync(string prompt, CancellationToken cancellationToken);
        Task ClearChatHistoryAsync();
    }
}
