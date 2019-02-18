## Reminders

The concept for this prototype is to provide an easy to digest days-to-go dsshboard for significant dates entered by the user.

It consists of a React, TypeScript, and Material-UI front-end, which communicates with an ASP.Net Core WebAPI backend.

In its current version 0.7.0, persistence is to MySQL via Dapper, and authentication is provided by Auth0.

It's intended to be served by the Caddy webserver running on CentOS Linux.