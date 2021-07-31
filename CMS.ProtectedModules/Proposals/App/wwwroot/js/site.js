//// Write your JavaScript code.
//// When you get an updated quote, come here, and set the Quote ID to the text field.
//function SendAgroQuoteToClient (updatedQuoteMessageBag) {
//	document.getElementById("QuoteId").value = updatedQuoteMessageBag.Message.QuoteId;
//	alert('ID for the new Quote was received.');
//}

//// Run this JS code when the body loads completely.
//$(document).ready(function () {
//	// Get a handle to the Signal-R hub.
//	var agroQuoteOutputQueueMonitorHub = $.connection.agroQuoteOutputQueueMonitorHub;
//	$.connection.hub.url = "./signalr";

//	agroQuoteOutputQueueMonitorHub.client.sendAgroQuoteToClient = function (updatedQuoteMessageBag) {
//		$("#Message_QuoteId").val(updatedQuoteMessageBag.Message.QuoteId);
//		alert('ID for the new Quote was received = ' + updatedQuoteMessageBag.Message.QuoteId);
//	}

//	// Start the hub.
//	$.connection.hub.start(function () {
//		// Get the connection ID of this web session from the Signal-R hub.
//		agroQuoteOutputQueueMonitorHub.server.GetPaymentInfoQueueMonitorHubClientId()
//			// Did it succeed?
//			.then(function (contextConnId) {
//				// Yes. Set the hidden field with value of Signal-R hub connection ID.
//				$("#ContextConnectionId").val(contextConnId);
//			});
//	});
//});