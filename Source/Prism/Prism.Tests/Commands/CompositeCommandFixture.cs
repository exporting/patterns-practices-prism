//===============================================================================
// Microsoft patterns & practices
// Composite WPF (PRISM)
//===============================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================

using System;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prism.Commands;

namespace Prism.Tests.Commands
{
    /// <summary>
    /// Summary description for MultiDispatchCommandFixture
    /// </summary>
    [TestClass]
    public class CompositeCommandFixture
    {

        [TestMethod]
        public void ShouldRegisterCommand()
        {
            TestableCompositeCommand multiCommand = new TestableCompositeCommand();
            TestCommand testCommand = new TestCommand();

            Assert.IsFalse(multiCommand.HasCommand(testCommand));
            multiCommand.RegisterCommand(new TestCommand());
            Assert.IsTrue(multiCommand.HasCommand(testCommand));
        }

        [TestMethod]
        public void RegisterACommandShouldRaiseCanExecuteEvent()
        {
            TestableCompositeCommand multiCommand = new TestableCompositeCommand();
            TestCommand testCommand = new TestCommand();

            multiCommand.RegisterCommand(new TestCommand());
            Assert.IsTrue(multiCommand.CanExecuteChangedRaised);
        }

        [TestMethod]
        public void ShouldDelegateExecuteToSingleRegistrant()
        {
            TestableCompositeCommand multiCommand = new TestableCompositeCommand();
            TestCommand testCommand = new TestCommand();

            multiCommand.RegisterCommand(testCommand);

            Assert.IsFalse(testCommand.ExecuteCalled);
            multiCommand.Execute(null);
            Assert.IsTrue(testCommand.ExecuteCalled);
        }

        [TestMethod]
        public void ShouldDelegateExecuteToMultipleRegistrants()
        {
            TestableCompositeCommand multiCommand = new TestableCompositeCommand();
            TestCommand testCommandOne = new TestCommand();
            TestCommand testCommandTwo = new TestCommand();

            multiCommand.RegisterCommand(testCommandOne);
            multiCommand.RegisterCommand(testCommandTwo);

            Assert.IsFalse(testCommandOne.ExecuteCalled);
            Assert.IsFalse(testCommandTwo.ExecuteCalled);
            multiCommand.Execute(null);
            Assert.IsTrue(testCommandOne.ExecuteCalled);
            Assert.IsTrue(testCommandTwo.ExecuteCalled);
        }

        [TestMethod]
        public void ShouldDelegateCanExecuteToSingleRegistrant()
        {
            TestableCompositeCommand multiCommand = new TestableCompositeCommand();
            TestCommand testCommand = new TestCommand();

            multiCommand.RegisterCommand(testCommand);

            Assert.IsFalse(testCommand.CanExecuteCalled);
            multiCommand.CanExecute(null);
            Assert.IsTrue(testCommand.CanExecuteCalled);
        }

        [TestMethod]
        public void ShouldDelegateCanExecuteToMultipleRegistrants()
        {
            TestableCompositeCommand multiCommand = new TestableCompositeCommand();
            TestCommand testCommandOne = new TestCommand();
            TestCommand testCommandTwo = new TestCommand();

            multiCommand.RegisterCommand(testCommandOne);
            multiCommand.RegisterCommand(testCommandTwo);

            Assert.IsFalse(testCommandOne.CanExecuteCalled);
            Assert.IsFalse(testCommandTwo.CanExecuteCalled);
            multiCommand.CanExecute(null);
            Assert.IsTrue(testCommandOne.CanExecuteCalled);
            Assert.IsTrue(testCommandTwo.CanExecuteCalled);
        }

        [TestMethod]
        public void CanExecuteShouldReturnTrueIfAllRegistrantsTrue()
        {
            TestableCompositeCommand multiCommand = new TestableCompositeCommand();
            TestCommand testCommandOne = new TestCommand() { CanExecuteValue = true };
            TestCommand testCommandTwo = new TestCommand() { CanExecuteValue = true };

            multiCommand.RegisterCommand(testCommandOne);
            multiCommand.RegisterCommand(testCommandTwo);

            Assert.IsTrue(multiCommand.CanExecute(null));
        }

        [TestMethod]
        public void CanExecuteShouldReturnFalseIfASingleRegistrantsFalse()
        {
            TestableCompositeCommand multiCommand = new TestableCompositeCommand();
            TestCommand testCommandOne = new TestCommand() { CanExecuteValue = true };
            TestCommand testCommandTwo = new TestCommand() { CanExecuteValue = false };

            multiCommand.RegisterCommand(testCommandOne);
            multiCommand.RegisterCommand(testCommandTwo);

            Assert.IsFalse(multiCommand.CanExecute(null));
        }

        [TestMethod]
        public void ShouldReraiseCanExecuteChangedEvent()
        {
            TestableCompositeCommand multiCommand = new TestableCompositeCommand();
            TestCommand testCommandOne = new TestCommand() { CanExecuteValue = true };

            Assert.IsFalse(multiCommand.CanExecuteChangedRaised);
            multiCommand.RegisterCommand(testCommandOne);
            testCommandOne.FireCanExecuteChanged();
            Assert.IsTrue(multiCommand.CanExecuteChangedRaised);
        }

        [TestMethod]
        public void UnregisterCommandRemovesFromExecuteDelegation()
        {
            TestableCompositeCommand multiCommand = new TestableCompositeCommand();
            TestCommand testCommandOne = new TestCommand() { CanExecuteValue = true };

            multiCommand.RegisterCommand(testCommandOne);
            multiCommand.UnregisterCommand(testCommandOne);

            Assert.IsFalse(testCommandOne.ExecuteCalled);
            multiCommand.Execute(null);
            Assert.IsFalse(testCommandOne.ExecuteCalled);
        }

