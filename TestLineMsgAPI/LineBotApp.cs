using Line.Messaging;
using Line.Messaging.Webhooks;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TestLineMsgAPI
{
    internal class LineBotApp : WebhookApplication
    {
        private LineMessagingClient messagingClient { get; }

        public LineBotApp(LineMessagingClient lineMessagingClient)
        {
            this.messagingClient = lineMessagingClient;
        }

        private async Task Initialize(MessageEvent ev)
        {
            var lineId = ev.Source.Id;
        }

        #region Handlers

        protected override async Task OnMessageAsync(MessageEvent ev)
        {
            switch (ev.Message.Type)
            {
                case EventMessageType.Text:
                    await HandleTextAsync(ev.ReplyToken, ((TextEventMessage)ev.Message).Text, ev.Source.UserId);
                    break;
                case EventMessageType.Image:
                    await HandleTextAsync(ev.ReplyToken, "รูป", ev.Source.UserId);
                    break;
                case EventMessageType.Audio:
                    await HandleTextAsync(ev.ReplyToken, "เสียง", ev.Source.UserId);
                    break;
                case EventMessageType.Video:
                    await HandleTextAsync(ev.ReplyToken, "วิดีโอ", ev.Source.UserId);
                    break;
                case EventMessageType.File:
                    await HandleTextAsync(ev.ReplyToken, "ไฟล์", ev.Source.UserId);
                    break;
                case EventMessageType.Location:
                    await HandleTextAsync(ev.ReplyToken, "โลเคชั่น", ev.Source.UserId);
                    break;
                case EventMessageType.Sticker:
                    await HandleTextAsync(ev.ReplyToken, "สติกเกอร์", ev.Source.UserId);
                    break;
            }
        }

        protected override async Task OnPostbackAsync(PostbackEvent ev)
        {
            string text = "";
            switch (ev.Postback.Data)
            {
                case "Date":
                    text = "You chose the date: " + ev.Postback.Params.Date;
                    break;
                case "Time":
                    text = "You chose the time: " + ev.Postback.Params.Time;
                    break;
                case "DateTime":
                    text = "You chose the date-time: " + ev.Postback.Params.DateTime;
                    break;
                default:
                    text = "Your postback is " + ev.Postback.Data;
                    break;
            }
        }

        protected override async Task OnFollowAsync(FollowEvent ev)
        {
            var userName = "";
            if (!string.IsNullOrEmpty(ev.Source.Id))
            {
                var userProfile = await messagingClient.GetUserProfileAsync(ev.Source.Id);
                userName = userProfile?.DisplayName ?? "";
            }

            await messagingClient.ReplyMessageAsync(ev.ReplyToken, $"สวัสดี {userName} ยินดีตอนรับสู่ LineMsg ครั้งแรก");
        }

        protected override async Task OnUnfollowAsync(UnfollowEvent ev)
        {

        }

        protected override async Task OnJoinAsync(JoinEvent ev)
        {
        }

        protected override async Task OnLeaveAsync(LeaveEvent ev)
        {
        }

        protected override async Task OnBeaconAsync(BeaconEvent ev)
        {
            var message = "";
            switch (ev.Beacon.Type)
            {
                case BeaconType.Enter:
                    message = "You entered the beacon area!";
                    break;
                case BeaconType.Leave:
                    message = "You leaved the beacon area!";
                    break;
                case BeaconType.Banner:
                    message = "You tapped the beacon banner!";
                    break;
            }

            await messagingClient.ReplyMessageAsync(ev.ReplyToken, $"{message}(Dm:{ev.Beacon.Dm}, Hwid:{ev.Beacon.Hwid})");
        }

        #endregion

        private async Task HandleTextAsync(string replyToken, string userMessage, string userId)
        {
            var userName = "";
            if (!string.IsNullOrEmpty(userId))
            {
                var userProfile = await messagingClient.GetUserProfileAsync(userId);
                userName = userProfile?.DisplayName ?? "";
            }

            await messagingClient.ReplyMessageAsync(replyToken, $"สวัสดี {userName} คุณได้ทำการส่ง {userMessage} เข้ามา");
        }
    }
}