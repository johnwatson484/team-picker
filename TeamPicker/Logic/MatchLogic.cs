using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using TeamPicker.Classes;

namespace TeamPicker.Logic
{
    public class MatchLogic
    {
        static readonly string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        readonly string fileName = Path.Combine(path, "MatchData_v2.xml");
        readonly PlayerLogic pLogic = new PlayerLogic();
        readonly SettingsLogic sLogic = new SettingsLogic();

        int totalVariance = 0;

        public void CheckFileExists()
        {
            if (!File.Exists(fileName))
            {
                string content = @"<?xml version=""1.0"" encoding=""utf - 8"" ?><MatchData><Version>2.00</Version></MatchData>";

                File.WriteAllText(fileName, content);
            }
        }

        public void Serialize(MatchData matchData)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MatchData));

            using (TextWriter writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, matchData);
            }
        }

        public MatchData DeSerialize()
        {
            MatchData matchData = new MatchData();

            XmlSerializer serializer = new XmlSerializer(typeof(MatchData));

            using (TextReader reader = new StreamReader(fileName))
            {
                matchData = (MatchData)serializer.Deserialize(reader);
            }

            return matchData;
        }

        public List<Match> SelectAll()
        {
            MatchData matchData = DeSerialize();

            List<Match> matches = matchData.Matches.OrderByDescending(x => x.DatePicked).ToList();

            return matches;
        }

        public Match SelectByID(Guid matchID)
        {
            MatchData matchData = DeSerialize();

            Match match = matchData.Matches.Where(x => x.MatchID == matchID).FirstOrDefault();

            return match;
        }

        public void Create(Match match)
        {
            MatchData matchData = DeSerialize();

            match.MatchID = Guid.NewGuid();
            match.DatePicked = DateTime.Now;

            matchData.Matches.Add(match);

            Serialize(matchData);
        }

        public void Update(Match match)
        {
            MatchData matchData = DeSerialize();

            Match exMatch = matchData.Matches.Where(x => x.MatchID == match.MatchID).FirstOrDefault();

            if (exMatch != null)
            {
                foreach (Team team in match.Teams)
                {
                    Team exTeam = exMatch.Teams.Where(x => x.TeamID == team.TeamID).FirstOrDefault();

                    if (exTeam != null)
                    {
                        exTeam.Score = team.Score;
                    }
                }

                Serialize(matchData);
            }
        }

        public void Delete(Guid matchID)
        {
            MatchData matchData = DeSerialize();

            Match match = matchData.Matches.Where(x => x.MatchID == matchID).FirstOrDefault();

            if (match != null)
            {
                matchData.Matches.Remove(match);

                Serialize(matchData);
            }
        }

        public Match PickTeams(PlayerData selectedPlayers, bool repick = false, bool randomPick = false)
        {
            if (!repick)
            {
                pLogic.UpdateSelection(selectedPlayers.Players);
            }

            SettingsData settings = sLogic.Select();

            if (!settings.BalanceTeams)
            {
                randomPick = true;
            }

            Match match = new Match();

            for (int i = 0; i < settings.NumberOfTeams; i++)
            {
                Team team = new Team(i + 1);
                match.Teams.Add(team);
            }

            List<PlayerMatch> matchPlayers = CreateMatchPlayersRandom(selectedPlayers.Players);

            if (!randomPick)
            {
                if (!settings.HighAccuracy)
                {
                    var eligiblePlayers = matchPlayers.Where(x => !x.Player.UniqueRole).ToList();
                    int playersToUpdate = (int)(eligiblePlayers.Count() * 0.2);

                    for (int i = 0; i < playersToUpdate; i++)
                    {
                        matchPlayers.Where(x => x.Player.PlayerID == eligiblePlayers[i].Player.PlayerID).FirstOrDefault().Player.UniqueRole = true;
                    }
                }

                matchPlayers = matchPlayers.OrderByDescending(x => x.Player.UniqueRole).ThenByDescending(x => x.Player.Rating).ToList();
            }

            int v = 0;

            foreach (var matchPlayer in matchPlayers)
            {
                if (v == settings.NumberOfTeams)
                {
                    v = 0;
                }
                match.Teams[v].Players.Add(matchPlayer.Player);
                v++;
            }

            match.Teams = match.Teams.Where(x => x.Players.Count > 0).OrderBy(x => x.TeamNumber).ToList();

            if (!randomPick)
            {
                CalculateVariance(match);

                bool changesMade = false;

                int i = 0;
                do
                {
                    changesMade = false;

                    for (int t = 0; t < match.Teams.Count; t++)
                    {
                        for (int p = 0; p < match.Teams[t].Players.Count; p++)
                        {
                            if (!match.Teams[t].Players[p].UniqueRole)
                            {
                                bool comparing = true;
                                int ct = 0;
                                while (comparing && ct < match.Teams.Count)
                                {
                                    if (match.Teams[ct].TeamID != match.Teams[t].TeamID)
                                    {
                                        int cp = 0;
                                        while (comparing && cp < match.Teams[ct].Players.Count)
                                        {
                                            var currentPlayer = match.Teams[t].Players[p];
                                            var comparePlayer = match.Teams[ct].Players[cp];

                                            match.Teams[t].Players[p] = comparePlayer;
                                            match.Teams[ct].Players[cp] = currentPlayer;

                                            int currentVariance = totalVariance;

                                            CalculateVariance(match);

                                            if (totalVariance < currentVariance)
                                            {
                                                comparing = false;
                                                changesMade = true;
                                            }
                                            else
                                            {
                                                match.Teams[t].Players[p] = currentPlayer;
                                                match.Teams[ct].Players[cp] = comparePlayer;
                                                CalculateVariance(match);
                                            }
                                            cp++;
                                        }
                                    }
                                    ct++;
                                }
                            }
                        }
                    }
                    i++;
                }
                while (changesMade && i < 100);

                foreach (var team in match.Teams)
                {
                    team.Players = team.Players.OrderBy(x => x.PlayerName).ToList();
                }
            }

            return match;
        }

        private void CalculateVariance(Match match)
        {
            int top = match.Teams.Max(x => x.Rating);
            int bottom = match.Teams.Min(x => x.Rating);
            totalVariance = top - bottom;
        }

        private List<PlayerMatch> CreateMatchPlayersRandom(List<Player> players)
        {
            List<PlayerMatch> matchPlayers = new List<PlayerMatch>();

            Random random = new Random(Guid.NewGuid().GetHashCode());

            foreach (Player player in players)
            {
                PlayerMatch playerM = new PlayerMatch
                {
                    Player = player,
                    Random = random.Next(1, 1000)
                };

                matchPlayers.Add(playerM);
            }

            matchPlayers = matchPlayers.OrderBy(x => x.Random).ToList();

            return matchPlayers;
        }
    }
}