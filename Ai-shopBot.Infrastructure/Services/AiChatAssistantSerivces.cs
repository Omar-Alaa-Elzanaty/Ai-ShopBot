using Ai_ShopBot.Croe.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace Ai_shopBot.Infrastructure.Services
{
    public class AiChatAssistantSerivces : IAiChatAssistantServices
    {
        private readonly IChatClient _chatClient;
        private readonly Kernel _kernel;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _context;

        public AiChatAssistantSerivces(
            IChatClient chatClient,
            Kernel kernel,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor)
        {
            _chatClient = chatClient;
            _kernel = kernel;
            _unitOfWork = unitOfWork;
            _context = httpContextAccessor;
        }

        public async IAsyncEnumerable<string> AIChattingAsync(string prompt, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var InitialChatInfo = await File.ReadAllTextAsync("wwwroot/prompt.txt", cancellationToken);

            //TODO: replace with real user id
            var userId = 1;// _context?.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var last10Messages = await _unitOfWork.Redis.ListRangeAsync($"chat_history:{userId}", -10, -1);

            var chatHistory = new List<ChatMessage>
            {
                new ChatMessage(ChatRole.System,InitialChatInfo)
            };

            if (last10Messages != null && last10Messages.Length > 0)
            {
                foreach (var message in last10Messages)
                {
                    var messageStr = message.ToString();

                    var separtedIndex = messageStr.ToString().IndexOf(':');

                    var userMessage = messageStr[..separtedIndex];
                    var assistantMessage = messageStr[(separtedIndex + 1)..];

                     chatHistory.Add(new ChatMessage(ChatRole.User, userMessage));
                    chatHistory.Add(new ChatMessage(ChatRole.Assistant, assistantMessage));
                }
            }

            chatHistory.Add(new ChatMessage(ChatRole.User, prompt));

            var chatResponse = new StringBuilder();

            var tools = _kernel.Plugins
                .SelectMany(p => p.Select(f => f.AsKernelFunction()))
                .Cast<AITool>()
                .ToList();

            var options = new ChatOptions
            {
                Tools = tools,
                ToolMode = ChatToolMode.Auto
            };

            await foreach (var update in _chatClient.GetStreamingResponseAsync(chatHistory, options, cancellationToken))
            {
                if (!string.IsNullOrEmpty(update.Text))
                {
                    chatResponse.Append(update.Text);
                    yield return update.Text;
                }
            }

            await _unitOfWork.Redis.ListRightPushAsync($"chat_history:{userId}", $"{prompt}:{chatResponse}");
        }

        public async Task ClearChatHistoryAsync()
        {
            var userId = _context?.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _unitOfWork.Redis.KeyDeleteAsync($"chat_history:{userId}");
        }
    }
}
