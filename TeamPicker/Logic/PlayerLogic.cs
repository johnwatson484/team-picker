using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using TeamPicker.Classes;

namespace TeamPicker.Logic
{
    public class PlayerLogic
    {
        static readonly string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        readonly string fileName = Path.Combine(path, "PlayerData.xml");

        public void CheckFileExists()
        {
            if (!File.Exists(fileName))
            {
                string content = @"<?xml version=""1.0"" encoding=""utf - 8"" ?><PlayerData><Version>1.00</Version><Players></Players></PlayerData>";

                File.WriteAllText(fileName, content);
            }
        }

        public void UpdateSelection(List<Player> players)
        {
            PlayerData playerData = DeSerialize();

            foreach (Player player in players)
            {
                Player exPlayer = playerData.Players.FirstOrDefault(x => x.PlayerID == player.PlayerID);

                if (exPlayer != null)
                {
                    exPlayer.Selections = exPlayer.Selections + 1;
                }
            }

            Serialize(playerData);
        }

        public void Serialize(PlayerData playerData)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PlayerData));

            using (TextWriter writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, playerData);
            }
        }

        public PlayerData DeSerialize()
        {
            PlayerData playerData;

            XmlSerializer serializer = new XmlSerializer(typeof(PlayerData));

            using (TextReader reader = new StreamReader(fileName))
            {
                playerData = (PlayerData)serializer.Deserialize(reader);
            }

            return playerData;
        }

        public List<PlayerSelection> SelectAll()
        {
            PlayerData playerData = DeSerialize();

            List<PlayerSelection> players = new List<PlayerSelection>();

            foreach (Player player in playerData.Players)
            {
                PlayerSelection selection = new PlayerSelection
                {
                    Player = player,
                    Selected = false
                };

                players.Add(selection);
            }

            return players;
        }

        public Player SelectByID(Guid playerID)
        {
            PlayerData playerData = DeSerialize();

            Player player = playerData.Players.FirstOrDefault(x => x.PlayerID == playerID);

            return player;
        }

        public void Create(Player player)
        {
            PlayerData playerData = DeSerialize();

            playerData.Players.Add(player);

            Serialize(playerData);
        }

        public void Update(Player player)
        {
            PlayerData playerData = DeSerialize();

            Player exPlayer = playerData.Players.FirstOrDefault(x => x.PlayerID == player.PlayerID);

            if (exPlayer != null)
            {
                exPlayer.PlayerName = player.PlayerName;
                exPlayer.Rating = player.Rating;
                exPlayer.UniqueRole = player.UniqueRole;
                exPlayer.Notes = player.Notes;

                Serialize(playerData);
            }
        }

        public void Delete(Guid playerID)
        {
            PlayerData playerData = DeSerialize();

            Player player = playerData.Players.FirstOrDefault(x => x.PlayerID == playerID);

            if (player != null)
            {
                playerData.Players.Remove(player);

                Serialize(playerData);
            }
        }
    }
}