# ZAP by Checkmarx Scanning Report

ZAP by [Checkmarx](https://checkmarx.com/).


## Summary of Alerts

| Risk Level | Number of Alerts |
| --- | --- |
| High | 0 |
| Medium | 2 |
| Low | 5 |
| Informational | 4 |




## Insights

| Level | Reason | Site | Description | Statistic |
| --- | --- | --- | --- | --- |
| Info | Informational |  | Percentage of network failures | 1 % |
| Info | Informational | https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net | Percentage of responses with status code 2xx | 82 % |
| Info | Informational | https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net | Percentage of responses with status code 3xx | 1 % |
| Info | Informational | https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net | Percentage of responses with status code 4xx | 15 % |
| Info | Informational | https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net | Percentage of endpoints with content type application/json | 5 % |
| Info | Informational | https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net | Percentage of endpoints with content type image/png | 5 % |
| Info | Informational | https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net | Percentage of endpoints with content type image/svg+xml | 11 % |
| Info | Informational | https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net | Percentage of endpoints with content type image/x-icon | 5 % |
| Info | Informational | https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net | Percentage of endpoints with content type text/css | 5 % |
| Info | Informational | https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net | Percentage of endpoints with content type text/html | 41 % |
| Info | Informational | https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net | Percentage of endpoints with content type text/javascript | 5 % |
| Info | Informational | https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net | Percentage of endpoints with content type text/plain | 5 % |
| Info | Informational | https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net | Percentage of endpoints with method GET | 82 % |
| Info | Informational | https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net | Percentage of endpoints with method POST | 17 % |
| Info | Informational | https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net | Count of total endpoints | 17    |
| Info | Informational | https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net | Percentage of slow responses | 64 % |




## Alerts

| Name | Risk Level | Number of Instances |
| --- | --- | --- |
| Content Security Policy (CSP) Header Not Set | Medium | Systemic |
| Missing Anti-clickjacking Header | Medium | 3 |
| Cookie Without Secure Flag | Low | 2 |
| Private IP Disclosure | Low | 1 |
| Strict-Transport-Security Header Not Set | Low | Systemic |
| Timestamp Disclosure - Unix | Low | 1 |
| X-Content-Type-Options Header Missing | Low | Systemic |
| Modern Web Application | Informational | Systemic |
| Re-examine Cache-control Directives | Informational | Systemic |
| Session Management Response Identified | Informational | 2 |
| User Agent Fuzzer | Informational | Systemic |




## Alert Detail



### [ Content Security Policy (CSP) Header Not Set ](https://www.zaproxy.org/docs/alerts/10038/)



##### Medium (High)

### Description

Content Security Policy (CSP) is an added layer of security that helps to detect and mitigate certain types of attacks, including Cross Site Scripting (XSS) and data injection attacks. These attacks are used for everything from data theft to site defacement or distribution of malware. CSP provides a set of standard HTTP headers that allow website owners to declare approved sources of content that browsers should be allowed to load on that page — covered types are JavaScript, CSS, HTML frames, fonts, images and embeddable objects such as Java applets, ActiveX, audio and video files.

* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/`
  * Method: `GET`
  * Parameter: ``
  * Attack: ``
  * Evidence: ``
  * Other Info: ``
* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/Home/Location
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/Home/Location`
  * Method: `GET`
  * Parameter: ``
  * Attack: ``
  * Evidence: ``
  * Other Info: ``
* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/Introduction/ChildName
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/Introduction/ChildName`
  * Method: `GET`
  * Parameter: ``
  * Attack: ``
  * Evidence: ``
  * Other Info: ``
* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/sitemap.xml
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/sitemap.xml`
  * Method: `GET`
  * Parameter: ``
  * Attack: ``
  * Evidence: ``
  * Other Info: ``
* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/ ()(__RequestVerificationToken)`
  * Method: `POST`
  * Parameter: ``
  * Attack: ``
  * Evidence: ``
  * Other Info: ``

Instances: Systemic


### Solution

Ensure that your web server, application server, load balancer, etc. is configured to set the Content-Security-Policy header.

### Reference


* [ https://developer.mozilla.org/en-US/docs/Web/HTTP/Guides/CSP ](https://developer.mozilla.org/en-US/docs/Web/HTTP/Guides/CSP)
* [ https://cheatsheetseries.owasp.org/cheatsheets/Content_Security_Policy_Cheat_Sheet.html ](https://cheatsheetseries.owasp.org/cheatsheets/Content_Security_Policy_Cheat_Sheet.html)
* [ https://www.w3.org/TR/CSP/ ](https://www.w3.org/TR/CSP/)
* [ https://w3c.github.io/webappsec-csp/ ](https://w3c.github.io/webappsec-csp/)
* [ https://web.dev/articles/csp ](https://web.dev/articles/csp)
* [ https://caniuse.com/#feat=contentsecuritypolicy ](https://caniuse.com/#feat=contentsecuritypolicy)
* [ https://content-security-policy.com/ ](https://content-security-policy.com/)


#### CWE Id: [ 693 ](https://cwe.mitre.org/data/definitions/693.html)


#### WASC Id: 15

#### Source ID: 3

### [ Missing Anti-clickjacking Header ](https://www.zaproxy.org/docs/alerts/10020/)



##### Medium (Medium)

### Description

The response does not protect against 'ClickJacking' attacks. It should include either Content-Security-Policy with 'frame-ancestors' directive or X-Frame-Options.

* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/Home/Location
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/Home/Location`
  * Method: `GET`
  * Parameter: `x-frame-options`
  * Attack: ``
  * Evidence: ``
  * Other Info: ``
* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/Introduction/ChildName
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/Introduction/ChildName`
  * Method: `GET`
  * Parameter: `x-frame-options`
  * Attack: ``
  * Evidence: ``
  * Other Info: ``
* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/Introduction/IsChildBorn%3FchildId=c7f6acee-8b1a-4d4d-ac1d-0f3a92292247
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/Introduction/IsChildBorn (childId)`
  * Method: `GET`
  * Parameter: `x-frame-options`
  * Attack: ``
  * Evidence: ``
  * Other Info: ``


Instances: 3

### Solution

Modern Web browsers support the Content-Security-Policy and X-Frame-Options HTTP headers. Ensure one of them is set on all web pages returned by your site/app.
If you expect the page to be framed only by pages on your server (e.g. it's part of a FRAMESET) then you'll want to use SAMEORIGIN, otherwise if you never expect the page to be framed, you should use DENY. Alternatively consider implementing Content Security Policy's "frame-ancestors" directive.

### Reference


* [ https://developer.mozilla.org/en-US/docs/Web/HTTP/Reference/Headers/X-Frame-Options ](https://developer.mozilla.org/en-US/docs/Web/HTTP/Reference/Headers/X-Frame-Options)


#### CWE Id: [ 1021 ](https://cwe.mitre.org/data/definitions/1021.html)


#### WASC Id: 15

#### Source ID: 3

### [ Cookie Without Secure Flag ](https://www.zaproxy.org/docs/alerts/10011/)



##### Low (Medium)

### Description

A cookie has been set without the secure flag, which means that the cookie can be accessed via unencrypted connections.

* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/`
  * Method: `GET`
  * Parameter: `.AspNetCore.Antiforgery.RtGCWVXC8-4`
  * Attack: ``
  * Evidence: `Set-Cookie: .AspNetCore.Antiforgery.RtGCWVXC8-4`
  * Other Info: ``
* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/Home/Location
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/Home/Location ()(Country,__RequestVerificationToken)`
  * Method: `POST`
  * Parameter: `.AspNetCore.Session`
  * Attack: ``
  * Evidence: `Set-Cookie: .AspNetCore.Session`
  * Other Info: ``


Instances: 2

### Solution

Whenever a cookie contains sensitive information or is a session token, then it should always be passed using an encrypted channel. Ensure that the secure flag is set for cookies containing such sensitive information.

### Reference


* [ https://owasp.org/www-project-web-security-testing-guide/v41/4-Web_Application_Security_Testing/06-Session_Management_Testing/02-Testing_for_Cookies_Attributes.html ](https://owasp.org/www-project-web-security-testing-guide/v41/4-Web_Application_Security_Testing/06-Session_Management_Testing/02-Testing_for_Cookies_Attributes.html)


#### CWE Id: [ 614 ](https://cwe.mitre.org/data/definitions/614.html)


#### WASC Id: 13

#### Source ID: 3

### [ Private IP Disclosure ](https://www.zaproxy.org/docs/alerts/2/)



##### Low (Medium)

### Description

A private IP (such as 10.x.x.x, 172.x.x.x, 192.168.x.x) or an Amazon EC2 private hostname (for example, ip-10-0-56-78) has been found in the HTTP response body. This information might be helpful for further attacks targeting internal systems.

* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/assets/images/favicon.svg%3Fv=6.0.0
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/assets/images/favicon.svg (v)`
  * Method: `GET`
  * Parameter: ``
  * Attack: ``
  * Evidence: `10.02.68.86`
  * Other Info: `10.02.68.86
`


Instances: 1

### Solution

Remove the private IP address from the HTTP response body. For comments, use JSP/ASP/PHP comment instead of HTML/JavaScript comment which can be seen by client browsers.

### Reference


* [ https://datatracker.ietf.org/doc/html/rfc1918 ](https://datatracker.ietf.org/doc/html/rfc1918)


#### CWE Id: [ 497 ](https://cwe.mitre.org/data/definitions/497.html)


#### WASC Id: 13

#### Source ID: 3

### [ Strict-Transport-Security Header Not Set ](https://www.zaproxy.org/docs/alerts/10035/)



##### Low (High)

### Description

HTTP Strict Transport Security (HSTS) is a web security policy mechanism whereby a web server declares that complying user agents (such as a web browser) are to interact with it using only secure HTTPS connections (i.e. HTTP layered over TLS/SSL). HSTS is an IETF standards track protocol and is specified in RFC 6797.

* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/`
  * Method: `GET`
  * Parameter: ``
  * Attack: ``
  * Evidence: ``
  * Other Info: ``
* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/Home/Location
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/Home/Location`
  * Method: `GET`
  * Parameter: ``
  * Attack: ``
  * Evidence: ``
  * Other Info: ``
* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/assets/images/govuk-icon-180.png%3Fv=6.0.0
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/assets/images/govuk-icon-180.png (v)`
  * Method: `GET`
  * Parameter: ``
  * Attack: ``
  * Evidence: ``
  * Other Info: ``
* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/robots.txt
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/robots.txt`
  * Method: `GET`
  * Parameter: ``
  * Attack: ``
  * Evidence: ``
  * Other Info: ``
* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/sitemap.xml
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/sitemap.xml`
  * Method: `GET`
  * Parameter: ``
  * Attack: ``
  * Evidence: ``
  * Other Info: ``

Instances: Systemic


### Solution

Ensure that your web server, application server, load balancer, etc. is configured to enforce Strict-Transport-Security.

### Reference


* [ https://cheatsheetseries.owasp.org/cheatsheets/HTTP_Strict_Transport_Security_Cheat_Sheet.html ](https://cheatsheetseries.owasp.org/cheatsheets/HTTP_Strict_Transport_Security_Cheat_Sheet.html)
* [ https://owasp.org/www-community/Security_Headers ](https://owasp.org/www-community/Security_Headers)
* [ https://en.wikipedia.org/wiki/HTTP_Strict_Transport_Security ](https://en.wikipedia.org/wiki/HTTP_Strict_Transport_Security)
* [ https://caniuse.com/stricttransportsecurity ](https://caniuse.com/stricttransportsecurity)
* [ https://datatracker.ietf.org/doc/html/rfc6797 ](https://datatracker.ietf.org/doc/html/rfc6797)


#### CWE Id: [ 319 ](https://cwe.mitre.org/data/definitions/319.html)


#### WASC Id: 15

#### Source ID: 3

### [ Timestamp Disclosure - Unix ](https://www.zaproxy.org/docs/alerts/10096/)



##### Low (Low)

### Description

A timestamp was disclosed by the application/web server. - Unix

* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/govuk-frontend.min.css%3Fv=6.0.0
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/govuk-frontend.min.css (v)`
  * Method: `GET`
  * Parameter: ``
  * Attack: ``
  * Evidence: `1904761905`
  * Other Info: `1904761905, which evaluates to: 2030-05-11 20:31:45.`


Instances: 1

### Solution

Manually confirm that the timestamp data is not sensitive, and that the data cannot be aggregated to disclose exploitable patterns.

### Reference


* [ https://cwe.mitre.org/data/definitions/200.html ](https://cwe.mitre.org/data/definitions/200.html)


#### CWE Id: [ 497 ](https://cwe.mitre.org/data/definitions/497.html)


#### WASC Id: 13

#### Source ID: 3

### [ X-Content-Type-Options Header Missing ](https://www.zaproxy.org/docs/alerts/10021/)



##### Low (Medium)

### Description

The Anti-MIME-Sniffing header X-Content-Type-Options was not set to 'nosniff'. This allows older versions of Internet Explorer and Chrome to perform MIME-sniffing on the response body, potentially causing the response body to be interpreted and displayed as a content type other than the declared content type. Current (early 2014) and legacy versions of Firefox will use the declared content type (if one is set), rather than performing MIME-sniffing.

* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/`
  * Method: `GET`
  * Parameter: `x-content-type-options`
  * Attack: ``
  * Evidence: ``
  * Other Info: `This issue still applies to error type pages (401, 403, 500, etc.) as those pages are often still affected by injection issues, in which case there is still concern for browsers sniffing pages away from their actual content type.
At "High" threshold this scan rule will not alert on client or server error responses.`
* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/Home/Location
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/Home/Location`
  * Method: `GET`
  * Parameter: `x-content-type-options`
  * Attack: ``
  * Evidence: ``
  * Other Info: `This issue still applies to error type pages (401, 403, 500, etc.) as those pages are often still affected by injection issues, in which case there is still concern for browsers sniffing pages away from their actual content type.
At "High" threshold this scan rule will not alert on client or server error responses.`
* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/assets/images/favicon.ico%3Fv=6.0.0
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/assets/images/favicon.ico (v)`
  * Method: `GET`
  * Parameter: `x-content-type-options`
  * Attack: ``
  * Evidence: ``
  * Other Info: `This issue still applies to error type pages (401, 403, 500, etc.) as those pages are often still affected by injection issues, in which case there is still concern for browsers sniffing pages away from their actual content type.
At "High" threshold this scan rule will not alert on client or server error responses.`
* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/assets/images/govuk-icon-180.png%3Fv=6.0.0
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/assets/images/govuk-icon-180.png (v)`
  * Method: `GET`
  * Parameter: `x-content-type-options`
  * Attack: ``
  * Evidence: ``
  * Other Info: `This issue still applies to error type pages (401, 403, 500, etc.) as those pages are often still affected by injection issues, in which case there is still concern for browsers sniffing pages away from their actual content type.
At "High" threshold this scan rule will not alert on client or server error responses.`
* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/robots.txt
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/robots.txt`
  * Method: `GET`
  * Parameter: `x-content-type-options`
  * Attack: ``
  * Evidence: ``
  * Other Info: `This issue still applies to error type pages (401, 403, 500, etc.) as those pages are often still affected by injection issues, in which case there is still concern for browsers sniffing pages away from their actual content type.
At "High" threshold this scan rule will not alert on client or server error responses.`

Instances: Systemic


### Solution

Ensure that the application/web server sets the Content-Type header appropriately, and that it sets the X-Content-Type-Options header to 'nosniff' for all web pages.
If possible, ensure that the end user uses a standards-compliant and modern web browser that does not perform MIME-sniffing at all, or that can be directed by the web application/web server to not perform MIME-sniffing.

### Reference


* [ https://learn.microsoft.com/en-us/previous-versions/windows/internet-explorer/ie-developer/compatibility/gg622941(v=vs.85) ](https://learn.microsoft.com/en-us/previous-versions/windows/internet-explorer/ie-developer/compatibility/gg622941(v=vs.85))
* [ https://owasp.org/www-community/Security_Headers ](https://owasp.org/www-community/Security_Headers)


#### CWE Id: [ 693 ](https://cwe.mitre.org/data/definitions/693.html)


#### WASC Id: 15

#### Source ID: 3

### [ Modern Web Application ](https://www.zaproxy.org/docs/alerts/10109/)



##### Informational (Medium)

### Description

The application appears to be a modern web application. If you need to explore it automatically then the Ajax Spider may well be more effective than the standard one.

* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/`
  * Method: `GET`
  * Parameter: ``
  * Attack: ``
  * Evidence: `<a class="govuk-link" href="#">give your feedback (opens in new tab)</a>`
  * Other Info: `Links have been found that do not have traditional href attributes, which is an indication that this is a modern web application.`
* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/Home/Location
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/Home/Location`
  * Method: `GET`
  * Parameter: ``
  * Attack: ``
  * Evidence: `<a class="govuk-link" href="#">give your feedback (opens in new tab)</a>`
  * Other Info: `Links have been found that do not have traditional href attributes, which is an indication that this is a modern web application.`
* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/Introduction/ChildName
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/Introduction/ChildName`
  * Method: `GET`
  * Parameter: ``
  * Attack: ``
  * Evidence: `<a class="govuk-link" href="#">give your feedback (opens in new tab)</a>`
  * Other Info: `Links have been found that do not have traditional href attributes, which is an indication that this is a modern web application.`
* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/sitemap.xml
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/sitemap.xml`
  * Method: `GET`
  * Parameter: ``
  * Attack: ``
  * Evidence: `<a class="govuk-link" href="#">give your feedback (opens in new tab)</a>`
  * Other Info: `Links have been found that do not have traditional href attributes, which is an indication that this is a modern web application.`
* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/ ()(__RequestVerificationToken)`
  * Method: `POST`
  * Parameter: ``
  * Attack: ``
  * Evidence: `<a class="govuk-link" href="#">give your feedback (opens in new tab)</a>`
  * Other Info: `Links have been found that do not have traditional href attributes, which is an indication that this is a modern web application.`

Instances: Systemic


### Solution

This is an informational alert and so no changes are required.

### Reference




#### Source ID: 3

### [ Re-examine Cache-control Directives ](https://www.zaproxy.org/docs/alerts/10015/)



##### Informational (Low)

### Description

The cache-control header has not been set properly or is missing, allowing the browser and proxies to cache content. For static assets like css, js, or image files this might be intended, however, the resources should be reviewed to ensure that no sensitive content will be cached.

* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/`
  * Method: `GET`
  * Parameter: `cache-control`
  * Attack: ``
  * Evidence: `no-cache, no-store`
  * Other Info: ``
* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/Home/Location
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/Home/Location`
  * Method: `GET`
  * Parameter: `cache-control`
  * Attack: ``
  * Evidence: `no-cache, no-store`
  * Other Info: ``
* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/Introduction/ChildName
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/Introduction/ChildName`
  * Method: `GET`
  * Parameter: `cache-control`
  * Attack: ``
  * Evidence: `no-cache, no-store`
  * Other Info: ``
* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/assets/manifest.json%3Fv=6.0.0
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/assets/manifest.json (v)`
  * Method: `GET`
  * Parameter: `cache-control`
  * Attack: ``
  * Evidence: ``
  * Other Info: ``
* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/robots.txt
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/robots.txt`
  * Method: `GET`
  * Parameter: `cache-control`
  * Attack: ``
  * Evidence: ``
  * Other Info: ``

Instances: Systemic


### Solution

For secure content, ensure the cache-control HTTP header is set with "no-cache, no-store, must-revalidate". If an asset should be cached consider setting the directives "public, max-age, immutable".

### Reference


* [ https://cheatsheetseries.owasp.org/cheatsheets/Session_Management_Cheat_Sheet.html#web-content-caching ](https://cheatsheetseries.owasp.org/cheatsheets/Session_Management_Cheat_Sheet.html#web-content-caching)
* [ https://developer.mozilla.org/en-US/docs/Web/HTTP/Reference/Headers/Cache-Control ](https://developer.mozilla.org/en-US/docs/Web/HTTP/Reference/Headers/Cache-Control)
* [ https://grayduck.mn/2021/09/13/cache-control-recommendations/ ](https://grayduck.mn/2021/09/13/cache-control-recommendations/)


#### CWE Id: [ 525 ](https://cwe.mitre.org/data/definitions/525.html)


#### WASC Id: 13

#### Source ID: 3

### [ Session Management Response Identified ](https://www.zaproxy.org/docs/alerts/10112/)



##### Informational (Medium)

### Description

The given response has been identified as containing a session management token. The 'Other Info' field contains a set of header tokens that can be used in the Header Based Session Management Method. If the request is in a context which has a Session Management Method set to "Auto-Detect" then this rule will change the session management to use the tokens identified.

* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/`
  * Method: `GET`
  * Parameter: `.AspNetCore.Antiforgery.RtGCWVXC8-4`
  * Attack: ``
  * Evidence: `.AspNetCore.Antiforgery.RtGCWVXC8-4`
  * Other Info: `cookie:.AspNetCore.Antiforgery.RtGCWVXC8-4`
* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/Home/Location
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/Home/Location ()(Country,__RequestVerificationToken)`
  * Method: `POST`
  * Parameter: `.AspNetCore.Session`
  * Attack: ``
  * Evidence: `.AspNetCore.Session`
  * Other Info: `cookie:.AspNetCore.Session`


Instances: 2

### Solution

This is an informational alert rather than a vulnerability and so there is nothing to fix.

### Reference


* [ https://www.zaproxy.org/docs/desktop/addons/authentication-helper/session-mgmt-id/ ](https://www.zaproxy.org/docs/desktop/addons/authentication-helper/session-mgmt-id/)



#### Source ID: 3

### [ User Agent Fuzzer ](https://www.zaproxy.org/docs/alerts/10104/)



##### Informational (Medium)

### Description

Check for differences in response based on fuzzed User Agent (eg. mobile sites, access as a Search Engine Crawler). Compares the response statuscode and the hashcode of the response body with the original response.

* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/Home
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/Home`
  * Method: `GET`
  * Parameter: `Header User-Agent`
  * Attack: `Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1)`
  * Evidence: ``
  * Other Info: ``
* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/Home/Location
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/Home/Location`
  * Method: `GET`
  * Parameter: `Header User-Agent`
  * Attack: `Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1)`
  * Evidence: ``
  * Other Info: ``
* URL: https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/
  * Node Name: `https://s279d01-uks-cec-web-fd-endpoint-c9g7dsevedbdascx.a02.azurefd.net/ ()(__RequestVerificationToken)`
  * Method: `POST`
  * Parameter: `Header User-Agent`
  * Attack: `Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1)`
  * Evidence: ``
  * Other Info: ``

Instances: Systemic


### Solution



### Reference


* [ https://owasp.org/wstg ](https://owasp.org/wstg)



#### Source ID: 1


