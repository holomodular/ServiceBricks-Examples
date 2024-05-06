![ServiceBricks Logo](https://github.com/holomodular/ServiceBricks/blob/main/Logo.png)  

[![NuGet version](https://badge.fury.io/nu/ServiceBricks.svg)](https://badge.fury.io/nu/ServiceBricks)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)

# ServiceBricks Examples

## Overview

This repository contains example applications built using the ServiceBricks foundation.
These examples are intended to provide you guidance on how to build your own applications quickly.
All of these examples use the official pre-built microservices we have available:

* [Cache Microservice](https://github.com/holomodular/ServiceBricks-Cache)
* [Logging Microservice](https://github.com/holomodular/ServiceBricks-Logging)
* [Notification Microservice](https://github.com/holomodular/ServiceBricks-Notification)
* [Security Microservice](https://github.com/holomodular/ServiceBricks-Security)


# Example 1 - One Application One Database

## Purpose

This example demonstrates how to host the ServiceBricks platform in a ** single web application ** and have all pre-built microservices store their data in a ** single database ** using the database engine you specify.

# Example 2 - One Application Multiple Databases

## Purpose

This example demonstrates how to host the ServiceBricks platform in a ** single web application ** and have all pre-built microservices store their data in ** multiple databases ** using the database engine(s) you specify.

# Example 3 - Multiple Applications One Database

## Purpose

This example demonstrates how to host the ServiceBricks platform in ** multiple web applications ** and have all pre-built microservices store their data in a ** single database ** using the database engine you specify.

### Service Bus Required
This example requires the use of a Service Bus provider to communicate asynchronous messages between microservices.
It is setup to use Azure Service Bus, Basic with queues, but you can also use standard/advanced with topics and subscriptions.

# Example 4 - Multiple Applications Multiple Databases

## Purpose

This example demonstrates how to host the ServiceBricks platform in ** multiple web applications ** and have all pre-built microservices store their data in ** multiple databases ** using the database engine(s) you specify.

### Service Bus Required
This example requires the use of a Service Bus provider to communicate asynchronous messages between microservices.
It is setup to use Azure Service Bus, Basic with queues, but you can also use standard or premium with topics and subscriptions.

# Feedback

We want to hear from our users. Let us know if you would like any more examples.


# ServiceBricks

ServiceBricks is the cornerstone for building a microservices foundation.
Visit http://ServiceBricks.com to learn more.
