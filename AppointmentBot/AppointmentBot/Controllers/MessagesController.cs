﻿using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace AppointmentBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            var client = new ConnectorClient(new System.Uri(activity.ServiceUrl), new MicrosoftAppCredentials());
            var reply = activity.CreateReply();
            if (activity.Type == ActivityTypes.Message)
            {
                if (activity.Text.Equals("Yes"))
                {                   
                    reply.Text = $"Please Enter Year";
                    client.Conversations.ReplyToActivityAsync(reply);
                   // await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
                }
            }
            else if (activity.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels

                // Note: Add introduction here:
                IConversationUpdateActivity update = activity;
               
                if (update.MembersAdded != null && update.MembersAdded.Count > 0)
                {
                    foreach (var newMember in update.MembersAdded)
                    {
                        if (newMember.Id != activity.Recipient.Id)
                        {
                            reply.Text = $"Welcome {newMember.Name}! Would you like to book an appointment?";
                            client.Conversations.ReplyToActivityAsync(reply);
                        }
                    }
                }
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}