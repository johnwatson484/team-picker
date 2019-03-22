using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using System.Xml.Serialization;
using TeamPicker.Classes;

namespace TeamPicker.Logic
{
    public class SettingsLogic
    {
        static string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        string fileName = Path.Combine(path, "SettingsData.xml");

        public void CheckFileExists()
        {
            if (!File.Exists(fileName))
            {
                string content = @"<?xml version=""1.0"" encoding=""utf - 8"" ?><SettingsData><Version>3.00</Version><NumberOfTeams>2</NumberOfTeams><BalanceTeams>true</BalanceTeams><TrafficLightRatings>true</TrafficLightRatings></SettingsData>";

                File.WriteAllText(fileName, content);
            }
        }

        public void Serialize(SettingsData settingsData)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SettingsData));

            using (TextWriter writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, settingsData);
            }
        }

        public SettingsData DeSerialize()
        {
            SettingsData settingsData = new SettingsData();

            XmlSerializer serializer = new XmlSerializer(typeof(SettingsData));

            using (TextReader reader = new StreamReader(fileName))
            {
                settingsData = (SettingsData)serializer.Deserialize(reader);
            }

            return settingsData;
        }

        public void Update(SettingsData newSettings)
        {
            SettingsData settingsData = DeSerialize();

            settingsData.NumberOfTeams = newSettings.NumberOfTeams;
            settingsData.MaximumRating = newSettings.MaximumRating;
            settingsData.BalanceTeams = newSettings.BalanceTeams;
            settingsData.OrderBy = newSettings.OrderBy;
            settingsData.TrafficLightRatings = newSettings.TrafficLightRatings;
            settingsData.HighAccuracy = newSettings.HighAccuracy;
            settingsData.DisplayNotes = newSettings.DisplayNotes;

            Serialize(settingsData);
        }

        public SettingsData Select()
        {
            SettingsData settingsData = DeSerialize();

            if (settingsData.Version == 1.00M)
            {
                settingsData.BalanceTeams = true;
                settingsData.TrafficLightRatings = true;
                settingsData.Version = 3.00M;
                Serialize(settingsData);
            }

            if (settingsData.Version == 2.00M)
            {
                settingsData.TrafficLightRatings = true;
                settingsData.Version = 3.00M;
                Serialize(settingsData);
            }

            if (string.IsNullOrEmpty(settingsData.OrderBy))
            {
                settingsData.OrderBy = "Most Selected";
            }

            if (settingsData.MaximumRating == 0)
            {
                settingsData.MaximumRating = 20;
            }

            return settingsData;
        }
    }
}