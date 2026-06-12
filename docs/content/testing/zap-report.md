---
title: ZAP Scanning Report
layout: sub-navigation
sectionKey: Testing
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
| Low | 2 |
| Informational | 4 |




## Insights

| Level | Reason | Site | Description | Statistic |
| --- | --- | --- | --- | --- |
| Low | Warning |  | ZAP errors logged - see the zap.log file for details | 1    |
| Low | Warning |  | ZAP warnings logged - see the zap.log file for details | 7    |
| Info | Informational | https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net | Percentage of responses with status code 2xx | 58 % |
| Info | Informational | https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net | Percentage of responses with status code 3xx | 1 % |
| Info | Informational | https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net | Percentage of responses with status code 4xx | 40 % |
| Info | Informational | https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net | Percentage of endpoints with content type application/json | 5 % |
| Info | Informational | https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net | Percentage of endpoints with content type image/png | 5 % |
| Info | Informational | https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net | Percentage of endpoints with content type image/svg+xml | 11 % |
| Info | Informational | https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net | Percentage of endpoints with content type image/x-icon | 5 % |
| Info | Informational | https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net | Percentage of endpoints with content type text/css | 5 % |
| Info | Informational | https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net | Percentage of endpoints with content type text/html | 41 % |
| Info | Informational | https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net | Percentage of endpoints with content type text/javascript | 5 % |
| Info | Informational | https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net | Percentage of endpoints with content type text/plain | 5 % |
| Info | Informational | https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net | Percentage of endpoints with method GET | 82 % |
| Info | Informational | https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net | Percentage of endpoints with method POST | 17 % |
| Info | Informational | https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net | Count of total endpoints | 17    |
| Info | Informational | https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net | Percentage of slow responses | 73 % |




## Alerts

| Name | Risk Level | Number of Instances |
| --- | --- | --- |
| Private IP Disclosure | Low | 1 |
| Timestamp Disclosure - Unix | Low | 1 |
| Modern Web Application | Informational | Systemic |
| Re-examine Cache-control Directives | Informational | Systemic |
| Session Management Response Identified | Informational | 2 |
| User Agent Fuzzer | Informational | Systemic |




## Alert Detail



### [ Private IP Disclosure ](https://www.zaproxy.org/docs/alerts/2/)



##### Low (Medium)

### Description

A private IP (such as 10.x.x.x, 172.x.x.x, 192.168.x.x) or an Amazon EC2 private hostname (for example, ip-10-0-56-78) has been found in the HTTP response body. This information might be helpful for further attacks targeting internal systems.

* URL: https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/assets/images/favicon.svg%3Fv=6.0.0
  * Node Name: `https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/assets/images/favicon.svg (v)`
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

### [ Timestamp Disclosure - Unix ](https://www.zaproxy.org/docs/alerts/10096/)



##### Low (Low)

### Description

A timestamp was disclosed by the application/web server. - Unix

* URL: https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/govuk-frontend.min.css%3Fv=6.0.0
  * Node Name: `https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/govuk-frontend.min.css (v)`
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

### [ Modern Web Application ](https://www.zaproxy.org/docs/alerts/10109/)



##### Informational (Medium)

### Description

The application appears to be a modern web application. If you need to explore it automatically then the Ajax Spider may well be more effective than the standard one.

* URL: https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/
  * Node Name: `https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/`
  * Method: `GET`
  * Parameter: ``
  * Attack: ``
  * Evidence: `<a class="govuk-link" href="#">give your feedback (opens in new tab)</a>`
  * Other Info: `Links have been found that do not have traditional href attributes, which is an indication that this is a modern web application.`
* URL: https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/Home/Location
  * Node Name: `https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/Home/Location`
  * Method: `GET`
  * Parameter: ``
  * Attack: ``
  * Evidence: `<a class="govuk-link" href="#">give your feedback (opens in new tab)</a>`
  * Other Info: `Links have been found that do not have traditional href attributes, which is an indication that this is a modern web application.`
* URL: https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/Introduction/ChildName
  * Node Name: `https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/Introduction/ChildName`
  * Method: `GET`
  * Parameter: ``
  * Attack: ``
  * Evidence: `<a class="govuk-link" href="#">give your feedback (opens in new tab)</a>`
  * Other Info: `Links have been found that do not have traditional href attributes, which is an indication that this is a modern web application.`
* URL: https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/sitemap.xml
  * Node Name: `https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/sitemap.xml`
  * Method: `GET`
  * Parameter: ``
  * Attack: ``
  * Evidence: `<a class="govuk-link" href="#">give your feedback (opens in new tab)</a>`
  * Other Info: `Links have been found that do not have traditional href attributes, which is an indication that this is a modern web application.`
* URL: https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/
  * Node Name: `https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/ ()(__RequestVerificationToken)`
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

* URL: https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/
  * Node Name: `https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/`
  * Method: `GET`
  * Parameter: `cache-control`
  * Attack: ``
  * Evidence: `no-cache, no-store`
  * Other Info: ``
* URL: https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/Home/Location
  * Node Name: `https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/Home/Location`
  * Method: `GET`
  * Parameter: `cache-control`
  * Attack: ``
  * Evidence: `no-cache, no-store`
  * Other Info: ``
* URL: https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/Introduction/ChildName
  * Node Name: `https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/Introduction/ChildName`
  * Method: `GET`
  * Parameter: `cache-control`
  * Attack: ``
  * Evidence: `no-cache, no-store`
  * Other Info: ``
* URL: https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/assets/manifest.json%3Fv=6.0.0
  * Node Name: `https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/assets/manifest.json (v)`
  * Method: `GET`
  * Parameter: `cache-control`
  * Attack: ``
  * Evidence: `cache-control: public, max-age=31536000, immutable`
  * Other Info: ``
* URL: https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/robots.txt
  * Node Name: `https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/robots.txt`
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

* URL: https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/
  * Node Name: `https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/`
  * Method: `GET`
  * Parameter: `.AspNetCore.Antiforgery.RtGCWVXC8-4`
  * Attack: ``
  * Evidence: `.AspNetCore.Antiforgery.RtGCWVXC8-4`
  * Other Info: `cookie:.AspNetCore.Antiforgery.RtGCWVXC8-4`
* URL: https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/Home/Location
  * Node Name: `https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/Home/Location ()(Country,__RequestVerificationToken)`
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

* URL: https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/
  * Node Name: `https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/`
  * Method: `GET`
  * Parameter: `Header User-Agent`
  * Attack: `Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)`
  * Evidence: ``
  * Other Info: ``
* URL: https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/
  * Node Name: `https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/`
  * Method: `GET`
  * Parameter: `Header User-Agent`
  * Attack: `Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1)`
  * Evidence: ``
  * Other Info: ``
* URL: https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/Home
  * Node Name: `https://s279t01-uks-cec-web-fd-endpoint-epgjc9h2b8hxgehc.a02.azurefd.net/Home`
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


