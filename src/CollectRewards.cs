using BepInEx;
using NodeCanvas.BehaviourTrees;
using SideLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace BossRush
{
    /*
    public class CollectRewards
    {
        public List<string> droptableUIDs;
        public List<BossRushRewardData> bossRushRewardData;

        public CollectRewards() 
        {
            droptableUIDs = new List<string>();
            bossRushRewardData = new List<BossRushRewardData>();

            // Collect UIDs
            List<string> classPathCollection = new List<string>();
            string[] bepInExPluginPlugins = Directory.GetDirectories(Paths.PluginPath);
            for (int i = 0; i < bepInExPluginPlugins.Length; i++)
            {
                string pluginFolderPath = bepInExPluginPlugins[i];
                if (pluginFolderPath != null) 
                {
                    string[] allXMLPathsInPlugin = Directory.GetFiles(pluginFolderPath, "*.xml", SearchOption.AllDirectories);
                    string[] validXMLPaths = FilterXMLPaths(allXMLPathsInPlugin);
                    classPathCollection.AddRange(validXMLPaths);
                }
            }
            
            // Now to serialize the collected paths
            for (int j = 0; j < classPathCollection.Count; j++)
            {
                string classPath = classPathCollection[j];
                bossRushRewardData.Add(DeserializeXml<BossRushRewardData>(classPath));
            }
            BossRushPlugin.bossRushRewardDatas = bossRushRewardData;
        }
        private string[] FilterXMLPaths(string[] inpaths)
        {
            // Converting to lists first
            List<string> pathsList = inpaths.ToList();
            List<string> outpathsList = new List<string>();

            // Checks each xml individually
            for (int i = 0; i < pathsList.Count; i++) 
            {
                string path = pathsList[i];
                if (IsXmlOfType<BossRushRewardData>(path))
                {
                    outpathsList.Add(path);
                }
            }
            string[] outpaths = outpathsList.ToArray();
            return outpaths;
        }

        public static bool IsXmlOfType<T>(string xmlPath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (FileStream fs = new FileStream(xmlPath, FileMode.Open))
                using (XmlReader reader = XmlReader.Create(fs))
                {
                    return serializer.CanDeserialize(reader);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during validation: {ex.Message} Merry Christmas!");
                return false;
            }
        }

        public static T DeserializeXml<T>(string xmlPath)
        {
            if (!File.Exists(xmlPath))
            {
                BossRushPlugin.Log.LogError($"XML file not found at path: {xmlPath}");
                throw new FileNotFoundException($"File not found at {xmlPath}");
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                using (FileStream stream = new FileStream(xmlPath, FileMode.Open))
                {
                    T data = (T)serializer.Deserialize(stream);
                    BossRushPlugin.Log.LogMessage("XML Deserialization Successful!");
                    return data;
                }
            }
            catch (Exception ex)
            {
                BossRushPlugin.Log.LogError($"Failed to deserialize XML: {ex.Message}");
                throw;
            }
        }
    }

    [Serializable] // Required for Unity
    [XmlRoot("BossRushRewardContainer")] // Optional, specifies the root element name in XML
    public class BossRushRewardData
    {
        [XmlArray("DropTableUID")]
        [XmlArrayItem("String")]
        public List<string> Strings = new List<string>();

        // Optional: Parameterless constructor for XmlSerializer
        public BossRushRewardData() { }

        public BossRushRewardData(List<string> strings)
        {
            Strings = strings;
        }
    }
    */
}
