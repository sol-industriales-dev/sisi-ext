function getName(obj, selector) {
    $.ajax({
        url: '/catobra/getNameCC',
        type: 'POST',
        data: { obj: obj },
        success: function (response) {
            if (response.success != 'False') {
                if (response.descripcionCC != '') {
                    selector.val(response.descripcionCC);
                }
                else {
                    selector.val();
                }
            }
            else if (response.message != '') {
                AlertaGeneral('Error', response.message);
            }
        },
        error: function (response) {
        }
    });
};

