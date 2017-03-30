# LEWSLoadBalancer
Load balancer for long-executing web services

## Projects

### Mock.Client.WebThinClient
Mock web thin client for executing test requests to load balancer.

### Mock.Service.SimulationRunner
A mock service that simulates a long-executing web service. The service does not contain any state information. Instead, simulation is determined done by random process. Each status check generates a random number between 0 and 63 and if the number is greater than 55, the job is determined done. Otherwise, it is left in processing status.

Mock service is a windows service hosted Web API. Endpoint uri can be set in app.config file. Setting to change is *UriEndpoint*.

### Service.LoadBalancer
LoadBalancer service is a diagnostic and pseudo load balancing windows service that by default runs on port 11111. This can be changed by modifying app.config file setting *UriEndpoint*

#### Diagnostics
Diagnostics module's task is to determine how many simulation services are active at specific time. Due to speed of implementation, checking is done in parallel using sockets API to connect to the simulation service endpoint.

Currently only two endpoints are supported. Their address can be set in app.config in settings *S1* and *S2* respectively.

Diagnostics run in its own long-running thread and is executed every second. The thread is stopped when windows service is stopped.

If node is determined unavailable an email is passed to specified recipients. For this functionality to work, an active and accessible SMTP server must be specified in app.config file. Settings connected to notifications are:
* *AdminMail* - comma separated list of recipients email addresses
* *FromAddress* - email address from which email will be sent
* *SmtpServer* - SMTP server address
* *SmtpPort* - port of SMTP server (25 is default)
* *SmtpUsername* - username used to connect to SMTP server (optional)
* *SmtpPassword* - password used to connect to SMTP server (optional, required if SmtpUsername is set)

Notifications are sent only on Up -> Down change in endpoint availability.

#### LoadBalancer
Module is a web API interface which based on diagnostics data, figures out which simulation service endpoint should be used and tries to process request. Successuflly started requests are stored into embedded no-SQL database so that further inquiries can be made to correct endpoint.

## Installation

Both Mock.Service.SimulationRunner and Service.LoadBalancer are windows services. Installation using InstallUtil in VS command prompt is necessary.

Mock.Client.WebThinClient is a simple .NET WebForms web-site. Hosting it in local IIS or IIS express is adequate. Anonymous access should be granted.
