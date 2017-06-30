using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LanguageFeatures.Models;
using System.Threading.Tasks;
using System.Net.Http;

namespace LanguageFeatures.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public string Index()
        {
            return "Navigate to a URL to show an example";
        }
        public ViewResult AutoProperty()
        {
            var product = new Product();
            product.Name = "Kayak";
            string productName = product.Name;
            return View("Result", (Object)String.Format("Product name:{0}", product.Name));
        }

        public async Task<long?> GetPageLength()
        {
            HttpClient client = new HttpClient();
            var httpTask = client.GetAsync("http://www.baidu.com");
            var httpMessage = await client.GetAsync("http://www.baidu.com");
            var tt = httpTask.ContinueWith(
                (Task<HttpResponseMessage> antecedent) =>
                {
                    return antecedent.Result.Content.Headers.ContentLength;
                }
            );
            return httpMessage.Content.Headers.ContentLength;
        }
    }
}