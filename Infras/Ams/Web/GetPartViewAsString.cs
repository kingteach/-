using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Ams.Web
{
    /// <summary>
    /// 将部分视图转换为字符串
    /// </summary>
    public static class GetPartViewAsString
    {
        private static readonly Regex m_regex = new Regex(@"(?<=[^])\t{2,}|(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,11}(?=[<])|(?=[\n])\s{2,}");

        public static string RenderViewAsString(this Controller controller, string viewName, object model)
        {
            //如果partialViewName为空,就把action名称作为部分视图名称
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = controller.ControllerContext.RouteData.GetRequiredString("action");
            }
            controller.ViewData.Model = model;
            IView view = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName).View;
            using (StringWriter writer = new StringWriter())
            {
                ViewContext viewContext = new ViewContext(controller.ControllerContext, view, controller.ViewData, controller.TempData, writer);
                viewContext.View.Render(viewContext, writer);
                //return m_regex.Replace(writer.ToString(), string.Empty);
                return writer.ToString();
            }
            //controller.ViewData.Model = model;
            //using (var sw = new System.IO.StringWriter())
            //{
            //    var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
            //    var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
            //    viewResult.View.Render(viewContext, sw);
            //    return m_regex.Replace(sw.GetStringBuilder().ToString(), string.Empty);
            //}
        }

        public static string RenderViewAsString(this Controller controller, string viewName)
        {
            return controller.RenderViewAsString(viewName, null);
        }
    }
}
