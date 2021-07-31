//// Write your JavaScript code.
//$("a.iframeLink").click(function () {
//	var bearerTokenValue = "bearer " + $("#uniqueCustomerId").val();
//	bearerTokenValue = bearerTokenValue.substring(0, bearerTokenValue.length - 1);
//	alert(bearerTokenValue);

//	var link = $(this).attr("href");
//    //console.log(link);
//    //$("#lobModuleIFrame").attr("src", link);
//	// return false;

//    $.ajax({
//        method: 'GET',
//		url: link,
//		// contentType: "application/json",
//		beforeSend: function (xhr, settings) {
//			xhr.setRequestHeader("Authorization", bearerTokenValue);
//		},
//		success: function (data) {
//			console.log(data);
//			alert("Success! HTML = " + data);
//			//$("#iframeData").html(data);
//			//$("#lobModuleIFrame").attr('src', "/");
//			//$("#lobModuleIFrame").contents().find('html').html(data); 
//			document.getElementById('lobModuleIFrame').contentWindow.document.write(data);
//			//var iframe = document.getElementById('lobModuleIFrame');
//			//iframe.contentWindow.contents = data;
//			//iframe.src = 'javascript:window["contents"]';
//			//step 2: obtain the document associated with the iframe tag
//			//most of the browser supports .document. Some supports (such as the NetScape series) .contentDocumet, while some (e.g. IE5/6) supports .contentWindow.document
//			//we try to read whatever that exists.
			
//		},
//		error: function (data) {
//			console.log(data);
//		}
//	});

//	return false;
//});
