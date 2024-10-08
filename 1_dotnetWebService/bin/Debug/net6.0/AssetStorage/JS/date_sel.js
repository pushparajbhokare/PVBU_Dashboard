$(document).ready(function () {

    (function ($) {
        $('.tab ul.tabs').addClass('active').find('> li:eq(0)').addClass('current');

        $('.tab ul.tabs li a').click(function (g) {
            var tab = $(this).closest('.tab'),
                index = $(this).closest('li').index();

            tab.find('ul.tabs > li').removeClass('current');
            $(this).closest('li').addClass('current');

            // tab.find('.tab_content').find('div.tabs_item').not('div.tabs_item:eq(' + index + ')').slideUp();
            // tab.find('.tab_content').find('div.tabs_item:eq(' + index + ')').slideDown();

            g.preventDefault();
        });


    })(jQuery);

    $('#FromDate').datepicker({
        onSelect: function (dateText, inst) {

            $('#ToDate').datepicker('option', 'minDate', new Date(dateText));
        }
    });
    $('#ToDate').datepicker({
        onSelect: function (dateText, inst) {
            $('#FromDate').datepicker('option', 'maxDate', new Date(dateText));
        }
    });


    $("li.date-selection").click(function () {
        getFromDate($(this).find('a').html());
    });

   

    getFromDate('1D')

   

});
function getQueryStringValue(key) {
    return decodeURIComponent(window.location.search.replace(new RegExp("^(?:.*[&\\?]" + encodeURIComponent(key).replace(/[\.\+\*]/g, "\\$&") + "(?:\\=([^&]*))?)?.*$", "i"), "$1"));
}
function linkWO(line, oper) {
    console.log('event Trigger')
    var url = "../oper_alarm/index.html?TE_1087=" + line + "&TEARBEITSGANG=" + oper;
    window.location.href = url;

}
function getFromDate(type) {
    $('#FromDate').attr("disabled", "disabled");
    $('#ToDate').attr("disabled", "disabled");

    if (type == '1D') {

        var format1 = moment().format('MM/DD/YYYY');

        $('#FromDate').val(format1);
        $('#ToDate').val(format1);



    } else if (type == '1W') {
        var format1 = moment().subtract(6, 'd').format('MM/DD/YYYY');
        var format2 = moment().format('MM/DD/YYYY');

        $('#FromDate').val(format1);
        $('#ToDate').val(format2);

    } else if (type == '1M') {
        var format1 = moment().subtract(1, 'M').add(1, 'd').format('MM/DD/YYYY');
        var format2 = moment().format('MM/DD/YYYY');

        $('#FromDate').val(format1);
        $('#ToDate').val(format2);

    } else if (type == 'Custom') {
        $("#FromDate").removeAttr("disabled");
        $("#ToDate").removeAttr("disabled");

    }

}

function getDateValue() {
    let FromDate = document.getElementById("FromDate").value;
    let ToDate = document.getElementById("ToDate").value;
    return {FromDate, ToDate };
}

