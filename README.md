# Консольная версия игры "Змейка"

Предоставляет консольную версию знаменитой игры "Змейка"

![image](https://github.com/user-attachments/assets/46f0428c-ff7c-438b-9c13-3878fd7a7a90)

Реализована с помощью языка программирования C#

## Запуск приложения

Для запуска приложения необходимо перейти в папку с проектом и выполнить следкющую команду: `.\SnakeConsoleGame\bin\Release\SnakeConsoleGame.exe {ширина поля} {высота поля} {скорость змейки}`

В качестве параметров указываются ширина поля, высота поля и скорость змейки. Чем меньше указанная скорость, тем быстрее будет двигаться змейка.

Можно также запускать приложение и без параметров, тогда будут применены параметры по умолчанию: `ширина: 20`, `высота: 15`, `скорость: 7`.

## Игровой процесс

Для управления используются клавиши на клавиатуре: `Up`, `Down`, `Left`, `Right`. После завершения игры можно нажать клавишу `R` для перезапуска игры или клавишу `Esc` для выхода из приложения.
