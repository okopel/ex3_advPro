using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace Ex3.Models
{
    public class InfoModel
    {
        #region
        private static InfoModel s_instace = null;
        public static InfoModel Instance
        {
            get
            {
                if (s_instace == null)
                {
                    s_instace = new InfoModel();
                }
                return s_instace;
            }
        }
        #endregion
        public Client client { get; private set; }
        private string _ip;
        public string ip
        {
            get
            {
                return _ip;
            }
            set
            {
                if (value != _ip)
                {
                    closeReader();
                    closeXml();
                    client.close();
                    _ip = value;

                }
            }
        }
        private int _port;
        public int port
        {
            get { return _port; }
            set
            {
                if (_port != value)
                {
                    closeReader();
                    closeXml();
                    client.close();
                    _port = value;

                }
            }

        }
        public int time { get; set; }
        public int len { get; set; }
        private string _fileName;
        public string fileName
        {
            get { return _fileName; }
            set
            {
                if (_fileName != value)
                {
                    closeReader();
                    closeXml();
                    client.close();
                    _fileName = value;

                }
            }
        }
        public XmlWriter xmlWriter { get; private set; }
        public XmlTextReader xmlReader { get; private set; }

        public InfoModel()
        {
            client = new Client();
            xmlWriter = null;
            xmlReader = null;
        }


        public void initionXml(string fileName)
        {
            closeXml();
            string path = HttpContext.Current.Server.MapPath("~/App_Data/" + fileName + ".xml");
            xmlWriter = XmlWriter.Create(path);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("myData");

        }
        public void initionXmlReader()
        {
            closeReader();
            string path = HttpContext.Current.Server.MapPath("~/App_Data/" + fileName + ".xml");
            xmlReader = new XmlTextReader(path);
        }
        public void closeXml()
        {
            if (xmlWriter != null)
            {
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
                xmlWriter.Close();
                xmlWriter = null;
            }
        }
        public void addToXml(double lat, double lon, double throttle, double rudder)
        {
            if (xmlWriter != null)
            {
                xmlWriter.WriteStartElement("arg");
                xmlWriter.WriteElementString("lat", lat.ToString());
                xmlWriter.WriteElementString("lon", lon.ToString());
                xmlWriter.WriteElementString("throttle", throttle.ToString());
                xmlWriter.WriteElementString("rudder", rudder.ToString());
                xmlWriter.WriteEndElement();
            }
        }

        public void closeReader()
        {
            if (xmlReader != null)
            {
                this.xmlReader.Close();
                this.xmlReader = null;
            }
        }
        public bool readOneArg(XmlWriter writer)
        {
            if (writer == null)
            {
                return false;
            }
            string la = string.Empty, lo = string.Empty, ra = string.Empty, th = string.Empty;
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Text)
                {
                    if (string.IsNullOrEmpty(la))
                    {
                        la = xmlReader.Value;
                        continue;
                    }
                    else if (string.IsNullOrEmpty(lo))
                    {
                        lo = xmlReader.Value;
                        continue;
                    }
                    else if (string.IsNullOrWhiteSpace(th))
                    {
                        th = xmlReader.Value;
                        continue;
                    }
                    else if (string.IsNullOrWhiteSpace(ra))
                    {
                        ra = xmlReader.Value;
                        break;
                    }
                }
            }
            if (string.IsNullOrEmpty(la))
            {
                return false;
            }
            writer.WriteElementString("lat", la);
            writer.WriteElementString("lon", lo);
            writer.WriteElementString("throttle", th);
            writer.WriteElementString("rudder", ra);
            return true;
        }

        public string getOneArg()
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("data");
            bool isGood = readOneArg(writer);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            if (isGood)
            {
                return sb.ToString();
            }
            return null;

        }
    }
}