using System.Collections.Generic;
using System.Threading.Tasks;
using MessagePusher.Core;
using MessagePusher.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MessagePusher.Web.Controllers
{
    public class ApiController : Controller
    {
        private static readonly IEnumerable<IMessageReceiver> Receivers = Bind.GetAllReceivers();
        private static readonly IEnumerable<IMessageSender> Senders = Bind.GetAllSenders();
        private readonly ILogger _logger;

        public ApiController(ILogger<ApiController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Go(string resource)
        {
            return await Action(resource, Request);
        }

        [HttpPost]
        [ActionName("Go")]
        public async Task<IActionResult> GoPost(string resource)
        {
            return await Action(resource, Request);
        }


        private async Task<IActionResult> Action(string resource, HttpRequest request)
        {
            var result = new Result { Success = false };
            var receiver = Bind.GetReceiver(resource, request.Method, Receivers);
            if (receiver == null)
            {
                result.Message = "receiver does not exists!";
                return NotFound(result);
            }
            await receiver.Init(request);

            if (!receiver.Verify())
            {
                result.Message = "Invalid request or nothing to push.";
                return StatusCode(StatusCodes.Status403Forbidden, result);
            }

            var message = receiver.Receive();
            var senders = Bind.GetSenders(receiver.SendTo, Senders);
            foreach (var sender in senders)
            {
                var r = await sender.Send(message);
                if (r.Success)
                {
                    _logger.LogInformation($"[{resource}]send to [{sender.Name}] success!");
                }
                else
                {
                    _logger.LogError($"[{resource}]send to [{sender.Name}] failed! Error: {r.Message}");
                }
            }
            result.Success = true;
            result.Message = "succes";
            return Json(result);
        }
    }
}