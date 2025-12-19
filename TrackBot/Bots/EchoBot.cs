// Generated with EchoBot .NET Template version v4.22.0

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace EchoBot.Bots
{
    public class EchoBot : ActivityHandler
    {
        private readonly IFuenfzehnZeitWrapper _fuenfzehnZeitWrapper;

        public EchoBot(IFuenfzehnZeitWrapper fuenfzehnZeitWrapper)
        {
            _fuenfzehnZeitWrapper = fuenfzehnZeitWrapper;
        }
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var replyText = $"Echo (Id Test): {turnContext.Activity.Text}";
            await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
            await turnContext.SendActivityAsync($"UserId: {turnContext.Activity.From.Id}");
            await turnContext.SendActivityAsync($"ConversationId: {turnContext.Activity.Conversation.Id}");

            await SendSuggestedActionsAsync(turnContext, cancellationToken);

            if (turnContext.Activity.Text == UserCommandType.GetStatus.ToString())
            {
                var status = await _fuenfzehnZeitWrapper.GetStatusAsync();
                await turnContext.SendActivityAsync($"{UserCommandType.GetStatus} initiated");

                await turnContext.SendActivityAsync($"Status: {status}");
            }
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome, my friend! Let's start a conversation";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }

        private static async Task SendSuggestedActionsAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var reply = MessageFactory.Text("What is your favorite color?");

            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                    {
                        new CardAction() { Title = "Status", Type = ActionTypes.ImBack, Value = UserCommandType.GetStatus.ToString()},
                        new CardAction() { Title = "Yellow", Type = ActionTypes.ImBack, Value = "Yellow"},
                        new CardAction() { Title = "Blue", Type = ActionTypes.ImBack, Value = "Blue", Image = "https://via.placeholder.com/20/0000FF?text=B", ImageAltText = "B" },
                    },
            };
            await turnContext.SendActivityAsync(reply, cancellationToken);
        }
    }
}
