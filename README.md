# Autoscale Azure Container Instance with Azure Serverless

A proof-of-concept on how you can autoscale Azure Container Instance container groups in a resource group by using with Azure Serverless as a scaling infrastructure.

## How does it work?

1. **Azure Monitor Alerts are used to monitor scaling criteria** and will trigger the scale trigger orchestrators
2. **Scale trigger orchestrators are in charge of enabling/disabling Scale Trigger Request workflows**. These are triggered with a schedule so that it initiates the scale controller to make scaling decisions every x minutes.
3. **Scale controllers evaluate the current amount of replicas** when they are triggered and take max/min replicas into account. In case scaling actions have to occur, they trigger a dedicated workflow to make it so and wait until it completes to trigger a scaling event.
4. **Scale In** workflow queries the current amount of Azure Container Instance container group in the resource group and delete the first one.
5. **Scale Out** workflow will load a deployment template provided in Azure Storage and replace it with the required information for the new container group. After that, it fully relies on Azure Resource Manager (ARM).
6. **Azure Resource Manager provisions a new Azure Container Instance container group** based on the provided template in Azure Storage. When required, it can automatically add an endpoint in Azure Traffic Manager to expose it to clients.

Here's a high-level overview:

![Overview](./media/overview.png)

### Why do we need the scale triggers?

Our goal was to fully rely on Azure Monitor as a scaling trigger but it only sends notifications once the alert state is transitioning from `Actived` → `Resolved` or vice versa which is not ideal if you want to keep on adding/removing instances. That's why the *scale trigger* approach was added to provide recurring scaling actions when the state remains the same.

## How do I deploy it?

You can easily deploy the POC:

1. Create a workload ARM template that will be used to spin up new instances.
    - The parameters file should have `NAME_PLACEHOLDER` as value for the container group name which will be replaced at runtime.
    - Use one of our sample workloads from [`deploy/app`](deploy/app/README.md)
2. Deploy our autoscaler infrastructure ARM template (`deploy/autoscaler/autoscaler-infrastructure.json`)
3. Upload your workload ARM template & parameters file to the `templates` blob container of the created storage account
4. Deploy our autoscaler runtime ARM template (`deploy/autoscaler/autoscaler-infrastructure.json`)
5. Authenticate the Azure Container Instance & ARM API connection that was created
6. Scale!
