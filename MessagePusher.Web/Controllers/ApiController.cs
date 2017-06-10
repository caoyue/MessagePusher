﻿using System.Collections.Generic;
using System.Threading.Tasks;
using MessagePusher.Core;
using MessagePusher.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace MessagePusher.Web.Controllers
{
    public class ApiController : Controller
    {
        private readonly IEnumerable<IMessageReceiver> _receivers;
        private readonly IEnumerable<IMessageSender> _senders;
        private readonly ILogger _logger;

        public ApiController(ILogger<ApiController> logger)
        {
            _receivers = Bind.GetAllReceivers();
            _senders = Bind.GetAllSenders();
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Go(string resource)
        {
            return await Action(resource, "Get", Request.Query, Request.Headers, null);
        }

        [HttpPost]
        public async Task<IActionResult> Go(string resource, [FromBody]dynamic requestData)
        {
            return await Action(resource, "Post", Request.Query, Request.Headers, requestData);
        }

        private async Task<IActionResult> Action(string resource, string method,
            IQueryCollection query, IHeaderDictionary headers, JObject json)
        {
            var result = new Result { Success = false };
            var receiver = Bind.GetReceiver(resource, method, _receivers);
            if (receiver == null)
            {
                result.Message = "receiver does not exists!";
                return NotFound(result);
            }

            if (!receiver.Verify(query, headers, json))
            {
                result.Message = "verify request failed!";
                return StatusCode(StatusCodes.Status403Forbidden, result);
            }

            var message = receiver.Receive(query, headers, json);
            var senders = Bind.GetSenders(receiver.SendTo, _senders);
            foreach (var sender in senders)
            {
                var r = await sender.Send(message.Data);
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