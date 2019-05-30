using Ex3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Ex3.Controllers
{
    public class DisplayController : Controller
    {

        // GET: First
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult showLocations(string ip, int port, int time)
        {
            InfoModel i = InfoModel.Instance;
            i.ip = ip;
            i.port = port;
            i.time = time;
            i.client.openClient();
            Session["time"] = time;
            return View();
        }
        [HttpGet]
        public ActionResult showOneLocation(string ip, int port)
        {
            InfoModel i = InfoModel.Instance;//todo check if already connected but another ip
            IPAddress d;
            if (!IPAddress.TryParse(ip, out d)) //play from file case
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

        [HttpPost]
        public string GetArgs()
        {

            return InfoModel.Instance.client.getArgs(false);
        }

        [HttpPost]
        public string GetOneArg()
        {
            return InfoModel.Instance.getOneArg();
        }
        [HttpPost]
        public void closeReader()
        {
            InfoModel.Instance.closeReader();
        }
    }
}