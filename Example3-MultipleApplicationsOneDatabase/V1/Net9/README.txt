YOU MUST CONFIGURE AZURE SERVICE BUS BEFORE RUNNING THIS EXAMPLE!

Add your endpoint configuration to the appsettings.json file for all microservices (except the client).
Change the ConnectionString value to your Azure Service Bus connection string.

    "ServiceBus": {
      "Azure": {
        "ConnectionString": "Endpoint=", <-- Add your connection string here