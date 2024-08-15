(function () {

    $.namespace('planActividades.outsourcing');

    outsourcing = function () {

        function init() {
            initCbo();

            getOrganigrama();
        }

        function initCbo() {
            
        }

        function getOrganigrama() {
            $.ajax({
                url: '/MAZDA/PlanActividades/GetOrganigramaPersonal',
                datatype: "json",
                type: "POST",
                data: {},
                success: function (response) {
                    var data = response.data;

                    fillOrganigrama(data);
                }
            });
        }

        function fillOrganigrama(data) {
            for (i = 0; i < data.length; i++) {

            }
        }

        function makeUL(array) {
            // Create the list element:
            var list = document.createElement('ul');

            for (var i = 0; i < array.length; i++) {
                // Create the list item:
                var item = document.createElement('li');

                // Set its contents:
                item.appendChild(document.createTextNode(array[i]));

                // Add it to the list:
                list.appendChild(item);
            }

            // Finally, return the constructed list:
            return list;
        }

        init();
    };

    $(document).ready(function () {
        planActividades.outsourcing = new outsourcing();
    });

})();