# vodRatings

**1. Założenia**

-aplikacja zbiera filmy z platformy NC+ VOD z wybranych gatunków (thriller, akcja, komedia).

-aplikacja dociąga dane z filmweb w celu pobrania np. oceny, ilości ocen, oraz gatunku wg filmweb ponieważ zdarzają się rozbieżności w stosunku do VOD

-wyniki sortowane są wg. oceny z filmweb

-jeśli film nie występuje w filmweb zostaje pominięty w wynikach

-jeżeli dane mają ponad 24h zostają odświeżane w tle, użytkownik zostaje poinformowany gdy dostępne są nowe wyniki

-najnowsze pozycje wyróżnione zostają znacznikiem

-możliwośc filtrowania wyników

-aplikacja w pełni responsywna


**2. Technologie wykorzystane w projekcie**

-ASP.NET Core

-SignalR

-Entity Framework

-Angular 8

-HTML/CSS + Bulma


Backend aplikacji napisany został w języku C# przy wykorzysaniu frameworku ASP.NET Core WebApi. Dostęp do bazy danych zapewnia ORM Entity Framework. 

Wykorzystano SignalR do informowania frontendu o dostępności nowych danych. Część frontendowa wykonana została przy wykorzystaniu frameworku Angular 8, widoki HTML/CSS zostały wsparte frameworkiem Bulma.
