az login --tenant 35d231ad-d70d-
az account set --subscription 400acf99-3ce6-4ee6-
az acr login --name myacrmonkey
az aks get-credentials --resource-group migration --name myakscluster

docker build -t myacrmonkey.azurecr.io/bigstore:latest -f ./BigStore_Dockerfile .
docker run -d -p80:80 --name bigstore bigstore:latest 
docker push myacrmonkey.azurecr.io/bigstore:latest
kubectl create deployment mybigstore --image=myacrmonkey.azurecr.io/bigstore:latest
kubectl expose deployment mybigstore --name bigstoreservice --type=LoadBalancer --port=80 --target-port=80
kubectl create ingress bigstoreingress --class=nginx --rule="/=bigstoreservice:80"


docker build -t myacrmonkey.azurecr.io/subsystem:latest -f ./Subsystem_Dockerfile .
docker run -d -p81:80 -p8282:8282 --name subsystem subsystem:latest 
docker push myacrmonkey.azurecr.io/subsystem:latest
kubectl create deployment mysubsystem --image=myacrmonkey.azurecr.io/subsystem:latest
kubectl expose deployment mysubsystem --name subsystem --type=LoadBalancer --port=80 --target-port=80
kubectl patch service subsystem --type=merge -p "{\"spec\":{\"ports\":[{\"name\":\"http\",\"port\":80,\"targetPort\":80},{\"name\":\"custom-port\",\"port\":8282,\"targetPort\":8282}]}}"
kubectl create ingress subsystemingress --class=nginx --rule="/=subsystemingress:80"
