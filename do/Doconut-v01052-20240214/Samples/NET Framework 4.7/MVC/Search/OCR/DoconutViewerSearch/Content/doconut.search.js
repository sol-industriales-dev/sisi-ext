function ShowMessage(msg) {
    $("#msg").val(msg);
}

function ShowLog(msg) {
    $("#log").val(msg);
}

function SearchKeyword() {

    if (searchReady === true) {
        if (null != objctlDoc && $("#kw").val().length > 0) {

            var count = objctlDoc.Search($("#kw").val(), $("#ext").is(':checked'));

            if (count > 0) {
                ShowMessage("Found " + count + " matches!");
                SearchSummary();
            }
            else {
                ShowMessage("Nothing found!");
            }
        }
    }
    else {
        alert('Search is not yet ready, please wait!');
    }
}

var resArr = null;
var counter = -1;


function SearchSummary() {
    resArr = objctlDoc.SearchSummary(true); // true to have a border on page that has a search word

    $("#liNext").show();
    $("#liPrev").show();

    ResetCounter();
}

function NewSearch() {
    objctlDoc.NewSearch();

    $("#kw").val('').focus();

    ShowMessage('');
    ShowLog('');

    resArr = null;

    $("#liNext").hide();
    $("#liPrev").hide();

    ResetCounter();
}

function ResetCounter() {
    counter = -1;
    ShowLog("");
}

function SearchPage(moveNext) {

    if (null != resArr) {

        if (moveNext && counter < (resArr.length - 1)) {
            counter++;
        }

        if (!moveNext && counter > 0) {
            counter--;
        }

        if (counter >= 0 && counter <= resArr.length - 1) {
            var page = resArr[counter][0];
            var matches = resArr[counter][1];

            ShowLog("Found " + matches + " on pg " + page);
            objctlDoc.GotoPage(page);
        }
    }
}