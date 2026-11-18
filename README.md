WebShell

- После ввода команды на веб-странице сервер выполняет ее и показывает ответ. История текущего сеанса отображается в области над строкой ввода команды. История команд храниться в базе данных. Листать историю команд можно клавишами "вверх" и "вниз"

- Безопасность
  Приложение выполняет любые команды на компьютере, где оно запущено. Запускай его только на своём компьютере. Не открывай его для   других людей и не выкладывай в интернет.

- Технологии
  * C#, ASP.NET Core Web API (.NET 10)
  * Entity Framework Core, SQL Server (в Docker)
  * HTML, CSS, JavaScript ('fetch'), без фреймворков

- Требования
  * .NET 10 SDK
  * Docker Desktop
  * macOS, Linux или Windows. На macOS и Linux команды запускаются через '/bin/bash', на Windows через 'cmd.exe'.
  * На Mac с чипом Apple Silicon включи в Docker Desktop эмуляцию Rosetta. SQL Server работает через неё.

- Запуск (БД и таблицы создаются автоматически при первом запуске)
  I. Запуск SQL Server в контейнере:
     docker run -e 'ACCEPT_EULA=Y' -e 'MSSQL_SA_PASSWORD=Str0ngPassw0rd123' -p 127.0.0.1:1433:1433 --name webshell-sql --platform linux/amd64 -d       mcr.microsoft.com/mssql/server:2022-latest
     (Пароль относится только к локальной разработке. Он совпадает со строкой подключения в 'WebShell.Api/appsettings.json'.)
  II. Запуск приложения на конфигурации 'WebShell.Api: http' (url: http://localhost:5032).
   
