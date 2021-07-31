// Write your JavaScript code.
$("a.iframeLink").click(function () {
    var link = $(this).attr("href");
    console.log(link);
    $("#lobModuleIFrame").attr("src", link);
    return false;

});