        [TestMethod]
        public void UnregisterCommandShouldRaiseCanExecuteEvent()
        {
            TestableCompositeCommand multiCommand = new TestableCompositeCommand();
            TestCommand testCommandOne = new TestCommand();

            multiCommand.RegisterCommand(testCommandOne);
            multiCommand.CanExecuteChangedRaised = false;
            multiCommand.UnregisterCommand(testCommandOne);

            Assert.IsTrue(multiCommand.CanExecuteChangedRaised);
        }

        [TestMethod]
        public void ExecuteDoesNotThrowWhenAnExecuteCommandModifiesTheCommandsCollection()
        {
            TestableCompositeCommand multiCommand = new TestableCompositeCommand();
            SelfUnregisterableCommand commandOne = new SelfUnregisterableCommand(multiCommand);
            SelfUnregisterableCommand commandTwo = new SelfUnregisterableCommand(multiCommand);

            multiCommand.RegisterCommand(commandOne);
            multiCommand.RegisterCommand(commandTwo);

            multiCommand.Execute(null);

            Assert.IsTrue(commandOne.ExecutedCalled);
            Assert.IsTrue(commandTwo.ExecutedCalled);
        }


        [TestMethod]
        public void UnregisterCommandDisconnectsCanExecuteChangedDelegate()
        {
            TestableCompositeCommand multiCommand = new TestableCompositeCommand();
            TestCommand testCommandOne = new TestCommand() { CanExecuteValue = true };

            multiCommand.RegisterCommand(testCommandOne);
            multiCommand.UnregisterCommand(testCommandOne);
            multiCommand.CanExecuteChangedRaised = false;
            testCommandOne.FireCanExecuteChanged();
            Assert.IsFalse(multiCommand.CanExecuteChangedRaised);
        }


        [TestMethod]
        public void ExecuteShouldHappenOnceWhenRegisteredMultipleTimes()
        {
            TestableCompositeCommand multiCommand = new TestableCompositeCommand();
            TestCommand testCommand = new TestCommand() { CanExecuteValue = true };

            multiCommand.RegisterCommand(testCommand);
            multiCommand.RegisterCommand(testCommand);

            Assert.AreEqual<int>(0, testCommand.ExecuteCallCount);
            multiCommand.Execute(null);
            Assert.AreEqual<int>(1, testCommand.ExecuteCallCount);

        }

        [TestMethod, ExpectedException(typeof(DivideByZeroException))]
        public void ShouldBubbleException()
        {
            TestableCompositeCommand multiCommand = new TestableCompositeCommand();
            BadDivisionCommand testCommand = new BadDivisionCommand();

            multiCommand.RegisterCommand(testCommand);
            multiCommand.Execute(null);
        }

        [TestMethod]
        public void CanExecuteShouldReturnFalseWithNoCommandsRegistered()
        {
            TestableCompositeCommand multiCommand = new TestableCompositeCommand();
            Assert.IsFalse(multiCommand.CanExecute(null));
        }

        [TestMethod]
        public void ShouldResetCanExecuteDelegateList()
        {
            var multiCommand = new TestableCompositeCommand();
            bool canExecuteCalled = false;

            multiCommand.CanExecuteChanged += delegate
                                                  {
                                                      canExecuteCalled = true;
                                                  };

            Assert.IsTrue(multiCommand.DelegateCount > 1);  // expect at least default empty delegate + our delegate
            multiCommand.ClearExecuteChangedDelegates();
            Assert.AreEqual<int>(1, multiCommand.DelegateCount);  // expect default empty delegate
        }


        internal class TestableCompositeCommand : CompositeCommand
        {
            public bool CanExecuteChangedRaised;

            public TestableCompositeCommand()
            {
                CanExecuteChanged += delegate
                                         {
                                             CanExecuteChangedRaised = true;
                                         };
            }

            public new bool HasCommand(ICommand command)
            {
                return base.HasCommand;
            }

            public int DelegateCount
            {
                get
                {
                    return base.CanExecuteChangedDelegateCount;
                }
            }
        }

        internal class TestCommand : ICommand
        {
            public bool CanExecuteCalled { get; set; }
            public bool ExecuteCalled { get; set; }
            public int ExecuteCallCount { get; set; }

            public bool CanExecuteValue = true;

            public void FireCanExecuteChanged()
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
            #region ICommand Members

            public bool CanExecute(object parameter)
            {
                CanExecuteCalled = true;
                return CanExecuteValue;
            }

            public event EventHandler CanExecuteChanged = delegate { };

            public void Execute(object parameter)
            {
                ExecuteCalled = true;
                ExecuteCallCount += 1;
            }

            #endregion
        }

        internal class BadDivisionCommand : ICommand
        {
            #region ICommand Members

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                throw new DivideByZeroException("Test Divide By Zero");
            }

            #endregion
        }

        internal class SelfUnregisterableCommand : ICommand
        {
            public CompositeCommand Command;
            public bool ExecutedCalled = false;

            public SelfUnregisterableCommand(CompositeCommand command)
            {
                Command = command;
            }

            #region ICommand Members

            public bool CanExecute(object parameter)
            {
                throw new NotImplementedException();
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                Command.UnregisterCommand(this);
                ExecutedCalled = true;
            }

            #endregion
        }
    }
}