
using Syncfusion.Blazor.InteractiveChat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socializer.BlazorHybrid.ViewModels
{
    internal class ChatViewModel
    {
        
        public List<ChatMessage> Messages { get; set; } = [new ChatMessage() { Text = "DUPA" }];


    }
}
