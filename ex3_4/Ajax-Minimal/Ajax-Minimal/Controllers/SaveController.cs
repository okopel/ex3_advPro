using Ex3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Ex3.Controllers
{
    public class SaveController : Controller
    {
        // GET: Save
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult save(string ip, int port, int time, int len, string fileName)
        {
            InfoModel i = InfoModel.Instance;//todo check if already connected but another ip
            i.ip = ip;
            i.port = port;
            i.time = time;
            i.len = len;
            i.fileName = fileName;
            Session["time"] = time;
            Session["timeOut"] = len;
            i.initionXml(fileName);
            if (ip != "123")
            {
                i.client.openClient();
            }
            return View();
        }

        [HttpPost]
        public string GetArgs()
        {
            return InfoModel.Instance.client.getArgs(true);
        }

        [HttpPost]
        public string SaveArgs()
        {
            InfoModel.Instance.closeXml();
            return "finish";
        }
        
    }
}