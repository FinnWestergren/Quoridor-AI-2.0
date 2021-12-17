using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Server.Game;
using Server.Players.Agent;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Agent
{
    [TestClass]
    public class MiniMaxAgentTests
    {
        private IGame _game;
        private Guid _playerId = new Guid("c8d9d320-2cb0-4d7b-b851-a6ad7bb8db45");
        private const int _winningAction = 69; // nice
        private const int _losingAction = 70; // not nice :(
        private IAgent _agent;
        private int selectedAction = 0;

        private int ActionValue() => selectedAction switch
        {
            _winningAction => 1,
            _losingAction => -1,
            _ => 0
        };

        private void Init()
        {

            var winningMockAction = new Mock<IGameAction>();
            winningMockAction.Setup(a => a.SerializedAction).Returns(_winningAction);
            winningMockAction.Setup(a => a.CommittedBy).Returns(_playerId);

            var losingMockAction = new Mock<IGameAction>();
            losingMockAction.Setup(a => a.SerializedAction).Returns(_losingAction);
            losingMockAction.Setup(a => a.CommittedBy).Returns(_playerId);

            var initialGame = new Mock<IGame>();
            initialGame.Setup(g => g.PlayerOne).Returns(_playerId);
            initialGame.Setup(g => g.GetBoardValue(_playerId)).Returns(ActionValue);
            initialGame.Setup(g => g.GetPossibleMoves(_playerId)).Returns(new List<IGameAction> { winningMockAction.Object, losingMockAction.Object });

            initialGame.Setup(g => g.UndoAction()).Callback(() => _game = initialGame.Object);
            initialGame.Setup(g => g.CommitAction(_winningAction, _playerId)).Callback(() => selectedAction = _winningAction);
            initialGame.Setup(g => g.CommitAction(_losingAction, _playerId)).Callback(() => selectedAction = _losingAction);

            _game = initialGame.Object;
            _agent = new MiniMaxAgent(_playerId);
        }

        [TestMethod]
        public void ChoosesWinningMove()
        {
            Init();
            var action = _agent.GetNextAction(_game);
            Assert.AreEqual(action.SerializedAction, _winningAction);
        }
    }
}
