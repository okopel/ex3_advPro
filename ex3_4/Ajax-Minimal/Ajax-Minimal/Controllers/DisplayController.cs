using Ex3.Models;
using System.Net;
using System.Web.Mvc;

namespace Ex3.Controllers
{
    public class DisplayController : Controller
    {

        // GET: First
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet] // show dynamic location Action
        public ActionResult showLocations(string ip, int port, int time)
        {
            InfoModel i = InfoModel.Instance;
            i.ip = ip;
            i.port = port;
            i.client.openClient();
            Session["time"] = time;
            return View();
        }

        [HttpGet] // show one location or load Actions
        public ActionResult showOneLocation(string ip, int port)
        {
            InfoModel i = InfoModel.Instance;//todo check if already connected but another ip
            IPAddress d;
            // Check if action is ShowOneLocation or load
            if (!IPAddress.TryParse(ip, out d)) // Load case
            {
                Session["time"] = port;
                i.fileName = ip;
                i.initionXmlReader();
                return View("play");
            }

            // show one location case
            i.ip = ip;
            i.port = port;
            i.client.openClient();
            return View();
        }

        [HttpPost] // get data from simulator - return XML file
        public string GetArgs()
        {
            return InfoModel.Instance.client.getArgs(false);
        }

        [HttpPost] // close client Action
        public void closeClient()
        {
            InfoModel.Instance.client.close();
        }

        [HttpPost] // read one arg from saved XML file
        public string GetOneArg()
        {
            return InfoModel.Instance.getOneArg();
        }

        [HttpPost] // close the reader of the saved XML file
        public void closeReader()
        {
            InfoModel.Instance.closeReader();
        }
    }
}