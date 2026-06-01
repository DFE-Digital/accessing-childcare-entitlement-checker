function sendingRequest(msg, initiator, helper) {
    var authHeader = java.lang.System.getenv("AUTH_HEADER");
    if (authHeader) {
        msg.getRequestHeader().setHeader("Authorization", authHeader);
    }
    
    // Set a custom User-Agent to identify ZAP automation traffic
    msg.getRequestHeader().setHeader("User-Agent", "OWASP-ZAP-Automation");
}

function responseReceived(msg, initiator, helper) {
    // No action needed on response
}