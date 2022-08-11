# StateMachine-Library
Эта программа позволяет сериализовать XML'образный файл как список состояний, а затем использовать его как обьект.

	Обьект - State на выходе сериализации хранит в себе схему с графами.
Иерархия в синтаксисе отвечает за начальное распределение состояний, 
а теги позволяют настроить более гибко все состояния.
Иерархия по сути создаёт рёбра между родительскими и дочерними состояниями.

	В данном синтаксисе есть 3 тега: i, o, d.
Данные ключи позволяют сделать более гибкой машину состояния:

	i - input, позволяет создать тег, по которому другие состояния смогут переходить игнорируя выше перечисленные правила.

	o - output, тег, который позволяет перейти на состояние с таким же тегом input.

	d - double, позволяет создать тег для доступа с обеих сторон.
Все состояния с одинаковым тегом смогут переходить друг к другу.

	По суте теги позволяют создать рёбра между обьектами,
input <- output и double <-> double
	Теги можно прописывать через запятую, в одном состоянии могут находится несколько тегов, 
	а так же теги разных ключей

	Пример машины состояния -> \testState.xml

	Примеры использования:
 - конвертация XML файла в SMF для дальнейшего использования:
	smf.converter fileConverter = new smf.converter();
	fileConverter.Init("директория куда сохранить файл", "директория файла XML");
	fileConverter.serialize(); // создаёт файл с таким же названием как 
 - десериализация файла в обьект состояния:
	smf.reader fileReader = new smf.reader();
	fileReader.Init("директория SMF файла");
	smf.state myState = fileReader.deserialize(); // возвращает обьект с начальный состоянием, 
в случае с примером возвращает состояние Idle.

	Полная реализации данной библиотеки есть только для Unity, 
но при желании её можно легко переписать для доступа как к обычному обьекту в c#.

	Формат записи состояний выглядит так -> 
Idle
Idle.Go
Idle.Go.Walk
и т.д.

	В реализации Unity (namespace smf.Unity) существуют 3 обьекта 
 - smf.Unity.StateMachine 
	Этот обьект крепится на игровой обьект в Unity и при запуске инициализируется.
 - smf.Unity.StateMachineLink
	Данный обьект инициализируется в любое время и прикрепляется к одной машине состояния,
	этот обьект несёт 3 задачи:

	- bool Move(string link)
		Попытка перехода состояния, возвращает удачная ли.
	- bool Cheak(string link)
		Возвращает равна ли данное состояние и состояние в машине состояния.
	- void AddListener(UnityAction<string> listener)
		Добавляет слушателя к ивенту машины состояния, 
		данный ивет выполняется при каждой смене состояния.

	Всю библиотеку написал Коноплев Арсений.
	Выход библиотеки в 2022.11.08.