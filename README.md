# CoordinateTestApp - приложение для добавления точек на координатную плоскость  
Релизованные возможности:
- Ввод координат точек (заданные лимиты плоскости - [-10;10])
- Сохранение введенных точек в кеш приложения при закрытии пакетно - по 5 точек в файле (файлы кеша создаются в папке %USERPROFILE%/AppData/Roaming/CoordinateTestApp/cache)
- Чтение сохраненных в кеше точек (при загрузке приложения)
- Удаление файлов кеша (по кнопке "удалить все")
- Добавление точек в список точек по кнопке "Добавить"
- Отображение списка точек в текущем графическом окне во кладке "список точек"  
# Был реализован пользовательский элемент управления - CartesianCoordinateSystemControl - координатная плоскость, имеющий следующие возможности:
  1. Отображение добавленных точек
  2. Изменение размеров координатной плоскости
  3. Отображение координаты точки при наведении курсора
  4. Отображение текущей координаты курсора мыши  
# Свойства элемента управления, доступные для изменения:
- PointRadius - радиус отображаемых точек
- LabelForeground - цвет значений на осях
- LabelFontSize - размер значений осей
- PointBackground - цвет точки
- XRelativeStep - размер шага по оси Х
- YRelativeStep - размер шага по оси Y
- AxisLength - относительный размер осей
- Points - для установки привязки к списку точек
- IsDrawnHelperXDivisions - установка отрисовки сетки по X
- IsDrawnHelperYDivisions - установка отрисовки сетки по Y
- XDivisionColor - цвет делений оси X
- YDivisionColor - цвет делений оси Y
- HelperXDivisionColor - цвет линий сетки по X
- HelperYDivisionColor - цвет линий сетки по Y  
# Внешний вид приложения:
- Главный экран
![image](https://github.com/Ksenia-gra/CoordinateTestApp/assets/58133251/f40de812-62c0-4b4f-9a40-d11cf76aa7da)
- Вкладка со списком точек
![image](https://github.com/Ksenia-gra/CoordinateTestApp/assets/58133251/bef5e3ed-60cc-4221-a29c-5a4543ddba37)
- Отображение координаты точки при наведении курсора
![image](https://github.com/Ksenia-gra/CoordinateTestApp/assets/58133251/e358afb2-bc56-43ec-a23b-5da30cb88961)
# Использованные библиотеки
- MaterialDesignThemes
- Microsoft.Extensions.DependencyInjection
- Microsoft.Xaml.Behaviors.Wpf
- Newtonsoft.Json
- PropertyChanged.Fody
