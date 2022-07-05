using MostValuablePlayer.Classes;
using MostValuablePlayer.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MostValuablePlayer
{
    class Program
    {
        static void Main(string[] args)
        {
            StartProgram();
        }

        static void StartProgram()
        {
            try
            {
                var listOfBasketballPlayers = new List<Player>();
                var listOfHandballPlayers = new List<Player>();
                var basketballMvp = new Player();
                var handballMvp = new Player();


                string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\");
                string[] files = Directory.GetFiles(path);

                foreach (var file in files)
                {
                    if (File.ReadAllLines(file)[0] == "BASKETBALL")
                    {
                        listOfBasketballPlayers = ParseBasketballFile(File.ReadAllLines(file));
                        basketballMvp = GetMvp(listOfBasketballPlayers);
                        Console.WriteLine("Basketball MVP: " + basketballMvp.PlayerName + ", Nickname: " + basketballMvp.PlayerNickname);
                    }
                    else if (File.ReadAllLines(file)[0] == "HANDBALL")
                    {
                        listOfHandballPlayers = ParseHandballFile(File.ReadAllLines(file));
                        handballMvp = GetMvp(listOfHandballPlayers);
                        Console.WriteLine("Handball MVP: " + handballMvp.PlayerName + ", Nickname: " + handballMvp.PlayerNickname);
                    }
                }
                var tournamentMvp = CalculateTournamentMvp(basketballMvp, handballMvp);
                Console.WriteLine("Tournament MVP: " + tournamentMvp.PlayerName + ", Nickname: " + tournamentMvp.PlayerNickname);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        static Player CalculateTournamentMvp(Player basketballMvp, Player handballMvp)
        {
            return basketballMvp.Contribution > handballMvp.Contribution ? basketballMvp : handballMvp;
        }

        static Player GetMvp(List<Player> playerData)
        {
            try
            {
                var listOfTeams = new List<Team>();
                var listOfPlayers = new List<Player>();

                var teams = playerData.GroupBy(x => x.TeamName).ToList();


                foreach (var item in teams)
                {
                    var team = new Team();
                    team.TeamName = item.Key;
                    team.Players = item.ToList();
                    team.TotalScore = CalculateTeamScore(team.Players.ToList());

                    listOfTeams.Add(team);

                    team.IsWinner = IsWinner(listOfTeams, team.TeamName);

                    foreach (var data in team.Players)
                    {
                        var player = new Player();
                        player = data;
                        player.Team = team;

                        listOfPlayers.Add(player);
                    }
                }


                var biggestContribution = listOfPlayers.Max(x => x.Contribution);
                return listOfPlayers.Where(x => x.Contribution == biggestContribution).FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw new Exception("Could not get MVP! Exception Message: " + ex.Message);
            }
            
            
        }

        static int CalculateTeamScore(List<Player> player)
        {
            try
            {
                int totalTeamScore = 0;

                foreach (var item in player)
                {
                    if (item.Sports == EnumSports.Basketball)
                    {
                        totalTeamScore += item.BasketballStats.ScoredPoint;
                    }
                    else if (item.Sports == EnumSports.Handball)
                    {
                        totalTeamScore += item.HandballStats.GoalsMade;
                    }
                }

                return totalTeamScore;
            }
            catch (Exception ex)
            {

                throw new Exception("Could not calculated total team score data! Exception Message: " + ex.Message);
            }
        }

        static bool IsWinner(List<Team> teams, string teamName)
        {
            bool isWinner = false;

            var teamScore = teams.Where(x => x.TeamName == teamName).First().TotalScore;
            var winnerScore = teams.Max(x => x.TotalScore);

            if (teamScore == winnerScore)
            {
                isWinner = true;
            }

            return isWinner;
        }

        static List<Player> ParseBasketballFile(string[] file)
        {
            try
            {
                var listOfPlayers = new List<Player>();

                var basketballFile = file.ToList();
                basketballFile.Remove("BASKETBALL");

                foreach (var item in basketballFile)
                {
                    var line = item.Split(';');

                    var player = new Player();
                    var basketballStats = new BasketballStats();
                    player.Sports = EnumSports.Basketball;
                    player.PlayerName = line[0];
                    player.PlayerNickname = line[1];
                    player.Number = int.Parse(line[2]);
                    player.TeamName = line[3];
                    player.Position = line[4];

                    basketballStats.ScoredPoint = int.Parse(line[5]);
                    basketballStats.Rebound = int.Parse(line[6]);
                    basketballStats.Assist = int.Parse(line[7]);

                    player.BasketballStats = basketballStats;

                    listOfPlayers.Add(player);

                }

                return listOfPlayers;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while parsing the basketball file! Exception Message: " + ex.Message );
            }
           
        }

        static List<Player> ParseHandballFile(string[] file)
        {
            try
            {
                var listOfPlayers = new List<Player>();

                var basketballFile = file.ToList();
                basketballFile.Remove("HANDBALL");

                foreach (var item in basketballFile)
                {
                    var line = item.Split(';');

                    var player = new Player();
                    var handballStats = new HandballStats();
                    player.Sports = EnumSports.Handball;
                    player.PlayerName = line[0];
                    player.PlayerNickname = line[1];
                    player.Number = int.Parse(line[2]);
                    player.TeamName = line[3];
                    player.Position = line[4];

                    handballStats.GoalsMade= int.Parse(line[5]);
                    handballStats.GoalsReceived = int.Parse(line[6]);

                    player.HandballStats = handballStats;

                    listOfPlayers.Add(player);

                }

                return listOfPlayers;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while parsing the basketball file! Exception Message: " + ex.Message);
            }
        }


    }
}
