function ShowMessage(msg) {
    $("#msg").val(msg);
}

function ShowLog(msg) {
    $("#log").val(msg);
}

function SearchKeyword() {

    if (null != objctlDoc && $("#kw").val().length > 0) {
        var count = objctlDoc.Search($("#kw").val(), $("#ext").is(':checked'));

        $("#log").val("");

        if (count > 0) {
            ShowMessage("Found " + count + " matches!");
            $("#btnSum").removeAttr("disabled");
            $("#chkFocus").removeAttr("disabled");

            $("#btnNext").attr("disabled", "disabled");
            $("#btnPrevious").attr("disabled", "disabled");
        }
        else {
            ShowMessage("Nothing found!");
        }
    }
}

var resArr = null;
var counter = -1;


function SearchSummary() {
    resArr = objctlDoc.SearchSummary($("#chkFocus").is(':checked'));
    ResetCounter();

    $("#btnNext").removeAttr("disabled");
    $("#btnPrevious").removeAttr("disabled");

    $("#chkFocus").removeAttr("disabled");
}

function NewSearch() {

    objctlDoc.NewSearch();

    $("#kw").val('').focus();
    $("#btnSum").attr("disabled", "disabled");
    $("#btnPrevious").attr("disabled", "disabled");
    $("#btnNext").attr("disabled", "disabled");
    $("#chkFocus").attr("disabled", "disabled");

    ShowMessage("");

    resArr = null;
    ResetCounter();
}

function ResetCounter() {
    counter = -1;
    ShowLog("");
}

function SearchPage(moveNext) {

    if (null != resArr) {
        if (moveNext) {
            counter++;
        }
        else {
            if (counter > 0) {
                counter--;
            }
        }

        if (counter > -1 && counter < resArr.length) {
            var page = resArr[counter][0];
            var matches = resArr[counter][1];

            ShowLog("Found " + matches + " instances on page " + page);
            objctlDoc.GotoPage(page);
        }
        else {
            ResetCounter();
        }
    }
}