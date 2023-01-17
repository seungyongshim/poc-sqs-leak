using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Amazon.SQS.Util;

var sqs1 = new AmazonSQSClient(new AmazonSQSConfig
{
    RegionEndpoint = RegionEndpoint.APNortheast1
});

var sqsClient = () => sqs1;

ParallelOptions parallelOptions = new()
{
    MaxDegreeOfParallelism = 300
};

while (true)
{
    await Parallel.ForEachAsync(Enumerable.Range(0, 300), parallelOptions, async (i, ct) =>
    {
        Console.WriteLine(i);
        var sqs = sqsClient.Invoke();
        var t = await sqs.ReceiveMessageAsync(new ReceiveMessageRequest
        {
            QueueUrl = "https://sqs.ap-northeast-1.amazonaws.com/947146334561/labs-dev-aums-sendmail-local.fifo",
            MaxNumberOfMessages = 1,
            WaitTimeSeconds = 5,
            AttributeNames = new() { "All" },
            MessageAttributeNames = new() { "All" },
        });
    });
}


