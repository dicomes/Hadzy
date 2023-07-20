##Listening to the VideoPageToken Queue:
Use a RabbitMQ client library (MassTransit) to continuously listen to the ```VideoPageToken Queue```.
Upon receiving a ```VideoPageTokenDto```, deserialize it and process.

##Fetching Comments from YouTube API:
Use the YouTube API V3 or make HTTP requests to fetch comments for the given videoId.
If lastPageToken is provided, start fetching from that token, otherwise fetch from the beginning.
Paginate through the comments as per YouTube API's pagination mechanism.

##Sending Comments to CommentsBatch Queue:
Group comments into batches of up to 100.
For each batch, wrap it in a ```CommentsBatchDto``` and send it to the ```CommentsBatch Queue```.

##Updating Execution Status:
Once processing a ```videoId```, send ```CommentsExecutionStatusDto``` with progress: 1 to the ```CommentsExecutionStatus Queue```.
As you paginate through comments, calculate the progress percentage and send updates.
Once all comments are fetched and sent, send ```CommentsExecutionStatusDto``` with progress: 100.

##Logging to Seq:
Serilog with ```Seq```.
Log the start, progress, and completion of the fetch operation for each videoId.

#Exception Handling:
Implement try-catch blocks around critical operations.
For YouTube API specific errors (like rate limits), implement a retry mechanism. You can use libraries like Polly for sophisticated retry policies.
For critical failures, consider re-queuing the ```VideoPageTokenDto``` back to RabbitMQ with a delay, so it can be retried later.

##Configuration & Deployment:
Store sensitive data like API keys in user secrets during development and in secure configuration providers in production.
Containerize the worker service using Docker for easy deployment.
