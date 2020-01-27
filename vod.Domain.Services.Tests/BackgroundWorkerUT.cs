using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using vod.Domain.Services.Boundary;
using vod.Domain.Services.Boundary.Interfaces.Enums;

namespace vod.Domain.Services.Tests
{
    public class BackgroundWorkerUT
    {
        private Mock<IBackgroundWorkerStateManager> _stateManagerMock;
        private BackgroundWorker _backgroundWorker;

        private void BaseArrange()
        {
            _stateManagerMock = new Mock<IBackgroundWorkerStateManager>(); 
            _backgroundWorker = new BackgroundWorker(_stateManagerMock.Object);
        }

        [Test]
        public void Execute_ShouldReturnIfStateManagerIsBusy()
        {
            BaseArrange();
            _stateManagerMock.Setup(x => x.IsBusy()).Returns(true);
            var counter = 0;
            Func<bool> funcToDo = () => { counter++; return true; };

            _backgroundWorker.Execute(It.IsAny<MovieTypes>(), funcToDo);

            Assert.AreEqual(0, counter);
        }


        [Test]
        public void Execute_ShouldExecuteFuncIfStateManagerIsNotBusy()
        {
            BaseArrange();
            _stateManagerMock.Setup(x => x.IsBusy()).Returns(false);
            var counter = 0;
            Func<bool> funcToDo = () => {
                counter++;
                return true;
            };

            _backgroundWorker.Execute(It.IsAny<MovieTypes>(), funcToDo);
            Thread.Sleep(50); //waiting for func in another thread

            Assert.AreEqual(1, counter);
        }

        [Test]
        public void Execute_ShouldExecuteFuncOfOneTypeOnlyOnce()
        {
            BaseArrange();
            _stateManagerMock.Setup(x => x.IsBusy()).Returns(false);
            var counter = 0;
            Func<bool> funcToDo = () => {
                counter++;
                return true;
            };

            _backgroundWorker.Execute(MovieTypes.Action, funcToDo);
            _backgroundWorker.Execute(MovieTypes.Action, funcToDo);
            Thread.Sleep(50); //waiting for func in another thread

            Assert.AreEqual(1, counter);
        }

        [Test]
        public void Execute_ShouldExecuteFuncForEveryGivenType()
        {
            BaseArrange();
            _stateManagerMock.Setup(x => x.IsBusy()).Returns(false);
            var counter = 0;
            Func<bool> funcToDo = () => {
                counter++;
                return true;
            };

            _backgroundWorker.Execute(MovieTypes.Action, funcToDo);
            _backgroundWorker.Execute(MovieTypes.Cartoons, funcToDo);
            _backgroundWorker.Execute(MovieTypes.Thriller, funcToDo);
            Thread.Sleep(50); //waiting for func in another thread

            Assert.AreEqual(3, counter);
        }
    }
}
