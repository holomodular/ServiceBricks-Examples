![ServiceBricks Logo](https://github.com/holomodular/ServiceBricks/blob/main/Logo.png)  

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)

# ServiceBricks Example QuickStart Applications

## Overview

This repository contains example applications built using the ServiceBricks foundation.
These examples are intended to provide you guidance on how to build your own applications quickly.
All of these examples use the official pre-built microservices we have available:

* [Cache Microservice](https://github.com/holomodular/ServiceBricks-Cache)
* [Logging Microservice](https://github.com/holomodular/ServiceBricks-Logging)
* [Notification Microservice](https://github.com/holomodular/ServiceBricks-Notification)
* [Security Microservice](https://github.com/holomodular/ServiceBricks-Security)



# Starting Multiple Projects
Each of the examples below contain multiple web applications.
After opening the solution in Visual Studio, right click the solution and select the options for **Configure Startup Projects**. 
Select the radio option for Multiple Startup Projects and change the drop down for each project to be Start.
When you run the application, multiple browser windows will be displayed for each web application.


# Deployment Examples

## Example 1 - One Application One Database

### Purpose

This example demonstrates how to host the ServiceBricks platform in a **single web application** and have all pre-built microservices store their data in a **single database** using the database engine you specify.

#### In-Memory Service Bus

Since all microservices are hosted within the same web application, we use the In-Memory Service Bus provider to allow microservices to communicate with each other.

### Diagram

![Example 1 Diagram](https://github.com/holomodular/ServiceBricks-Examples/blob/main/Example1-OneApplicationOneDatabase/Example1.png)  


## Example 2 - One Application Multiple Databases

### Purpose

This example demonstrates how to host the ServiceBricks platform in a **single web application** and have all pre-built microservices store their data in **multiple databases** using the database engine(s) you specify.

#### In-Memory Service Bus

Since all microservices are hosted within the same web application, we use the In-Memory Service Bus provider to allow microservices to communicate with each other.

### Diagram

![Example 2 Diagram](https://github.com/holomodular/ServiceBricks-Examples/blob/main/Example2-OneApplicationMultipleDatabases/Example2.png) 


## Example 3 - Multiple Applications One Database

### Purpose

This example demonstrates how to host the ServiceBricks platform in **multiple web applications** and have all pre-built microservices store their data in a **single database** using the database engine you specify.

#### Service Bus Required

This example requires the use of a Service Bus provider to communicate asynchronous messages between microservices.
It is setup to use Azure Service Bus, Basic with queues, but you can also use standard/advanced with topics and subscriptions.

### Diagram

![Example 3 Diagram](https://github.com/holomodular/ServiceBricks-Examples/blob/main/Example3-MultipleApplicationsOneDatabase/Example3.png) 


## Example 4 - Multiple Applications Multiple Databases

### Purpose

This example demonstrates how to host the ServiceBricks platform in **multiple web applications** and have all pre-built microservices store their data in **multiple databases** using the database engine(s) you specify.

#### Service Bus Required

This example requires the use of a Service Bus provider to communicate asynchronous messages between microservices.
It is setup to use Azure Service Bus, Basic with queues, but you can also use standard/advanced with topics and subscriptions.

#### Service-Specific Logging (option)

By using a separate logging database for each web application, you achieve service-specific logging. 
You will have to query each logging microservice on each web application to view all messages in the infrastructure.

#### Centralized Logging (option)

By using the same logging database for all logging microservices in all web applications, you achieve centralized logging. 
You can query one place to view logging messages for the entire infrastructure.

### Diagram
![Example 4 Diagram](https://github.com/holomodular/ServiceBricks-Examples/blob/main/Example4-MultipleApplicationsMultipleDatabases/Example4.png) 

Alternatively, you can also setup:

![Example 4 Diagram](https://github.com/holomodular/ServiceBricks-Examples/blob/main/Example4-MultipleApplicationsMultipleDatabases/Example4CentralizedLogging.png) 


# Feedback

We want to hear from our users. Let us know if you would like any more examples.


# About ServiceBricks

ServiceBricks is the cornerstone for building a microservices foundation.
Visit http://ServiceBricks.com to learn more.
