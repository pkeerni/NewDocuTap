 
$(document).ready(function(){

    $("button").click(function(){
        $("p").slideToggle();
    });
});

$("#btnSubmit").live("click", function () { 
    if (($('#ddlSite').val() == "") || $('#txtSiteid').val() == "") {
        $("#spanfile").html("Select Site ID.");
        return false;
    }
    else if ($('#fileUpload').val() == "") {
        $("#spanfile").html("Please Select file");
        return false;
    }
    else {
        return checkfile();
    }
});

function getNameFromPath(strFilepath) {
    var objRE = new RegExp(/([^\/\\]+)$/);
    var strName = objRE.exec(strFilepath);
    if (strName == null) {
        return null;
    }
    else {
        return strName[0];
    }
}

function checkfile() {
    var file = getNameFromPath($("#fileUpload").val());
    if (file != null) {
        var extension = file.substr((file.lastIndexOf('.') + 1));
        switch (extension) {
            case 'xls':
            case 'xlt':
            case 'xlsx':
            case 'xlsm':
            case 'xlsb':
            case 'xltx':
            case 'xltm':
            case 'xlam':
                flag = true;
                break;
            default:
                flag = false;
        }
    }
    if (flag == false) {
        $("#spanfile").text("You can upload only excel file");
        return false;
    }
    else {
        $("#spanfile").text("");
    }

}

$(function () {
    $("#fileUpload").change(function () {
        checkfile();
    });
});
 

function buildSiteDropDown(options, defaultValue) {
    //    var $select = $('<select></select>');
    var $option;
    var siteId = document.getElementById("ddlSiteId");
    alert("Before For");
    for (var val in options) {
        $option = $('<option value="' + val + '">' + options[val] + '</option>');
        /*     if (defaultValue == default) {
                 $option.attr('selected', 'selected');
    }*/
        siteId.add($option);
    }
    alert("After For");
    return $select;
}