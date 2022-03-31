using MeowBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text;

namespace MeowBlog.Controllers
{
    /// <summary>
    /// 博客控制器 C/
    /// </summary>
    public class BlogController : Controller
    {
        /// <summary>
        /// 主页
        /// </summary>
        /// <returns></returns>
        public IActionResult Index() => View();
        /// <summary>
        /// 显示某个博客
        /// </summary>
        /// <param name="id">Uid</param>
        /// <returns></returns>
        public IActionResult Display(string id)
		{
			try
			{
                ViewData["blogitem"] = BlogManagement.DeserilizeBlogToModel(id);
            }
			catch
			{
                ViewData["blogitem"] = null;
            }
            return View();
        }
        /// <summary>
        /// 插入一个博客
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Insert()
        {
            var k = new StreamReader(Request.HttpContext.Request.Body, Encoding.Default).ReadToEnd();
            var jo = JObject.Parse(k);
            if (
                Request.HttpContext.Session.IsRoot() || //session验证
                (ProgramProperties.Password?.Equals(jo["pass"]?.ToString()) ?? false) //内部密码验证
                )
            {
                if (jo != null)
                {
                    var name = jo["name"]?.ToString() ?? "";
                    var desc = jo["desc"]?.ToString() ?? "";
                    var visa = "yes".Equals(jo["isVisable"]?.ToString() ?? "yes");
                    var text = (jo["body"]?.ToString() ?? "");
                    if (!string.IsNullOrEmpty(text))
                    {
                        BlogModel bm = new() {id=Guid.NewGuid().ToString().Replace("-",""),name = name,description=desc,isVisiable=visa,Text=text,publish=DateTime.Now };
                        bm.Save();
                        ViewData["sc"] = "1";
                        return Content("0");
                    }
                    else
                    {
                        ViewData["sc"] = "0";
                        return Json(new { result = 1 ,errs="Blog Empty, Not Emit to save"});
                    }
                }
                else
                {
                    ViewData["sc"] = "0";
                    return Json(new { result = -1, errs = "Request Body Empty" });
                }
            }
            else
            {
                ViewData["sc"] = "0";
                return Json(new { result = 2, errs = "No session indecated your blog id -> request process stops" });
            }
        }
        /// <summary>
        /// 删除一个博客
        /// </summary>
        /// <param name="id">博客id</param>
        /// <returns></returns>
        public IActionResult Delete(string id)
        {
            if (Request.HttpContext.Session.IsRoot())//session验证
            {
                id.Delete();
                return Redirect("/Blog/Index");
            }
            else
            {
                return Json(new { errs = "No session indecated you're administrator -> request process stops" });
            }
        }
        /// <summary>
        /// 添加一个博客
        /// </summary>
        /// <returns></returns>
        public IActionResult AddBlog()
        {
            if (Request.HttpContext.Session.GetInt32("isRoot") == 0)
            {
                return View();
            }
            else
            {
                return Redirect("/Home/Login");
            }
        }
    }
}
