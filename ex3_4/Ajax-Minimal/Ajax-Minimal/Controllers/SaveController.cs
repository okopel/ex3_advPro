using Ex3.Models;
using System.Web.Mvc;

namespace Ex3.Controllers
{
    public class SaveController : Controller
    {
        // GET: Save
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet] // save data Action
        public ActionResult save(string ip, int port, int time, int len, string fileName)
        {
            InfoModel i = InfoModel.Instance;
            i.ip = ip;
            i.port = port;
            i.fileName = fileName;
            Session["time"] = time;
            Session["timeOut"] = len;
            i.initionXml(fileName);
            i.client.openClient();
            return View();
        }

        [HttpPost] // get XML with DATA about plane
        public string GetArgs()
        {
            return InfoModel.Instance.client.getArgs(true);
        }

        [HttpPost] // in save method, when time passed, save and close.
        public string SaveArgs()
        {
            InfoModel.Instance.closeXml();
            InfoModel.Instance.client.close();
            return "finish";
        }
        
    }
}