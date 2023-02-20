using System.Collections.ObjectModel;
using AIChatModel;
using Drastic.Tools;
using Drastic.ViewModels;
using ThoughtBot.Utilities;

namespace ThoughtBot
{
    public class AIChatViewModel : BaseViewModel
    {
        private string message;
        private AIClientWrapper wrapper;
        private byte[] placeholderIcon;
        private bool sessionStarted;

        public AIChatViewModel(IServiceProvider services)
            : base(services)
        {
            this.placeholderIcon = LibraryUtilities.GetPlaceholderIcon();
            this.wrapper = services.GetService(typeof(AIClientWrapper)) as AIClientWrapper ?? throw new NullReferenceException(nameof(AIClientWrapper));
            this.SendMessageCommand = new AsyncCommand(async () => await this.SendCommandAsync(this.Message), () => !string.IsNullOrEmpty(this.Message) && this.sessionStarted, this.Dispatcher, this.ErrorHandler);
            this.SendMessageWithStringCommand = new AsyncCommand<string>(this.SendCommandAsync, (x) => !string.IsNullOrEmpty(x) && this.sessionStarted, this.ErrorHandler);
        }

        public event EventHandler<EventArgs>? OnSendingMessage;

        public ObservableCollection<ChatMessage> Messages { get; private set; } = new ObservableCollection<ChatMessage>();

        public string Message
        {
            get { return this.message; }
            set
            {
                this.SetProperty(ref this.message, value);
                this.SendMessageCommand.RaiseCanExecuteChanged();
            }
        }

        public AsyncCommand SendMessageCommand { get; }

        public AsyncCommand<string> SendMessageWithStringCommand { get; }

        public override Task OnLoad()
        {
            if (!this.Messages.Any())
            {
                this.StartSessionAsync().FireAndForgetSafeAsync();
            }

            return Task.CompletedTask;
        }

        private async Task StartSessionAsync()
        {
            this.sessionStarted = true;
            var chatMessage = new ChatMessage(this.placeholderIcon, ChatMessageType.AI);
            this.Messages.Add(chatMessage);
            chatMessage.Message = await this.wrapper.StartAsync();
        }

        private async Task EndSessionAsync()
        {
            this.sessionStarted = false;
            var chatMessage = new ChatMessage(this.placeholderIcon, ChatMessageType.AI);
            this.Messages.Add(chatMessage);
            chatMessage.Message = await this.wrapper.StopAsync();
        }


        private async Task SendCommandAsync(string message)
        {
            this.Message = string.Empty;
            this.OnSendingMessage?.Invoke(this, new EventArgs());
            this.Messages.Add(new ChatMessage(this.placeholderIcon, message, ChatMessageType.User));
            await Task.Delay(1000);
            await this.PerformBusyAsyncTask(() => this.SendQueryToModelAsync(message));
        }

        private async Task SendQueryToModelAsync(string message)
        {
            var chatMessage = new ChatMessage(this.placeholderIcon, ChatMessageType.AI);
            this.Messages.Add(chatMessage);

            var result = await this.wrapper.QueryAsync(message);
            if (!string.IsNullOrEmpty(result))
            {
                chatMessage.Message = result;
            }
            else
            {
                // If we get an empty response from the model, something went wrong.
                // Remove the loading message.
                this.Messages.Remove(chatMessage);
            }
        }
    }
}
