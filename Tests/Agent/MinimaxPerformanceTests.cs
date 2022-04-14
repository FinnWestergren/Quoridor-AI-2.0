using CsvHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Game.Quoridor;
using Server.Players.Agent;
using Server.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

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
        public void TestDepthTwoQuoridor()
        {
            var game = new Quoridor();
            var p1 = new MiniMaxAgent(game.PlayerOne, 2);
            var p2 = new MiniMaxAgent(game.PlayerTwo, 0);
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
            WriteResults("DepthTwoQuoridor", results);
        }

        private void WriteResults(string testType, List<Result> results)
        {
            var path = @"C:\Users\fwest\Documents\GitHub\Quoridor-AI-2.0\TestOutput\";
            var dateString = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            var filename = $"{dateString}_{testType}.csv";
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
    }
}
