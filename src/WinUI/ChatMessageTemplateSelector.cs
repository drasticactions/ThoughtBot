using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThoughtBot.WinUI
{
    public class ChatMessageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate User { get; set; }
        public DataTemplate AI { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is ChatMessage message)
            {
                switch (message.Type)
                {
                    case ChatMessageType.AI:
                        return AI;
                    case ChatMessageType.User:
                        return User;
                    default:
                        break;
                }
            }

            return User;
        }
    }
}
