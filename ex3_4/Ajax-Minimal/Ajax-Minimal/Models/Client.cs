using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Xml;

namespace Ex3.Models
{
    public class Client
    {
        private TcpClient client;
        private bool _isConnected;
        public string ip { get; set; }
        public string LastName { get; set; }
        public int port { get; set; }
        public double lat { get { return getData("/position/latitude-deg"); } }
        public double lon { get { return getData("/position/longitude-deg"); } }
        public double throttle { get { return getData("/controls/engines/current-engine/throttle"); } }
        public double rudder { get { return getData("/controls/flight/rudder"); } }


        public void ToXml(XmlWriter writer,bool toFile)
        {
            double la = this.lat;
            double lo = this.lon;
            double th = this.throttle;
            double ra = this.rudder;
            if (toFile)
            {
                InfoModel.Instance.addToXml(la, lo, th, ra);
            }
            writer.WriteStartElement("client");
            writer.WriteElementString("lat", la.ToString());
            writer.WriteElementString("lon", lo.ToString());
            writer.WriteElementString("throttle", th.ToString());
            writer.WriteElementString("rudder", ra.ToString());
            writer.WriteEndElement();
        }



        public string getArgs(bool toFile)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);
            writer.WriteStartDocument();

            ToXml(writer,toFile);

            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }

        public void openClient()
        {
            if (client != null && client.Connected && _isConnected)
            {
                return;
            }
            string ip = InfoModel.Instance.ip;
            int CommandPort = InfoModel.Instance.port;
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), CommandPort);
            client = new TcpClient();
            // try to connect
            while (!client.Connected)
            {
                try { client.Connect(ep); }
                catch (Exception) { }
            }
            _isConnected = true;
        }

        private string askDataFromServer(string msg)
        {
            // send stream
            Stream stm = client.GetStream();
            ASCIIEncoding asen = new ASCIIEncoding();
            Byte[] ba;
            Byte[] bb = new byte[100];
            if (_isConnected && client.Connected)
            {
                ba = asen.GetBytes(msg);
                stm.Write(ba, 0, ba.Length);
                int k = stm.Read(bb, 0, 100);
                // get feedback
                Console.Write("feedback:");
                string buffer = "";
                //todo validate that response=""
                for (int i = 0; i < k; i++)
                {
                    char c = Convert.ToChar(bb[i]);
                    Console.Write(c);
                    buffer += c;
                }
                return buffer;
            }
            else
            {
                Console.WriteLine("Client Close!!!");
                return null;
            }
        }

        // close client
        public void close()
        {
            _isConnected = false;
            if (client != null)
            {
                this.client.Close();
            }
        }
        private double getData(string msg)
        {
            //Random r = new Random();
            //return r.NextDouble() * 100;
            string req = this.askDataFromServer("get " + msg + "\r\n");
            return this.parser(req);
        }
        private double parser(string str)
        {
            int f = str.IndexOf("'");
            if (f == -1)
            {
                return 0.000000001; //todo!!
            }
            str = str.Substring(f + 1);
            f = str.IndexOf("'");
            if (f == -1)
            {
                return 0.000000001;
            }
            str = str.Substring(0, f);
            double ans = 0;
            try { ans = Convert.ToDouble(str); }
            catch
            {
                throw new Exception("To Dobule in Client");
            }
            return ans;

        }
    }
}