namespace Ai_ShopBot.Application.Interfaces
{
    public interface IAiChatAssistantServices
    {
        IAsyncEnumerable<string> AIChattingAsync(string prompt, CancellationToken cancellationToken);
        Task ClearChatHistoryAsync();
    }
}
