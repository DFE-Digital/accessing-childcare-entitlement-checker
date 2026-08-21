---
title: ZAP Scanning Report
layout: sub-navigation
sectionKey: Reference
eleventyNavigation:
  parent: Testing
  key: Scanning Report
order: 99
---

## Summary of Alerts

| Risk Level | Number of Alerts |
| --- | --- |
| High | 0 |
| Medium | 0 |
| Low | 0 |
| Informational | 3 |




## Insights

| Level | Reason | Site | Description | Statistic |
| --- | --- | --- | --- | --- |
| Low | Warning |  | ZAP errors logged - see the zap.log file for details | 1    |
| Low | Warning |  | ZAP warnings logged - see the zap.log file for details | 7    |
| Info | Informational | https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net | Percentage of responses with status code 2xx | 66 % |
| Info | Informational | https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net | Percentage of responses with status code 3xx | 1 % |
| Info | Informational | https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net | Percentage of responses with status code 4xx | 31 % |
| Info | Informational | https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net | Percentage of endpoints with content type application/json | 5 % |
| Info | Informational | https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net | Percentage of endpoints with content type image/png | 5 % |
| Info | Informational | https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net | Percentage of endpoints with content type image/svg+xml | 10 % |
| Info | Informational | https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net | Percentage of endpoints with content type image/x-icon | 5 % |
| Info | Informational | https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net | Percentage of endpoints with content type text/css | 5 % |
| Info | Informational | https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net | Percentage of endpoints with content type text/html | 40 % |
| Info | Informational | https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net | Percentage of endpoints with content type text/javascript | 5 % |
| Info | Informational | https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net | Percentage of endpoints with content type text/plain | 5 % |
| Info | Informational | https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net | Percentage of endpoints with method GET | 80 % |
| Info | Informational | https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net | Percentage of endpoints with method POST | 20 % |
| Info | Informational | https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net | Count of total endpoints | 20    |
| Info | Informational | https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net | Percentage of slow responses | 100 % |







## Alerts

| Name | Risk Level | Number of Instances |
| --- | --- | --- |
| Re-examine Cache-control Directives | Informational | Systemic |
| User Agent Fuzzer | Informational | Systemic |
| User Controllable HTML Element Attribute (Potential XSS) | Informational | 1 |
| Modern Web Application | 				False Positives: | 4 |
| Private IP Disclosure | 				False Positives: | 1 |
| Session Management Response Identified | 				False Positives: | 3 |
| Timestamp Disclosure - Unix | 				False Positives: | 1 |




## Alert Detail



### [ Re-examine Cache-control Directives ](https://www.zaproxy.org/docs/alerts/10015/)



##### Informational (Low)

### Description

The cache-control header is set incorrectly or is missing, allowing browsers and proxies to cache content. For static assets (such as CSS, JS, or image files), this behavior is typical; however, resources must be reviewed to ensure that no sensitive content is cached.

* URL: https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/
  * Node Name: `https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/`
  * Method: `GET`
  * Parameter: `cache-control`
  * Attack: ``
  * Evidence: `no-cache, no-store`
  * Other Info: ``
* URL: https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/assets/manifest.json%3Fv=6.3.0
  * Node Name: `https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/assets/manifest.json (v)`
  * Method: `GET`
  * Parameter: `cache-control`
  * Attack: ``
  * Evidence: `cache-control: public, max-age=31536000, immutable`
  * Other Info: ``
* URL: https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/cookies
  * Node Name: `https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/cookies`
  * Method: `GET`
  * Parameter: `cache-control`
  * Attack: ``
  * Evidence: `no-cache, no-store`
  * Other Info: ``
* URL: https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/robots.txt
  * Node Name: `https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/robots.txt`
  * Method: `GET`
  * Parameter: `cache-control`
  * Attack: ``
  * Evidence: ``
  * Other Info: ``
* URL: https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/where-do-you-live
  * Node Name: `https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/where-do-you-live`
  * Method: `GET`
  * Parameter: `cache-control`
  * Attack: ``
  * Evidence: `no-cache, no-store`
  * Other Info: ``

Instances: Systemic


### Solution

For secure content, the cache-control HTTP header is configured with "no-cache, no-store, must-revalidate". If caching of an asset is intended, the directives are set to "public, max-age, immutable".

### Reference


