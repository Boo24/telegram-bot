using System.Collections.Generic;
using FakeItEasy;
using NUnit.Framework;
using source.App;
using source.App.Commands;
using source.Infrastructure.Databases;
using source.Domain.Model;

namespace Tests
{
    [TestFixture]
    class CookBotTests
    {
        IBotCommand fakeCommand;
        string fakeCommandName = "/fakeCommand";
        List<IBotCommand> commandsList;
        IDatabase<Recipe> database;
        CookBot cookBot;

        [SetUp]
        public void SetUp()
        {
            fakeCommand = A.Fake<IBotCommand>();
            commandsList = new List<IBotCommand>();
            commandsList.Add(fakeCommand);
            A.CallTo(() => fakeCommand.Name).Returns(fakeCommandName);
            database = A.Fake<IDatabase<Recipe>>();
            cookBot = new CookBot(database, commandsList);
        }

        [Test]
        public void should_find_and_execute_command()
        {
            var expectedResult = "commandResult";

            A.CallTo(() => fakeCommand.Execute(A<IDatabase<Recipe>>.Ignored, 
                new string[] {})).Returns(expectedResult);

            
            var commandResult = cookBot.HandleCommand(fakeCommandName);
            Assert.AreEqual(commandResult, expectedResult);
        }

        [Test]
        public void should_not_find_and_execute_command()
        {
            A.CallTo(() => fakeCommand.Execute(A<IDatabase<Recipe>>.Ignored,
                new string[] { })).MustNotHaveHappened();

            cookBot.HandleCommand("Non-existent command");
        }
    }
}
