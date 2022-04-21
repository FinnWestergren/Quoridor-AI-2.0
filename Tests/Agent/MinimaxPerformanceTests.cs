using CsvHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Game;
using Server.Game.Quoridor;
using Server.Players.Agent;
using Server.Utilities;
using System;
using System.Globalization;
using System.IO;

namespace Tests.Agent
{
    [TestClass]
    public class MinimaxPerformanceTests
    {
        protected class Result
        {
            public int Nodes { get; set; }
            public long Time { get; set; }
            public float TimePerNode => (float)Time / Nodes;
        }

        [TestMethod]
        public void TestDepthOneQuoridor()
        {
            var game = new Quoridor();
            var p1 = new MiniMaxAgent(PLAYER_ID.PLAYER_ONE, 1);
            var (time, r1) = ActionTimer.TimeFunction(() => p1.GetNextAction(game));
            var result = new Result
            {
                Nodes = p1.NodeCount,
                Time = time
            };

            WriteResults("DepthOneQuoridor", result);
        }

        [TestMethod]
        public void TestDepthTwoQuoridor()
        {
            var game = new Quoridor();
            var p1 = new MiniMaxAgent(PLAYER_ID.PLAYER_ONE, 2);
            var (time, r1) = ActionTimer.TimeFunction(() => p1.GetNextAction(game));
            var result = new Result
            {
                Nodes = p1.NodeCount,
                Time = time
            };

            WriteResults("DepthTwoQuoridor", result);
        }

        private void WriteResults(string testType, Result result)
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
                csv.WriteRecord(result);
                csv.NextRecord();
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
