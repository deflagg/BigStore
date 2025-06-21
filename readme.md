# BigStore Solution Overview

The purpose of this repository is to demonstrate and validate whether a legacy technology—.NET Remoting, first released in 2006—can still function reliably when containerized with Windows Server 2019 and orchestrated on Azure Kubernetes Service (AKS). This experiment aims to objectively assess the compatibility and operational viability of .NET Remoting in a modern cloud-native environment.

> **Note:** The AKS setup for this experiment should use Windows nodes. No particular special configuration is required beyond the standard setup for Windows-based containers in AKS.

This solution consists of two main ASP.NET web applications—**BigStore** and **Subsystem**—that communicate with each other using **.NET Remoting**. The solution is designed for modularity and scalability, with each subsystem able to run independently or as part of a larger distributed system.

## Project Structure

- **BigStore/**: Main web application (ASP.NET MVC)
- **BigStoreBL/**: Business logic for BigStore
- **Subsystem/**: Secondary web application (ASP.NET MVC)
- **SubsystemBL/**: Business logic for Subsystem

## .NET Remoting Integration

### What is .NET Remoting?
.NET Remoting is a legacy Microsoft technology introduced in **2002** with the first release of the .NET Framework. It allows applications to communicate across application domains, processes, or network boundaries, enabling objects in different processes (or on different machines) to interact as if they were local.

**.NET Remoting pre-dates and was eventually replaced by newer technologies such as:**
- **Windows Communication Foundation (WCF)** (introduced in 2006, .NET Framework 3.0)
- **gRPC** (modern, cross-platform, high-performance RPC framework)
- **RESTful APIs** (using ASP.NET Web API, introduced in 2012)

### How It Works in This Solution
- **BigStore** and **Subsystem** each expose business logic objects (e.g., managers, services) via .NET Remoting.
- These objects are registered as remote services, typically using TCP channels.
- Each application acts as both a remoting server (exposing its own objects) and a remoting client (consuming objects from the other app).
- Communication is handled by referencing shared interfaces or contracts (e.g., in `BigStoreBL` and `SubsystemBL`).

### Example Architecture

```
+----------------+         .NET Remoting (TCP)       +----------------+
|   BigStore     | <-------------------------------> |   Subsystem    |
|  (Web + BL)    |                                   |  (Web + BL)    |
+----------------+                                   +----------------+
```

- Both apps host their own remoting endpoints.
- Each can call business logic methods on the other via remoting proxies.

### Why Use .NET Remoting?
- Enables distributed processing and separation of concerns.
- Allows for independent scaling and deployment of subsystems.
- (Note: .NET Remoting is now considered legacy; for new projects, consider WCF, gRPC, or REST APIs.)

## Deployment & Running (AKS, Docker, Kubernetes)

The following commands show how to build, run, and deploy the applications using Docker, Azure Container Registry, and Azure Kubernetes Service:

```sh
# Prerequisites: Azure CLI, Docker, and kubectl must be installed and configured

# Log in to Azure with the specified tenant
az login --tenant 35d231ad-d70d-

# Set the active Azure subscription
az account set --subscription 400acf99-3ce6-4ee6-

# Log in to Azure Container Registry
az acr login --name myacrmonkey

# Get AKS cluster credentials for kubectl
az aks get-credentials --resource-group migration --name myakscluster

# ----------- Build and deploy BigStore -----------

# Build the BigStore Docker image
docker build -t myacrmonkey.azurecr.io/bigstore:latest -f ./BigStore_Dockerfile .

# Run BigStore container locally for testing
docker run -d -p80:80 --name bigstore bigstore:latest 

# Push BigStore image to Azure Container Registry
docker push myacrmonkey.azurecr.io/bigstore:latest

# Create BigStore deployment in AKS
kubectl create deployment mybigstore --image=myacrmonkey.azurecr.io/bigstore:latest

# Expose BigStore as a LoadBalancer service
kubectl expose deployment mybigstore --name bigstoreservice --type=LoadBalancer --port=80 --target-port=80

# Create ingress for BigStore
kubectl create ingress bigstoreingress --class=nginx --rule="/=bigstoreservice:80"

# ----------- Build and deploy Subsystem -----------

# Build the Subsystem Docker image
docker build -t myacrmonkey.azurecr.io/subsystem:latest -f ./Subsystem_Dockerfile .

# Run Subsystem container locally for testing
docker run -d -p81:80 -p8282:8282 --name subsystem subsystem:latest 

# Push Subsystem image to Azure Container Registry
docker push myacrmonkey.azurecr.io/subsystem:latest

# Create Subsystem deployment in AKS
kubectl create deployment mysubsystem --image=myacrmonkey.azurecr.io/subsystem:latest

# Expose Subsystem as a LoadBalancer service
kubectl expose deployment mysubsystem --name subsystem --type=LoadBalancer --port=80 --target-port=80

# Add custom port 8282 to Subsystem service
kubectl patch service subsystem --type=merge -p "{\"spec\":{\"ports\":[{\"name\":\"http\",\"port\":80,\"targetPort\":80},{\"name\":\"custom-port\",\"port\":8282,\"targetPort\":8282}]}}"

# Create ingress for Subsystem
kubectl create ingress subsystemingress --class=nginx --rule="/=subsystemingress:80"
```

---

For more details, see the source code and comments in each project. For questions about .NET Remoting, see the official [Microsoft documentation](https://learn.microsoft.com/en-us/dotnet/framework/remoting/).
