# Справка по командам Git

## Содержание

* [Установка Git](#install-git)
* [Настройка Git](#setting-up-git)
* [Создание репозитория в существующем каталоге](#initializing-a-repository-in-an-existing-directory)
* [Определение состояния файлов](#checking-the-status-of-your-files)
* [Отслеживание новых файлов](#staging-files)
* [Игнорирование файлов](#gitignore)
* [Псевдонимы в Git](#useful-alias) 
* [Прятанье (stash)](#stashing-files)
* [Фиксация изменений (commit)](#committing-files)
* [Удаление файлов](#removing-files)
* [Перемещение файлов](#moving-files)
* [Ветки и слияния (checkout, branch, merge)](#branching-and-merging)
* [Обновление изменений в локальном репозитории из Github репозитория (pull)](#updating-a-local-repository-with-changes-from-a-github-repository)
* [Инструменты Git](#git-tools)
* [Git blame](#git-blame)
* [Git bisect](#git-bisect)
* [Полезное ПО](#useful-software)
* [Git log](#git-log)
* [Пример](#example)


#### Install Git

Нажмите [сюда](http://git-scm.com/download/), чтобы скачать и установить Git.

#### Setting up git

##### [Установка имени и электронной почты](https://git-scm.com/book/ru/v1/%D0%92%D0%B2%D0%B5%D0%B4%D0%B5%D0%BD%D0%B8%D0%B5-%D0%9F%D0%B5%D1%80%D0%B2%D0%BE%D0%BD%D0%B0%D1%87%D0%B0%D0%BB%D1%8C%D0%BD%D0%B0%D1%8F-%D0%BD%D0%B0%D1%81%D1%82%D1%80%D0%BE%D0%B9%D0%BA%D0%B0-Git#Имя-пользователя)

```sh
$ git config --global user.name "Your Name"

$ git config --global user.email "your_email@whatever.com"
```

##### [Параметры установки окончаний строк](https://git-scm.com/book/ru/v1/%D0%9D%D0%B0%D1%81%D1%82%D1%80%D0%BE%D0%B9%D0%BA%D0%B0-Git-%D0%9A%D0%BE%D0%BD%D1%84%D0%B8%D0%B3%D1%83%D1%80%D0%B8%D1%80%D0%BE%D0%B2%D0%B0%D0%BD%D0%B8%D0%B5-Git#Форматирование-и-пробельные-символы)

Для пользователей Unix/Mac:

```sh
$ git config --global core.autocrlf input

$ git config --global core.safecrlf true
```

Для пользователей Windows:
```sh
$ git config --global core.autocrlf true

$ git config --global core.safecrlf true
```

##### [Цвета в Git](https://git-scm.com/book/ru/v1/%D0%9D%D0%B0%D1%81%D1%82%D1%80%D0%BE%D0%B9%D0%BA%D0%B0-Git-%D0%9A%D0%BE%D0%BD%D1%84%D0%B8%D0%B3%D1%83%D1%80%D0%B8%D1%80%D0%BE%D0%B2%D0%B0%D0%BD%D0%B8%D0%B5-Git#Цвета-в-Git)

```sh
$ git config --global color.ui true
```

##### [Проверка настроек](https://git-scm.com/book/ru/v1/%D0%92%D0%B2%D0%B5%D0%B4%D0%B5%D0%BD%D0%B8%D0%B5-%D0%9F%D0%B5%D1%80%D0%B2%D0%BE%D0%BD%D0%B0%D1%87%D0%B0%D0%BB%D1%8C%D0%BD%D0%B0%D1%8F-%D0%BD%D0%B0%D1%81%D1%82%D1%80%D0%BE%D0%B9%D0%BA%D0%B0-Git#Проверка-настроек)

```sh
$ git config --list
user.name=Your Name
user.email=your_email@whatever.com
color.status=auto
color.branch=auto
color.interactive=auto
color.diff=auto
...
```


#### Основы Git

##### Initializing a repository in an existing directory

Если вы собираетесь начать использовать Git для существующего проекта, то вам необходимо перейти в проектный каталог и в командной строке ввести:

```sh
$ git init
```

Эта команда создаёт в текущем каталоге новый подкаталог с именем .git, содержащий все необходимые файлы репозитория — основу Git-репозитория. На этом этапе ваш проект ещё не находится под версионным контролем. Если вы хотите добавить под версионный контроль существующие файлы, вам стоит проиндексировать эти файлы и осуществить первую фиксацию изменений. Осуществить это вы можете с помощью нескольких команд `git add`, указывающих индексируемые файлы, а затем `commit`:

```sh
$ git add *.c
$ git add README
$ git commit -m 'initial project version'
```

##### Checking the status of your files

Используйте команду `git status`, чтобы проверить текущее состояния файлов.

```sh
$ git status
# On branch master
nothing to commit, working directory clean

# Сокращенный вывод статуса
$ git status -s
 M README
MM Rakefile
A  lib/git.rb
M  lib/simplegit.rb
?? LICENSE.txt
```
##### Staging files

После инициализации Git-репозитория в выбранную директорию, все файлы должны отслеживаться. Любые изменения, сделанные в любом файле, будут показаны после `git status`, как неиндексированные изменения.

Чтобы отслеживать новые файлы, нужно использовать команду `git add`.

```sh
# Добавить файл
$ git add filename

# Добавить все файлы
$ git add -A

# Добавить все измененные файлы в директорию
$ git add .

# Выбрать какие изменения нужно добавить
$ git add -p
```

##### gitignore

Чтобы не видеть файлы, которые вы не хотите добавлять в Git-репозиторий, в списках неотслеживаемых, нужно создать файл .gitignore. Набор полезных .gitignore шаблонов можно посмотреть [тут](https://github.com/github/gitignore).

##### Useful Alias

```sh
$ git config --global alias.co checkout
$ git config --global alias.br branch
$ git config --global alias.ci commit
$ git config --global alias.st status
```

#### Stashing files

Часто возникает такая ситуация, что пока вы работаете над частью своего проекта, всё находится в беспорядочном состоянии, а вам нужно переключить ветки, чтобы немного поработать над чем-то другим. Проблема в том, что вы не хотите фиксировать изменения с наполовину сделанной работой только для того, чтобы позже можно было вернуться в это же состояние. Ответ на эту проблему — команда `git stash`.

```sh
# Спрятать локальные изменения в тайник
$ git stash

# Спрятать локальные изменения с сообщением в тайник
$ git stash save "this is your custom message"

# Применить последние спрятанные в тайнике данные
$ git stash apply

# Применить последние спрятанные в тайнике данные под номером stash_number
$ git stash apply stash@{stash_number}

# Удалить данные из стека, спрятанные в тайнике
$ git stash drop stash@{0}

# Применить последние спрятанные в тайнике данные и удалить их из стека
$ git stash pop

# Применить последние спрятанные в тайнике данные и удалить их из стека под номером stash_number 
$ git stash pop stash@{stash_number}

# Показать содержимое тайника
$ git stash list

# Показать последние спрятанные изменения
$ git stash show

# Показать изменения, находящиеся в тайнике под номером stash_number
$ git diff stash@{0}
```

##### Committing files

После индексирования или прятанья файлов следующим шагом фиксируются изменения.
Простейший способ зафиксировать изменения - это использовать команду `git commit` 

```sh
# Зафиксировать изменения файлов
$ git commit -m 'commit message'

# Зафиксировать изменение файла
$ git commit filename -m 'commit message'

# Игнорировать индексацию и зафиксировать изменение
$ git commit -a -m 'insert commit message'

# Изменение последнего коммита
$ git commit --amend 'new commit message' or no message to maintain previous message
```

##### Removing files

```sh
# Удаление файла
$ git rm filename

# Удалить файл из индекса
git rm --cached filename

# Удалить все файлы, чьи имена заканчиваются на ~.
git rm \*~
```

##### Moving files

Git не отслеживает перемещение файлов явно. Для этого нужно использовать команду `git mv`

```sh
$ git mv file_from file_to
```

#### Branching and merging

```sh
# Создание локальной ветки
$ git checkout -b branchname

# Переключение между двумя ветками
$ git checkout branchname

# Отправить ветку branchname на сервер origin
# git push [удал. сервер] [ветка]
$ git push origin branchname

# Удаление локальной ветки после слияния
$ git branch -d branchname

# Удаление локальной ветки (тупиковая ветка)
$ git branch -D branchname

# Удалить все ветки, которых нет во внешнем репозитории, можно командой
$ git remote prune origin

# Просмотреть все существующие ветки  
$ git branch -a

# Просмотреть все ветки, которые участвовали в процессе слияния с текущей веткой 
$ git branch -a --merged

# Просмотреть все ветки, которые не использовались в процессе слияния с текущей веткой
$ git branch -a --no-merged

# Просмотреть локальные ветки
$ git branch

# Просмотреть все удаленные ветви
$ git branch -r

# Слияние локальной ветки с веткой master 
$ git rebase origin/master

# Отправить локальную ветку после слияния с веткой master 
$ git push origin branchname

# Запустить соответствующий графический инструмент и показать конфликтные ситуации
$ git mergetool

# Слияние текущей ветки с веткой master
$ git merge branchname

# Отменить слияние
$ git merge --abort
```

#### Updating a local repository with changes from a Github repository

```sh
$ git pull origin master

# Команда заберет новую ветку master и переместит локальные изменения вашего коллеги на ее вершину.
$ git pull --rebase origin master
```

#### Git Tools

##### Git blame

Если вы отловили ошибку в коде и хотите узнать, когда и по какой причине она была внесена, то аннотация файла - лучший инструмент для этого случая.

```sh
# Посмотреть, кем в последний раз правилась каждая строка файла
$ git blame [filename]
```

##### Git bisect

Аннотирование файла помогает, когда вы знаете, где у вас ошибка, и есть с чего начинать. Если вы не знаете, что у вас сломалось, и с тех пор, когда код работал, были сделаны десятки или сотни коммитов, вы наверняка обратитесь за помощью к `git bisect`. Команда `bisect` выполняет бинарный поиск по истории коммитов и призвана помочь, как можно быстрее определить, в каком коммите была внесена ошибка.

Положим, вы только что отправили новую версию вашего кода в производство, и теперь вы периодически получаете отчёты о какой-то ошибке, которая не проявлялась, пока вы работали над кодом, и вы не представляете, почему код ведёт себя так. Вы возвращаетесь к своему коду, и у вас получается воспроизвести ошибку, но вы не понимаете, что не так. Вы можете использовать `bisect`, чтобы выяснить это. Сначала выполните `git bisect start`, чтобы запустить процесс, а затем `git bisect bad`, чтобы сказать системе, что текущий коммит, на котором вы сейчас находитесь, сломан. Затем необходимо сказать bisect, когда было последнее известное хорошее состояние с помощью `git bisect good [хороший_коммит]`:

```sh
$ git bisect start
$ git bisect bad
$ git bisect good v1.0
Bisecting: 6 revisions left to test after this
[ecb6e1bc347ccecc5f9350d878ce677feb13d3b2] error handling on repo
```

##### Useful software

* [TortoiseGit](https://tortoisegit.org/)
* [SourceTree](https://www.sourcetreeapp.com/)
* [Git Client SmartGit](http://www.syntevo.com/smartgit/)
* [GUI Clients](https://git-scm.com/download/gui/windows)



#### Git log

```sh
# Показать список всех фиксированных изменений, всю информацию о фиксированных изменениях
$ git log

# Список фиксированных изменений с конкретными сообщениями и изменениями
$ git log -p

# Список фиксированных изменений, сделанных конкретным автором
$ git log --author 'Author Name'

# Показать список фиксированных изменений за определенный промежуток времени
$ git log --since=yesterday
```

#### Example

```sh
$ git clone https://github.com/NataliaDSmirnova/CGAdvanced2017
$ git add filename
$ git commit -m 'Add some feature'
$ git status
$ git pull --rebase
$ git status
$ git push -u origin
```

#### Источники

https://git-scm.com/

https://githowto.com/ru

https://github.com/bpassos/git-commands

https://habrahabr.ru/post/161009/

https://noteskeeper.ru/621/

http://eax.me/git-commands/

http://stepansuvorov.com/blog/2012/11/git-stash-%D1%8D%D1%82%D0%BE-%D1%82%D0%BE-%D1%87%D1%82%D0%BE-%D1%8F-%D0%B8%D1%81%D0%BA%D0%B0%D0%BB/

http://najomi.org/git

https://marklodato.github.io/visual-git-guide/index-ru.html