using Line.Messaging;
using Line.Messaging.Webhooks;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace TestLineMsgAPI.Controllers
{
    public class LineBotController : ApiController
    {
        private static LineMessagingClient lineMessagingClient;
        private string accessToken = ConfigurationManager.AppSettings["ChannelAccessToken"];
        private string channelSecret = ConfigurationManager.AppSettings["ChannelSecret"];
        public LineBotController()
        {
            if (lineMessagingClient == null)
            {
                lineMessagingClient = new LineMessagingClient(accessToken);
            }
            
        }

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Hook(HttpRequestMessage request)
        {
            var events = await request.GetWebhookEventsAsync(channelSecret);
            
            var app = new LineBotApp(lineMessagingClient);

            await app.RunAsync(events);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}