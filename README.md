# Autoscale Azure Container Instance

A proof-of-concept on how you can autoscale Azure Container Instance container groups in a resource group by using with Azure Serverless as a scaling infrastructure.

## How does it work?

1. **Azure Monitor Alerts are used to monitor scaling criteria** and will trigger scale controller to make scaling decisions.
2. **Scale controllers evaluate the current amount of replicas** when they are triggered and take max/min replicas into account. In case scaling actions have to occur, they trigger a dedicated workflow to make it so.
3. **Scale In** workflow queries the current amount of Azure Container Instance container group in the resource group and delete the first one.
4. **Scale Out** workflow will load a deployment template provided in Azure Storage and replace it with the required information for the new container group. After that, it fully relies on Azure Resource Manager (ARM).
5. **Azure Resource Manager provisions a new Azure Container Instance container group** based on the provided template in Azure Storage. When required, it can automatically add an endpoint in Azure Traffic Manager to expose it to clients.

Here's a high-level overview:

![Overview](./media/overview.png)