* [ https://cheatsheetseries.owasp.org/cheatsheets/Session_Management_Cheat_Sheet.html#web-content-caching ](https://cheatsheetseries.owasp.org/cheatsheets/Session_Management_Cheat_Sheet.html#web-content-caching)
* [ https://developer.mozilla.org/en-US/docs/Web/HTTP/Reference/Headers/Cache-Control ](https://developer.mozilla.org/en-US/docs/Web/HTTP/Reference/Headers/Cache-Control)
* [ https://grayduck.mn/2021/09/13/cache-control-recommendations/ ](https://grayduck.mn/2021/09/13/cache-control-recommendations/)


#### CWE Id: [ 525 ](https://cwe.mitre.org/data/definitions/525.html)


#### WASC Id: 13

#### Source ID: 3

### [ User Agent Fuzzer ](https://www.zaproxy.org/docs/alerts/10104/)



##### Informational (Medium)

### Description

This check analyzes response variations based on a fuzzed User Agent (e.g. mobile sites, crawler access). Status codes and body hash values are compared against the baseline response.

* URL: https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/
  * Node Name: `https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/`
  * Method: `GET`
  * Parameter: `Header User-Agent`
  * Attack: `Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)`
  * Evidence: ``
  * Other Info: ``
* URL: https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/
  * Node Name: `https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/`
  * Method: `GET`
  * Parameter: `Header User-Agent`
  * Attack: `Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1)`
  * Evidence: ``
  * Other Info: ``
* URL: https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/assets
  * Node Name: `https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/assets`
  * Method: `GET`
  * Parameter: `Header User-Agent`
  * Attack: `Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1)`
  * Evidence: ``
  * Other Info: ``

Instances: Systemic


### Solution



### Reference


* [ https://owasp.org/wstg ](https://owasp.org/wstg)



#### Source ID: 1

### [ User Controllable HTML Element Attribute (Potential XSS) ](https://www.zaproxy.org/docs/alerts/10031/)



##### Informational (Low)

### Description

This check analyzes user-supplied input in query string parameters and POST data to identify controllable HTML attribute values, providing detection of potential XSS (cross-site scripting) points that require analyst review to determine exploitability.

* URL: https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/cookies%3FhasSetCookies=True
  * Node Name: `https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/cookies (hasSetCookies)`
  * Method: `GET`
  * Parameter: `hasSetCookies`
  * Attack: ``
  * Evidence: ``
  * Other Info: `User-controlled HTML attribute values were found. Special character injection testing may be conducted to determine exploitability. The page at the following URL:

https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/cookies?hasSetCookies=True

includes user input in:
a(n) [input] tag [value] attribute

The user input found was:
hasSetCookies=True

The user-controlled value was:
true`


Instances: 1

### Solution

Input must be validated and output sanitized before writing to any HTML attributes.

### Reference


* [ https://cheatsheetseries.owasp.org/cheatsheets/Input_Validation_Cheat_Sheet.html ](https://cheatsheetseries.owasp.org/cheatsheets/Input_Validation_Cheat_Sheet.html)


#### CWE Id: [ 20 ](https://cwe.mitre.org/data/definitions/20.html)


#### WASC Id: 20

#### Source ID: 3

### [ Modern Web Application ](https://www.zaproxy.org/docs/alerts/10109/)



##### 				False Positives: (False Positive)

### Description

The application exhibits characteristics of a modern web application. If automated exploration is required, the Client Spider may be more effective than the standard spider.

* URL: https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/
  * Node Name: `https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/`
  * Method: `GET`
  * Parameter: ``
  * Attack: ``
  * Evidence: `<a class="govuk-footer__link" href="#">Item 2</a>`
  * Other Info: `Links have been found that do not have traditional href attributes, which is an indication that this is a modern web application.`
* URL: https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/cookies
  * Node Name: `https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/cookies`
  * Method: `GET`
  * Parameter: ``
  * Attack: ``
  * Evidence: `<a class="govuk-footer__link" href="#">Item 2</a>`
  * Other Info: `Links have been found that do not have traditional href attributes, which is an indication that this is a modern web application.`
* URL: https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/sitemap.xml
  * Node Name: `https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/sitemap.xml`
  * Method: `GET`
  * Parameter: ``
  * Attack: ``
  * Evidence: `<a class="govuk-footer__link" href="#">Item 2</a>`
  * Other Info: `Links have been found that do not have traditional href attributes, which is an indication that this is a modern web application.`
* URL: https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/where-do-you-live
  * Node Name: `https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/where-do-you-live`
  * Method: `GET`
  * Parameter: ``
  * Attack: ``
  * Evidence: `<a class="govuk-footer__link" href="#">Item 2</a>`
  * Other Info: `Links have been found that do not have traditional href attributes, which is an indication that this is a modern web application.`


Instances: 4

### Solution

This is an informational alert; no changes are required.

### Reference




#### Source ID: 3

### [ Private IP Disclosure ](https://www.zaproxy.org/docs/alerts/2/)



##### 				False Positives: (False Positive)

### Description

A private IP (such as 10.x.x.x, 172.x.x.x, 192.168.x.x) or an Amazon EC2 private hostname (for example, ip-10-0-56-78) has been found in the HTTP response body. This information might be helpful for further attacks targeting internal systems.

* URL: https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/assets/images/favicon.svg%3Fv=6.3.0
  * Node Name: `https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/assets/images/favicon.svg (v)`
  * Method: `GET`
  * Parameter: ``
  * Attack: ``
  * Evidence: `10.02.68.86`
  * Other Info: `10.02.68.86
`


Instances: 1

### Solution

The private IP address is removed from the HTTP response body. For comments, server-side comments (e.g. JSP/ASP/PHP) are used instead of HTML/JavaScript comments that are visible to client browsers.

### Reference


* [ https://datatracker.ietf.org/doc/html/rfc1918 ](https://datatracker.ietf.org/doc/html/rfc1918)


#### CWE Id: [ 497 ](https://cwe.mitre.org/data/definitions/497.html)


#### WASC Id: 13

#### Source ID: 3

### [ Session Management Response Identified ](https://www.zaproxy.org/docs/alerts/10112/)



##### 				False Positives: (False Positive)

### Description

The given response has been identified as containing a session management token. The 'Other Info' field contains a set of header tokens that can be used in the Header Based Session Management Method. If the request is in a context which has a Session Management Method set to "Auto-Detect" then this rule will change the session management to use the tokens identified.

* URL: https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/
  * Node Name: `https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/`
  * Method: `GET`
  * Parameter: `.AspNetCore.Antiforgery.RtGCWVXC8-4`
  * Attack: ``
  * Evidence: `.AspNetCore.Antiforgery.RtGCWVXC8-4`
  * Other Info: `cookie:.AspNetCore.Antiforgery.RtGCWVXC8-4`
* URL: https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/sitemap.xml
  * Node Name: `https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/sitemap.xml`
  * Method: `GET`
  * Parameter: `.AspNetCore.Antiforgery.RtGCWVXC8-4`
  * Attack: ``
  * Evidence: `.AspNetCore.Antiforgery.RtGCWVXC8-4`
  * Other Info: `cookie:.AspNetCore.Antiforgery.RtGCWVXC8-4`
* URL: https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/where-do-you-live
  * Node Name: `https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/where-do-you-live ()(Country,ReturnTo,__RequestVerificationToken)`
  * Method: `POST`
  * Parameter: `.AspNetCore.Session`
  * Attack: ``
  * Evidence: `.AspNetCore.Session`
  * Other Info: `cookie:.AspNetCore.Session`


Instances: 3

### Solution

This is an informational alert rather than a vulnerability; no action is required.

### Reference


* [ https://www.zaproxy.org/docs/desktop/addons/authentication-helper/session-mgmt-id/ ](https://www.zaproxy.org/docs/desktop/addons/authentication-helper/session-mgmt-id/)



#### Source ID: 3

### [ Timestamp Disclosure - Unix ](https://www.zaproxy.org/docs/alerts/10096/)



##### 				False Positives: (False Positive)

### Description

A timestamp was disclosed by the application/web server. - Unix

* URL: https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/govuk-frontend.min.css%3Fv=6.3.0
  * Node Name: `https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net/govuk-frontend.min.css (v)`
  * Method: `GET`
  * Parameter: ``
  * Attack: ``
  * Evidence: `1904761905`
  * Other Info: `1904761905, which evaluates to: 2030-05-11 20:31:45.`


Instances: 1

### Solution

Manual verification is conducted to ensure the timestamp data is not sensitive and cannot be aggregated to disclose exploitable patterns.

### Reference


* [ https://cwe.mitre.org/data/definitions/200.html ](https://cwe.mitre.org/data/definitions/200.html)


#### CWE Id: [ 497 ](https://cwe.mitre.org/data/definitions/497.html)


#### WASC Id: 13

#### Source ID: 3


