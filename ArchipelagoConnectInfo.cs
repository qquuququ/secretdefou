using System;
using System.Collections.Generic;
using System.IO;
using BepInEx;
using Newtonsoft.Json;
using UnityEngine;

namespace Archipelago
{
    public class APConnectInfo
    {
        public string host_name = "";
        public string slot_name = "";
        public string password = "";
        public bool death_link = false;
        public HashSet<long> @checked = new HashSet<long>();
        public Dictionary<long, HashSet<long>> resources_granted = new Dictionary<long, HashSet<long>>();

        // ✅ Add this property
        public bool Valid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(host_name) &&
                       !string.IsNullOrWhiteSpace(slot_name);
            }
        }

        public APConnectInfo GetAsLastConnect()
        {
            return this;
        }

        public void WriteToFile(string path)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllText(path, JsonConvert.SerializeObject(this));
            }
            catch (Exception ex)
            {
                Debug.LogError("Could not write connection info: " + ex.Message);
            }
        }
    }
}