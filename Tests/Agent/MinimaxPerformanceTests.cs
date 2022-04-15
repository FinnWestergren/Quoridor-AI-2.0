using CsvHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Game;
using Server.Game.Quoridor;
using Server.Players.Agent;
using Server.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace Tests.Agent
{
    [TestClass]
    public class MinimaxPerformanceTests
    {
        protected class Result
        {
            public string Move { get; set; }
            public int Nodes { get; set; }
            public long Time { get; set; }
        }

        [TestMethod]
        public void TestDepthOneQuoridor()
        {
            var game = new Quoridor();
            var p1 = new MiniMaxAgent(game.PlayerOne, 1);
            var p2 = new MiniMaxAgent(game.PlayerTwo, 1);
            
            WriteResults("DepthOneQuoridor", RunBots(p1,p2,game));
        }

        [TestMethod]
        public void TestDepthTwoQuoridor()
        {
            var game = new Quoridor();
            var p1 = new MiniMaxAgent(game.PlayerOne, 2);
            var p2 = new MiniMaxAgent(game.PlayerTwo, 1);

            WriteResults("DepthTwoQuoridor", RunBots(p1, p2, game));
        }

        [TestMethod]
        public void TestDepthThreeQuoridor()
        {
            var game = new Quoridor();
            var p1 = new MiniMaxAgent(game.PlayerOne, 3);
            var p2 = new MiniMaxAgent(game.PlayerTwo, 1);

            WriteResults("DepthThreeQuoridor", RunBots(p1, p2, game));
        }

        [TestMethod]
        public void BunchaTests()
        {
            var threads = new List<Thread>();
            for (var i = 0; i < 4; i++)
            {
                threads.Add(new Thread(TestDepthOneQuoridor));
                threads.Add(new Thread(TestDepthTwoQuoridor));
                threads.Add(new Thread(TestDepthThreeQuoridor));
            }

            foreach (var t in threads)
            {
                t.Start();
            }
            while (threads.Any(t => t.IsAlive)) ;
        }


        private List<Result> RunBots(MiniMaxAgent p1, MiniMaxAgent p2, IGame game)
        {
            var results = new List<Result>();
            var move = 1;
            while (!game.IsGameOver())
            {
                var (time, r1) = ActionTimer.TimeFunction(() => p1.GetNextAction(game));
                results.Add(new Result
                {
                    Move = move.ToString(),
                    Nodes = p1.NodeCount,
                    Time = time
                });
                game.CommitAction(r1.SerializedAction, p1.PlayerId);
                move++;
                if (!game.IsGameOver())
                {
                    var r2 = p2.GetNextAction(game);
                    game.CommitAction(r2.SerializedAction, p2.PlayerId);
                }
            }
            results.Add(new Result
            {
                Move = "Total",
                Nodes = results.Sum(r => r.Nodes),
                Time = results.Sum(r => r.Time)
            });
            results.Add(new Result
            {
                Move = "Avg",
                Nodes = (int)results.Average(r => r.Nodes),
                Time = (int)results.Average(r => r.Time)
            });
            return results;
        }

        private void WriteResults(string testType, List<Result> results)
        {
            var path = $@"{getRootPath()}\TestOutput\{testType}\";
            Directory.CreateDirectory(path);
            var dateString = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            var filename = $"{dateString}.csv";
            using (var writer = new StreamWriter(path + filename))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteHeader<Result>();
                csv.NextRecord();
                foreach (var record in results)
                {
                    csv.WriteRecord(record);
                    csv.NextRecord();
                }
            }
        }


        private string getRootPath()
        {
            var cd = Environment.CurrentDirectory;
            var root = "Quoridor-AI-2.0";
            var index = cd.IndexOf(root) + root.Length;
            return $@"{cd.Substring(0, index)}";
        }
    }
}
