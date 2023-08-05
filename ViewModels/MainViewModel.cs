using AsyncAwaitBestPractices;
using ChatApp.Interfaces;
using ChatApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace ChatApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool loading;

        [ObservableProperty]
        private IList<Chat> chats;

        [ObservableProperty]
        private IList<Message> messages;

        [ObservableProperty]
        private Chat selectedChat;

        [ObservableProperty]
        private string newMessageText;

        IDataService _dataService { get; }
        IUserService _userService { get; }

        public IAsyncRelayCommand ChangeChatCommand { get; }
        public IAsyncRelayCommand SendMessageCommand { get; }

        public MainViewModel(IDataService dataService, IUserService userService) 
        {
            _dataService = dataService;
            _userService = userService;
            ChangeChatCommand = new AsyncRelayCommand(ChangeChat);
            SendMessageCommand = new AsyncRelayCommand(SendMessage);
        }

        public async Task LoadData(bool fromBackground = false)
        {
            if (!fromBackground)
            {
                Loading = true;
                await Task.Delay(1500);
                Chats = await _dataService.GetChats();
                Messages = (await _dataService.GetMessages(Chats?.FirstOrDefault()?.Id ?? 0)).ToList();

                if (Chats.Count == 0)
                    GenerateRandomMessagesStart().SafeFireAndForget();

                Loading = false;
                return;
            }

            Chats = await _dataService.GetChats();
            Messages = (await _dataService.GetMessages(SelectedChat?.Id ?? 0)).Reverse().ToList();
        }

        public async Task SendMessage()
        {
            if (string.IsNullOrEmpty(NewMessageText))
                return;

            Message newMessage = new()
            {
                ChatId = SelectedChat.Id,
                Text = NewMessageText,
                Date = DateTime.Now,
                UserId = _userService.GetUser().Id
            };

            await _dataService.SaveMessage(newMessage);
            Messages.Add(newMessage);
            var updatedMessages = Messages.Reverse();
            Messages = new List<Message>(updatedMessages);
            NewMessageText = string.Empty;

            await Task.Delay(2000);
            await GenerateNewMessage(SelectedChat.Id);

        }

        async Task ChangeChat()
        {
            Messages = (await _dataService.GetMessages(SelectedChat.Id)).Where(c => c.ChatId == SelectedChat.Id).ToList();
        }

        #region Generating fake chats and messages 10 per session
        async Task GenerateRandomMessagesStart()
        {
            try
            {
                await GenerateNewChats();

                for(int i = 0; i < 5; i++)
                {
                    await GenerateNewMessage();
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Error while generating messages: {ex.Message}");
            }
        }

        async Task GenerateNewChats()
        {
            for(int i = 0; i < 10; i++)
            {
                Chat newChat = new()
                {
                    ChatLastDate = DateTime.Now,
                    ChatLastMessage = 0,
                    ChatName = $"Chat {DateTime.Now.Ticks}",
                };

                await _dataService.SaveChat(newChat);
                Debug.WriteLine($"Chat generated: {newChat.ChatName}");
            }

            await LoadData(true);
        }

        async Task GenerateNewMessage(int chatId = -1)
        {
            if(chatId < 0)
            {
                var from = Chats.FirstOrDefault().Id;
                var to = Chats.LastOrDefault().Id;

                chatId = new Random().Next(from, to);
            }

            Message newMessage = new()
            {
                ChatId = chatId,
                Date = DateTime.Now,
                Text = $"New message for chat {chatId} with ticks: {DateTime.Now.Ticks}",
                UserId = 3
            };

            await _dataService.SaveMessage(newMessage);
            await LoadData(true);
            Debug.WriteLine($"Chat generated: {newMessage.Text}");
        }
        #endregion
    }
}
