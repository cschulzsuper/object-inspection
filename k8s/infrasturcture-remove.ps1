kubectl delete -f azurecosmosdb-secret.yaml
kubectl delete -f azurecosmosdb-service.yaml
kubectl delete -f azurecosmosdb-deployment.yaml
kubectl delete -f azurite-secret.yaml
kubectl delete -f azurite-service.yaml
kubectl delete -f azurite-deployment.yaml
#kubectl port-forward deployment/azurecosmosdb 8081:8081
#kubectl port-forward deployment/azurecosmosdb 10251:10251
#kubectl port-forward deployment/azurecosmosdb 10252:10252
#kubectl port-forward deployment/azurecosmosdb 10253:10253
#kubectl port-forward deployment/azurecosmosdb 10254:10254
#kubectl port-forward deployment/azurite 10000:10000