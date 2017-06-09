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
    internal class CommandsTests
    {
        private EasyDatabase database;

        [SetUp]
        public void setUp()
        {
            database = new EasyDatabase();
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
            Assert.AreEqual(database.GetRecipe("кекс").GetPrintableView(), actualResult.Result);
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
            Assert.AreEqual(database.GetRecipe("бутерброд").GetPrintableView(), actualResult.Result);
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

        public IRecipe GetRecipe(string name)
        {
            return Recipes.Where(rec => rec.Name == name).First();
        }

        public EasyDatabase()
        {
            Recipes = new List<IRecipe>()
            {
                new Recipe(
                    "бутерброд",
                    "вкусный",
                    new Dictionary<IIngredient, IIngredientAmount>() { {
                            new Ingredient("хлеб"), new IngredientAmount(1, "ед") } }),
                new Recipe(
                    "кекс",
                    "пышный",

                    new Dictionary<IIngredient, IIngredientAmount>() { {
                            new Ingredient("мука"), new IngredientAmount(10, "кг") } })
            };
        }

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
