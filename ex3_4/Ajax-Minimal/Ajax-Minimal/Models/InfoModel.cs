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
        public string ip { get; set; }
        private int _port;
        public int port { get { return _port; }
            set
            {
                //reopen client by changing port
                if (_port != value)
                {
                    this.client.close();
                    this._port = value;
                    this.client.openClient();
                }
            }
        }
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

        // open XML writer
        public void initionXml(string fileName)
        {
            closeXml();
            string path = HttpContext.Current.Server.MapPath("~/App_Data/" + fileName + ".xml");
            xmlWriter = XmlWriter.Create(path);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("myData");

        }


        // open XML reader
        public void initionXmlReader()
        {
            closeReader();
            string path = HttpContext.Current.Server.MapPath("~/App_Data/" + fileName + ".xml");
            xmlReader = new XmlTextReader(path);
        }

        // close XML file
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


        // add entry to XML file
        public void addToXml(string lat, string lon, string throttle, string rudder)
        {
            if (xmlWriter != null)
            {
                xmlWriter.WriteStartElement("arg");
                xmlWriter.WriteElementString("lat", lat);
                xmlWriter.WriteElementString("lon", lon);
                xmlWriter.WriteElementString("throttle", throttle);
                xmlWriter.WriteElementString("rudder", rudder);
                xmlWriter.WriteEndElement();
            }
        }

        // close reader
        public void closeReader()
        {
            if (xmlReader != null)
            {
                this.xmlReader.Close();
                this.xmlReader = null;
            }
        }


        // read one entry from XML file
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

        // // read one entry from server
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