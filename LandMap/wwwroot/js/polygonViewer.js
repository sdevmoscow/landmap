ymaps.ready(init);

var myMap;

function init() {

    if (myMap === undefined) {

        myMap = new ymaps.Map("map", {
            center: [55.73, 37.75],
            zoom: 10
        },
        {
            searchControlProvider: 'yandex#search'
        });

    }

    //myMap.geoObjects.removeAll();

    coords.forEach(function (coord) {

        /*myLabel = new ymaps.GeoObject({
            // Описание геометрии.
            geometry: {
                type: "Point",
                coordinates: getCenter(coord)
            },
            // Свойства.
            properties: {
                // Контент метки.
                iconContent: '123',
                //hintContent: 'Ну давай уже тащи'
                iconCaption: '123'
            }
        }, {
                // Опции.
                // Иконка метки будет растягиваться под размер ее содержимого.
                preset: 'islands#blackStretchyIcon',
                // Метку можно перемещать.
                draggable: true
            });*/

        // Создаём макет содержимого.
        MyIconContentLayout = ymaps.templateLayoutFactory.createClass(
            '<div style="color: #0000FF; font-weight: bold;">$[properties.iconContent]</div>'
        ),
        myPlacemarkWithContent = new ymaps.Placemark(getCenter(coord.Coords), {
            hintContent: coord.Name,
            balloonContent: coord.Name,
            iconContent: coord.Id
        }, {
                // Опции.
                // Необходимо указать данный тип макета.
                iconLayout: 'default#imageWithContent',
                // Своё изображение иконки метки.
                iconImageHref: 'images/ball.png',
                // Размеры метки.
                iconImageSize: [36, 36],
                // Смещение левого верхнего угла иконки относительно
                // её "ножки" (точки привязки).
                iconImageOffset: [-14, -14],
                // Смещение слоя с содержимым относительно слоя с картинкой.
                iconContentOffset: [11, 11],
                // Макет содержимого.
                iconContentLayout: MyIconContentLayout
            });
    // Создаем многоугольник, используя класс GeoObject.
    var myGeoObject = new ymaps.GeoObject({
        // Описываем геометрию геообъекта.
        geometry: {
            // Тип геометрии - "Многоугольник".
            type: "Polygon",
            // Указываем координаты вершин многоугольника.
            coordinates: coord.Coords,
            // Задаем правило заливки внутренних контуров по алгоритму "nonZero".
            fillRule: "nonZero"
        },
        // Описываем свойства геообъекта.
        properties: {
            // Содержимое балуна.
            hintContent: coord.Name,
            balloonContent: coord.Name,
            balloonContentBody: coord.Name,
            iconCaption: coord.Id
        }
    }, {
            // Описываем опции геообъекта.
            // Цвет заливки.
            fillColor: '#00FF00',
            // Цвет обводки.
            strokeColor: '#0000FF',
            // Общая прозрачность (как для заливки, так и для обводки).
            opacity: 1.0,
            // Ширина обводки.
            strokeWidth: 5
            // Стиль обводки.
            //strokeStyle: 'shortdash'
        });

    myGeoObject.events.add(['click'], function (e) {
        var coordinates = myGeoObject.geometry.getCoordinates();

        var pos = getCenter(coordinates); 

        myMap.setCenter(pos);

        if (!e.get('target').editor.state.get("drawing")) {
            $('#polyCoords').val(JSON.stringify(myGeoObject.geometry.getCoordinates())); 

            myGeoObject.options.set({
                strokeWidth: 7,
                strokeColor: '#1d74cf',
                fillColor: '#FF0000'
            });
        }
    });


    // Добавляем многоугольник на карту.
        myMap.geoObjects.add(myGeoObject).add(myPlacemarkWithContent);
    });

}

function drawPoly() {

    var max_vertexes = parseInt($('#max_vertexes').val()) + 1;

    // Создаем многоугольник без вершин.
    var myPolygon = new ymaps.Polygon([], {}, {
        // Курсор в режиме добавления новых вершин.
        editorDrawingCursor: "crosshair",
        // Максимально допустимое количество вершин.
        editorMaxPoints: max_vertexes,
        // Цвет заливки.
        fillColor: '#00FF00',   
        // Цвет обводки.
        strokeColor: '#0000FF',
        // Ширина обводки.
        strokeWidth: 5
    });

    if ($('#hide_polygons').is(':checked')) {
        showObjects(false);
    }

    myPolygon.events.add(['editorstatechange'], function (e) {

        var coordinate = myPolygon.geometry.getCoordinates();
        if (!e.get('target').editor.state.get("drawing")) {

            $('#polyCoords').val(JSON.stringify(myPolygon.geometry.getCoordinates())); 
            $('#landId').css('display', 'none');
            $('#editor_id').val(0);
            $('#editor_name').val('');
            $('#editor_cadastralnumber').val('');
            $('#editor_inventorynumber').val('');
            $('#editor_landtype_id').val(1);
            $('#editor_landrighttype_id').val(1);
            $('#submitbutton').text('Сохранить');
            $('#landModal').modal('show');
            showObjects(true);
        }

    });

    myPolygon.events.add(['mouseenter'], function (e) {

        var coordinate = myPolygon.geometry.getCoordinates();
        if (!e.get('target').editor.state.get("drawing")) {
            $('#polyCoords').val(JSON.stringify(myPolygon.geometry.getCoordinates()));
            showObjects(true);
        }
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
}

function showObjects(visible) {

    myMap.geoObjects.each(function (e) {
        e.options.set('visible', visible);
    });
}

function getCenter(coordinates) {

    if (coordinates.length === 0 || coordinates[0].length === 0) return [0, 0];

    let [minX, maxX, minY, maxY] = [coordinates[0][0][0], coordinates[0][0][0], coordinates[0][0][1], coordinates[0][0][1]];

    coordinates[0].forEach(function (c) {
        //console.log(c);
        if (c[0] < minX) minX = c[0];
        if (c[0] > maxX) maxX = c[0];
        if (c[1] < minY) minY = c[1];
        if (c[1] > maxY) maxY = c[1];
    });

    return [(minX + maxX) / 2, (minY + maxY) / 2];
}

function getAverageCenter(coordinates) {

    if (coordinates.length === 0 || coordinates[0].length === 0) return [0, 0];

    let [x, y, i] = [0, 0, 0];

    coordinates[0].forEach(function (c) {
        if ((++i) < coordinates[0].length) {
            x += c[0];
            y += c[1];
        }
    });

    return [x / (coordinates[0].length-1), y / (coordinates[0].length-1)];
}

