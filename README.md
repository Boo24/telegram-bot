# CookBot
## Описание
  Бот - кулинарная книга для Telegram
  Найти в Telegram - @CookingHelperBot
## Авторы
   Ишин Данил, Гладышева Татьяна, Немцев Евгений
## План презентации
### Суть проекта
Данный бот позволяет по некоторым критериям получить рецепт. В данный момент поддерживаются следующие команды:
* **/recipe** - найти рецепт по названию.
* **/ingr** - получить список рецептов, в которых содержится указанный ингридиент.
* **/all** - отобразить список всех рецептов.
* **/help** - информация о поддерживаемых командах.

#### Пример использования команды:
![Example](UsingExample.PNG)

Легко добавлять новые типы команд и использовать различаные базы данных. Также существует более глобальная точка расширения [IBot](/CookBot/source/App/IBot.cs), о которой будет расказано ниже.

Расширяемость команд продемонстрирована выше. Для баз данных - [ArrayDatabase](/CookBot/source/Infrastructure/Databases/ArrayDatabase.cs) (используется массив, который изначально засериализован). Для демонстрации IBot как точки расширения помимо [CookBot](/CookBot/source/App/CookBot.cs), был написан [HelloBot](/CookBot/source/App/HelloBot.cs) (который здоровается с вами в ответ на любое сообщение).


## Точки расширения
1. Команды.

    Для того, чтобы создать новую команду достаточно реализовать интерфейс [IBotCommand](CookBot/source/App/Commands/IBotCommand.cs). У команды должно быть имя(свойство *Name*), описание(свойство *Description*), а также метод *Execute*, в который передается ссылка на базу данных и аргументы команды. Примеры реализации можно посмотреть в [этой папке](/CookBot/source/App/Commands) .
    
2. База данных.

    Для того, чтобы добавить новую базу данных, нужно реализовать дженерик интерфейс [IDatabase](/CookBot/source/Infrastructure/Databases/IDatabase.cs). Этот интерфейс декларирует два метода *GetAllSuitable*, *GetAnySuitable*, в которые передается лямбда-выражение, которому должен удовлетворять искомый объект.

3. Интерфейс [IBot](/CookBot/source/App/IBot.cs)

    Для реализации этого интерфейса необходимо реализовать метод *HandleCommand*, который возвращает ответ на полученное сообщение. Благодаря этому интерфейсу при работе с TelgramAPI(или другим API), мы можем не привязываться к конкретному боту. В нашем проекте есть две реализации этого интерфейса [CookBot](/CookBot/source/App/CookBot.cs) и [HelloBot](/CookBot/source/App/HelloBot.cs). Так же есть класс [TelegramHandler](/CookBot/source/App/TelegramHandler.cs), который работает с TelegramAPI и ему в конструктор передается реализация интерфейса [IBot](/CookBot/source/App/IBot.cs). Тем самым, создав нового бота, мы можем просто передать его в конструктор [TelegramHandler](/CookBot/source/App/TelegramHandler.cs) и не думать снова о том, как работать с TelegramAPI.

## Общая структура приложения:
Имеем три слоя:

1. [Инфраструктура](/CookBot/source/Infrastructure)
  Содержит всю работу с базами данных. Не взаимодействует с двумя остальными слоями.
Пример использования базы данных в слое приложения [при выполнении команды](/CookBot/source/App/Commands/RecipeByNameCommand.cs):
```C#
      try
      {
           var result = Database
               .GetAnySuitable(x => string.Equals(x.Name, recipeName, StringComparison.CurrentCultureIgnoreCase))
               .GetPrintableView();
           return new BotCommandResult(BotCode.Good, result);
      }
      catch (InvalidOperationException)
      {
           return new BotCommandResult(BotCode.Bad, "К сожалению, ничего подходящего не найдено :(");
      }
    
```
2. [Предметный слой](/CookBot/source/Domain)
  В нем описана модель рецепта.
Главный класс - [класс рецепта](/CookBot/source/Domain/Model/Recipe.cs)
Он использует несколько вспомогательных классов, например, класс, отвечающий за количество и единицу измерения ингредиента и сам ингредиент. Все они расположены [тут](/CookBot/source/Domain/Model)

3. [Слой приложения](/CookBot/source/App)
  Содержит непосредственно работу с Telegram API и команды бота. Не используется двумя предыдущими слоями, но использует их. Демонстрация работы с БД была выше. Работа с предметным слоем происходит при обработке команд (Получили рецепт и попросили у него отдаваемый пользователю вид):
```C#
public BotCommandResult Execute(string[] arguments)
        {
            var suitableRecipes = Database.GetAllSuitable(x => arguments
                        .All(z => x.Components.Keys.Select(y => y.Name.ToLower()).Contains(z.ToLower())));

            if (!suitableRecipes.Any())
                return new BotCommandResult(BotCode.Bad, "К сожалению, ничего подходящего не найдено: (");

            return new BotCommandResult(BotCode.Good,
                string.Join("\n", suitableRecipes.Select(res => res.GetPrintableView())));
        }
```
Один из наиболее важных классов слоя приложения - класс [TelegramHandler.cs](/CookBot/source/App/TelegramHandler.cs)
Содержит всю работу с Телеграм АПИ и использует интерфейс [IBot](/CookBot/source/App/IBot.cs)

### Использование DI-контейнера.
Для создания DI-контейнера была выбрала библиотека Ninject. Итоговая сборка осуществляется в классе [Program.cs](/CookBot/source/Program.cs)
Наиболее неочевидные моменты:
1. Циклическая зависимость. Команда хелп должна знать обо всех зарегистрированных командах.
```C#
      container.Bind<Lazy<List<IBotCommand>>>().ToConstant(new Lazy<List<IBotCommand>>(() => container
                                                                           .GetAll<IBotCommand>()
                                                                           .ToList()))
```
Для этого нам приходится воспользоваться классом ```Lazy<T>``` в классе [команды Help](/CookBot/source/App/Commands/HelpCommand.cs):
```C#
  private readonly Lazy<List<IBotCommand>> commands;
```

2. Cигнлтоны
```C#
            container.Bind<IBotCommand>().To<RecipeByNameCommand>().InSingletonScope();
            container.Bind<IBotCommand>().To<RecipeByIngredientsCommand>().InSingletonScope();
            container.Bind<IBotCommand>().To<RecipeListCommand>().InSingletonScope();
```
Данные класс сделаны сигнлтонами для того, чтобы не создавалось несколько их экземпляров (при создании команды [Help](/CookBot/source/App/Commands/HelpCommand.cs) ей необходимы и другие зарегистрированные команды, экземпляры которых DI-контейнер создаст еще раз)

