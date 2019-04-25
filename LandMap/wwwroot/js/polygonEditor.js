ymaps.ready(init);

function init() {

        myMap = new ymaps.Map("map", {
        center: [55.73, 37.75],
        zoom: 14
    }, {
            searchControlProvider: 'yandex#search'
        });

    alert('initDrawing');

    // Создаем многоугольник без вершин.
    var myPolygon = new ymaps.Polygon([], {}, {
        // Курсор в режиме добавления новых вершин.
        editorDrawingCursor: "crosshair",
        // Максимально допустимое количество вершин.
        editorMaxPoints: 10,
        // Цвет заливки.
        fillColor: '#00FF00',
        // Цвет обводки.
        strokeColor: '#0000FF',
        // Ширина обводки.
        strokeWidth: 5
    });

     myPolygon.events.add([
        //  'mapchange', 
        //  'geometrychange', 
        //  'pixelgeometrychange', 
        //  'optionschange', 
        //  'propertieschange',
        //  'balloonopen', 
        //  'balloonclose', 
        //  'hintopen', 
        //  'hintclose', 
        //  'dragstart', 
        //  'dragend'
        'editorstatechange'
     ], function (e) {
         //log.innerHTML = '@' + e.get('type') + '<br/>' + log.innerHTML;
     });
    
    // Добавляем многоугольник на карту.
    myMap.geoObjects.add(myPolygon);

    // В режиме добавления новых вершин меняем цвет обводки многоугольника.
    var stateMonitor = new ymaps.Monitor(myPolygon.editor.state);
    stateMonitor.add("drawing", function (newValue) {
        myPolygon.options.set("strokeColor", newValue ? '#FF0000' : '#0000FF');
    });

    // Включаем режим редактирования с возможностью добавления новых вершин.
    myPolygon.editor.startDrawing();
};

