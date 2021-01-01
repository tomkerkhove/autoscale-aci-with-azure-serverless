# Example Workloads

Here are a few workload ARM templates that you can use to test:

- `sample-worker-app` - Autoscale .NET Queue worker(s) to process Azure Service Bus messages. This uses [KEDA's .NET Queue Worker](https://github.com/kedacore/sample-dotnet-worker-servicebus-queue) which provides a message generator to queue work as well.
- `sample-http-app` - Autoscale HTTP workloads which are being exposed through Azure Traffic Manager. The autoscaller will automatically add the DNS endpoint of the new instance to the traffic profile.
  - This workload requires you to deploy the `sample-http-infrastructure` template first
  - This POC, however, does not support removing DNS endpoints as part of the scale-in but can easily be added
