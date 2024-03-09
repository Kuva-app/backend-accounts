using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Kuva.Accounts.Service.Controllers
{
    public class BaseController: Controller
    {
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
        private CultureInfo CultureInfo { get; set; }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string defaultCulture = "pt-BR";
            try
            {
                if (context.HttpContext.Request.Headers.Any() && context.HttpContext.Request.Headers[Constants.HeaderCultureKey].Any())
                    defaultCulture = context.HttpContext.Request.Headers[Constants.HeaderCultureKey].ToString();
                CultureInfo = new CultureInfo(defaultCulture);
            }
            catch
            {
                CultureInfo = new CultureInfo("pt-BR");
            }
#if DEBUG
            PrintConsole(context.HttpContext.Request, keyValues: context.ActionArguments);
#endif
            return base.OnActionExecutionAsync(context, next);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
#if DEBUG
            PrintConsole(context.HttpContext.Response, result: context.Result);
#endif
            base.OnActionExecuted(context);
        }
        
        #region Printers
        private void PrintConsole(object httpObj, IDictionary<string, object> keyValues = null, IActionResult result = null)
        {
            var defaultColor = Console.ForegroundColor;
            const string requestTitle = "[REQUEST]";
            const string cookieTitle = "[COOKIES]";
            const string responseTitle = "[RESPONSE]";
            try
            {
                switch (httpObj)
                {
                    case HttpRequest request:
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(requestTitle);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"[{request.Host.Value} {request.Host.Port} {request.Method} {request.Path} {request.QueryString.ToUriComponent()}]");
                        Console.ForegroundColor = defaultColor;
                        PrintHeaders(request.Headers);
                        if (request.Cookies.Any())
                        {
                            Console.WriteLine(cookieTitle);
                            request.Cookies.ToList().ForEach(item => Console.WriteLine($"{item.Key}: {item.Value}"));
                        }
                        if (Request.Body.CanRead)
                        {
                            Console.WriteLine(Request.Body);
                        }
                        PrintKeyValues(keyValues);
                        break;
                    }
                    case HttpResponse response:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(responseTitle);
                        PrintStatusCode(response);
                        Console.ForegroundColor = defaultColor;
                        PrintHeaders(response.Headers);
                        PrintResult(result);
                        break;
                }
            }
            finally
            {
                Console.ForegroundColor = defaultColor;
            }
        }
        private static void PrintStatusCode(HttpResponse response)
        {
            Console.ForegroundColor = response.StatusCode switch
            {
                < 200 => ConsoleColor.Yellow,
                >= 200 and < 300 => ConsoleColor.DarkGreen,
                >= 300 => ConsoleColor.Red
            };
            Console.WriteLine($"[{response.HttpContext.Request.Method} {response.StatusCode}]");
        }
        private static void PrintResult(IActionResult result)
        {
            if (result == null)
                return;
            var obj = JsonConvert.SerializeObject(result, Formatting.Indented);
            Console.WriteLine(obj);
        }
        private static void PrintKeyValues(IDictionary<string, object> keyValues)
        {
            if (keyValues == null)
                return;
            var keys = keyValues.Keys.ToList();
            keys.ForEach(item =>
            {
                var obj = JsonConvert.SerializeObject(keyValues[item], Formatting.Indented);
                Console.WriteLine(obj);
            });
        }
        private static void PrintHeaders(IHeaderDictionary headerDictionary)
        {
            const string headerTitle = "[HEADERS]";
            if (headerDictionary == null || !headerDictionary.Any())
                return;
            var header = JsonConvert.SerializeObject(headerDictionary, Formatting.Indented);
            Console.WriteLine(headerTitle);
            Console.WriteLine(header);
        }
        #endregion
    }
}
