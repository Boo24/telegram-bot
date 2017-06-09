using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FakeItEasy;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using source.App.Commands;
using source.Domain.Model;
using source.Infrastructure.Databases;

namespace Tests
{
    [TestFixture]
    internal class CommansTests
    {
        private IDatabase<Recipe> database = A.Fake<IDatabase<Recipe>>();
        [SetUp]
        public void setUp()
        {
            database = A.Fake<IDatabase<Recipe>>();
        }

        [Test]
        public void should_call_db_recipe_list_command()
        {
            var com = new RecipeListCommand();
            com.Execute(database, new string[] { });
            A.CallTo(() => database.GetAllSuitable(A<Func<Recipe, bool>>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void should_call_db_recipe_name_command()
        {
            var com = new RecipeByNameCommand();
            com.Execute(database, new string[] { });
            A.CallTo(() => database.GetAnySuitable(A<Func<Recipe, bool>>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
        }



    }
}
