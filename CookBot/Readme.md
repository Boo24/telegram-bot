Пример из кода::::
```C#
        public string Execute(IDatabase<Recipe> db, params string[] arguments)
        {
            var result = "Привет! Меня зовут Cook Bot :D. У меня самые лучшие рецепты. Вот список команд, которые я могу выполнить: \n";
            result += string.Join("\n", commands.Value.Select(command => $"{command.Name} - {command.Description}."));
            return result;
        }
```
