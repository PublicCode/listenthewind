using System.Web.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace T2VSoft.MVC.Core
{
    public class JsonNetResult : ActionResult
    {
        /// <summary>
        /// The result object to render using JSON.
        /// </summary>
        public object Result { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {

            context.HttpContext.Response.ContentType = "application/json";

            var serializer = new JsonSerializer();

                serializer.Serialize(context.HttpContext.Response.Output, Result);
        }
    }
}