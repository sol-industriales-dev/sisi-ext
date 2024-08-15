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

            // populate data into keywordArr for search results
            SearchSummary();

        }
        else {
            ShowMessage("Nothing found!");
        }
    }
}

var keywordArr = null;
var counter = -1;
var keywordsFound = -1;
var keyWordsCounter = -1;
var currentPage = -1;
var lastKeywordDiv = null;
var lastDivTop = 0;

function SearchSummary() {

    keywordArr = objctlDoc.SearchSummary(false);

    ResetCounter();
}

function NewSearch() {

    objctlDoc.NewSearch();

    $("#kw").val('').focus();

    ShowMessage("");

    keywordArr = null;

    ResetCounter();
}

function ResetCounter() {
    counter = -1;
    keywordsFound = -1;
    currentPage = -1;

    if (null !== lastKeywordDiv) {
        lastKeywordDiv.css("background-color", "lime");
        lastKeywordDiv = null;
    }

    keyWordsCounter = -1;
    lastDivTop = 0;

    // first default call
    setTimeout(function () { DoKeywordFocus(); }, 1000);
}

function DoKeywordFocus() {

    if (null != keywordArr) {

        if (keywordsFound > 0) {

            keywordsFound--;
            keyWordsCounter++;

            if (null !== lastKeywordDiv) {
                lastKeywordDiv.css("background-color", "lime");
            }

            var searchDiv = $("#" + "div_Search_" + currentPage);
            var divPreview = $("#div_ctlDoc_divPreview"); 

            var focusKeywordDiv = searchDiv.children().eq(keyWordsCounter);           

            var divDiff = parseInt(focusKeywordDiv.offset().top) - lastDivTop;

            if (keyWordsCounter == 0) {
                divDiff = parseInt(focusKeywordDiv.position().top) - lastDivTop;
            }

            if (divDiff > 80) { // adjust padding as required
                divDiff = divDiff - 80; 
            }

            var scrollTill = parseInt(divPreview.scrollTop()) + divDiff;

            divPreview.animate({
                scrollTop: scrollTill
            }, 100);

            setTimeout(function () { focusKeywordDiv = searchDiv.children().eq(keyWordsCounter); focusKeywordDiv.css("background-color", "red"); lastKeywordDiv = focusKeywordDiv; lastDivTop = parseInt(focusKeywordDiv.offset().top); }, 500);
        }
        else {

            counter++;

            if (counter > -1 && counter < keywordArr.length) {

                currentPage = keywordArr[counter][0];
                keywordsFound = keywordArr[counter][1];
                keyWordsCounter = -1;

                lastDivTop = 0;

                objctlDoc.GotoPage(currentPage);

                setTimeout(function () { DoKeywordFocus(); }, 1000);
            }
            else {
                ResetCounter();
            }
        }
    }
}