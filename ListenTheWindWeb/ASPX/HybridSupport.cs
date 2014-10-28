using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;

namespace Web.ASPX
{
    public class WebFormController : Controller { }

    public static class WebFormMvcUtil
    {

        public static void RenderPartial(string partialName, object model)
        {
            //get a wrapper for the legacy WebForm context 
            var httpCtx = new HttpContextWrapper(System.Web.HttpContext.Current);

            //create a mock route that points to the empty controller 
            var rt = new RouteData();
            rt.Values.Add("controller", "WebFormController");

            //create a controller context for the route and http context 
            var ctx = new ControllerContext(
                new RequestContext(httpCtx, rt), new WebFormController());

            //find the partial view using the viewengine 
            var view = ViewEngines.Engines.FindPartialView(ctx, partialName).View;

            var sb = new StringBuilder();
            var textwriter = new StringWriter(sb);
            var htmlwriter = new HtmlTextWriter(textwriter);

            //create a view context and assign the model 
            var vctx = new ViewContext(ctx, view,
                new ViewDataDictionary { Model = model },
                new TempDataDictionary(), htmlwriter);

            //render the partial view 
            view.Render(vctx, HttpContext.Current.Response.Output);
        }

        public static void RenderPartial(string partialName, Dictionary<string, object> viewData)
        {
            //get a wrapper for the legacy WebForm context 
            var httpCtx = new HttpContextWrapper(System.Web.HttpContext.Current);

            //create a mock route that points to the empty controller 
            var rt = new RouteData();
            rt.Values.Add("controller", "WebFormController");

            //create a controller context for the route and http context 
            var ctx = new ControllerContext(
                new RequestContext(httpCtx, rt), new WebFormController());

            //find the partial view using the viewengine 
            var view = ViewEngines.Engines.FindPartialView(ctx, partialName).View;

            var sb = new StringBuilder();
            var textwriter = new StringWriter(sb);
            var htmlwriter = new HtmlTextWriter(textwriter);

            var viewDataDictionary = new ViewDataDictionary();
            if (viewData != null)
            {
                foreach (var j in viewData)
                {
                    viewDataDictionary.Add(j);
                }
            }

            //create a view context and assign the model 
            var vctx = new ViewContext(ctx, view,
                                       viewDataDictionary,
                                       new TempDataDictionary(), htmlwriter);

            //render the partial view 
            view.Render(vctx, HttpContext.Current.Response.Output);

        }

    } 

}