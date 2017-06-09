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
using System.Collections.Immutable;

namespace Tests
{
    [TestFixture]
    internal class CommandsTests
    {
        private EasyDatabase database;
        private List<IRecipe> fakeRecipes;

        [SetUp]
        public void setUp()
        {
            fakeRecipes = new List<IRecipe>();

            var fakeRecipe1 = CreateFakeRecipe("бутерброд", "хлеб", 1, "ед");
            var fakeRecipe2 = CreateFakeRecipe("кекс", "мука", 10, "кг");

            fakeRecipes.Add(fakeRecipe1);
            fakeRecipes.Add(fakeRecipe2);

            database = new EasyDatabase(fakeRecipes);
        }

        public IRecipe CreateFakeRecipe(string name, string ingrName,
            double ingrCount, string ingrMeasureUnit)
        {
            var fakeRecipe = A.Fake<IRecipe>();
            var fakeIngredient = A.Fake<IIngredient>();
            var fakeIngredientAmount = A.Fake<IIngredientAmount>();

            A.CallTo(() => fakeRecipe.Name).Returns(name);
            A.CallTo(() => fakeIngredient.Name).Returns(ingrName);
            A.CallTo(() => fakeIngredientAmount.Count).Returns(ingrCount);
            A.CallTo(() => fakeIngredientAmount.MeasureUnit).Returns(ingrMeasureUnit);
            A.CallTo(() => fakeRecipe.Components)
                .Returns(new Dictionary<IIngredient, IIngredientAmount>()
                { { fakeIngredient, fakeIngredientAmount} }.ToImmutableDictionary());

            return fakeRecipe;
        }

        [Test]
        public void RecipeListCommandTest()
        {
            var actualResult = new RecipeListCommand(database).Execute(null).Result;
            var expectedResult = "1. бутерброд\n2. кекс\n";
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void RecipeByNameCommandGoodTest()
        {
            var actualResult = new RecipeByNameCommand(database)
                .Execute( new string[] { "КЕКС" });
            Assert.AreEqual(BotCode.Good, actualResult.Code);
            Assert.AreEqual(fakeRecipes.ElementAt(1).GetPrintableView(), actualResult.Result);
        }

        [Test]
        public void RecipeByNameCommandBadTest()
        {
            var actualResult = new RecipeByNameCommand(database)
                .Execute(new string[] { "молоко" }).Code;
            Assert.AreEqual(BotCode.Bad, actualResult);
        }

        [Test]
        public void RecipeByIngredientsCommandGoodTest()
        {
            var actualResult = new RecipeByIngredientsCommand(database)
                .Execute(new string[] { "хлеб" });
            Assert.AreEqual(BotCode.Good, actualResult.Code);
            Assert.AreEqual(fakeRecipes.ElementAt(0).GetPrintableView(), actualResult.Result);
        }

        [Test]
        public void RecipeByIngredientsCommandBadTest()
        {
            var actualResult = new RecipeByIngredientsCommand(database)
                .Execute(new string[] { "вода" });
            Assert.AreEqual(BotCode.Bad, actualResult.Code);
        }
    }

    public class EasyDatabase : IDatabase<IRecipe>
    {
        public List<IRecipe> Recipes;

        public EasyDatabase(List<IRecipe> fakeRecipes)
            => Recipes = fakeRecipes;

        public IEnumerable<IRecipe> GetAllSuitable(Func<IRecipe, bool> condition)
        {
            return Recipes.Where(condition);
        }

        public IRecipe GetAnySuitable(Func<IRecipe, bool> condition)
        {
            return Recipes.First(condition);
        }
    }
}
