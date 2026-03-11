# Volumetrics

## Introduction

It is important to understand the Volumetrics of the Care Leavers website to ensure that the website can handle the
expected traffic without degradation in performance, without incurring unnecessary Azure costs.

This is a new site, so no historical data is available. However, we can make estimates based on data from other
similar GovUK sites.

## Projected Volumetrics

The assumptions will be based on 1,000 users per month, so roughly 33 users per day. We can make reasonable assumptions
that the site will be operating during UK day time hours, with increased usage during work hours due to access by
intermediaries. Therefore, we can make an assumption of around 10 users per hour at peak times.

Assuming a user session lasts roughly 5 minutes: 300 * 10 = 30 concurrent requests.

During spike times, we expect around x3 this amount (such as when comms have been sent out).

| Test Type | Concurrent Users | Requests per second |
|-----------|------------------|---------------------|
| Normal    | 30               | ~3 requests/sec     |
| Spike     | 90               | ~9 requests/sec     |
| Stress    | 150              | ~15 requests/sec    |




