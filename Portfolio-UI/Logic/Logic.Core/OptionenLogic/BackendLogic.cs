using Data.Types.OptionTypes;
using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Logic.Core.OptionenLogic
{
    public class BackendLogic
    {
        private static readonly string FILENAME = "conf.ini";

        private readonly FileIniDataParser parser;
        private IniData iniData;
        private readonly string iniPath;

        private string ip;
        private string url;
        private BackendProtokollTypes typ;
        public int? port;
        public BackendLogic()
        {
            ip = "";
            iniPath = "";
            port = null;
            url = "";
            typ = BackendProtokollTypes.http;

            iniPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Portfolio\\";
            if (!Directory.Exists(iniPath))
                Directory.CreateDirectory(iniPath);
            iniPath += FILENAME;
            parser = new FileIniDataParser();
        }

        public string getBackendIP()
        {
            return ip;
        }

        public string getURL()
        {
            var URL = "http://";
            if (typ.Equals(BackendProtokollTypes.https))
                URL = "https://";
            if (this.url.Length == 0)
                URL += getBackendIP();
            else
                URL += GetBackendURL();

            if (port.HasValue)
                URL += ":" + port.Value;
            return URL;
        }

        public BackendProtokollTypes getProtokollTyp()
        {
            return typ;
        }

        public bool istINIVorhanden()
        {
            return File.Exists(iniPath);
        }

        public void LoadData()
        {
            
            if (File.Exists(iniPath))
            {
                iniData = parser.ReadFile(iniPath);
                LoadBackendSettings();
            }
        }
        public void SaveData(string ip, BackendProtokollTypes protokollTyp, int? port, string url)
        {
            this.ip = ip;
            this.typ = protokollTyp;
            this.port = port;
            this.url = url;

            iniData = new IniData();
            iniData.Sections.AddSection("Backend-Settings");

            iniData.Sections.GetSectionData("Backend-Settings").Keys.AddKey("IP", ip);
            iniData.Sections.GetSectionData("Backend-Settings").Keys.AddKey("URL", url);
            iniData.Sections.GetSectionData("Backend-Settings").Keys.AddKey("Protokoll", Convert.ToString ( Convert.ToInt32(protokollTyp) ) );
            iniData.Sections.GetSectionData("Backend-Settings").Keys.AddKey("Port", Convert.ToString(port));

            parser.WriteFile(iniPath, iniData);
        }


        private void LoadBackendSettings()
        {
            if (isFieldVorhanden("Backend-Settings", "IP"))
                ip = iniData.Sections["Backend-Settings"].GetKeyData("IP").Value;
            if (isFieldVorhanden("Backend-Settings", "URL"))
                url = iniData.Sections["Backend-Settings"].GetKeyData("URL").Value;

            if ((isFieldVorhanden("Backend-Settings", "Protokoll")) && int.TryParse(iniData.Sections["Backend-Settings"].GetKeyData("Protokoll").Value, out _))
                typ = (BackendProtokollTypes)int.Parse(iniData.Sections["Backend-Settings"].GetKeyData("Protokoll").Value);
            if ((isFieldVorhanden("Backend-Settings", "Port")) && int.TryParse(iniData.Sections["Backend-Settings"].GetKeyData("Port").Value, out _))
                port = int.Parse(iniData.Sections["Backend-Settings"].GetKeyData("Port").Value);
        }

        public int? getBackendPort()
        {
            return port;
        }
        public string GetBackendURL()
        {
            return url;
        }
        public bool isFieldVorhanden(string section, string key)
        {
            return (iniData.Sections[section].GetKeyData(key) != null);
        }
    }
}
