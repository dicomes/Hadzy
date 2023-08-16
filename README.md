
### 🚧 **Project Status Notice**

>**This project is currently in its initial development phase. As such, changes to the codebase, documentation, and overall structure can be frequent and significant.** 

# Hadzy Microservices
This is the code repository for HADZY application.
The application will fetch and persist ```YouTube``` comments for a specific ```VideoId```.
It is designed as a set of ```.NET Core Microservices``` communicating via ```RabbitMQ``` message bus.  
Data persistence in ```PostgreSQL``` and logging in ```Seq```.

## Application Components:
**Gateway Service (ASP.NET Core Web Api with Ocelot)**
* The service is responsible for receiving ```HTTP GET``` requests containing a videoId. Upon receiving a videoId, it forwards the videoId to the Data API Service for validation.
* If the Data API Service does not find the videoId in the database, the Gateway API Service sends the new videoId to the ```VideoIdQueue``` in the RabbitMQ system and notifies the UI that comments are loading. If the Data API Service does find the videoId and returns associated comments, the Gateway API Service forwards these comments to the UI and send the videoId to the ```VideoIdQueue```.
* Service will use an instance of ```IRabbitMQClientService``` (from the RabbitMQ Client Library) to send ```VideoIdDTO``` objects to the ```VideoIdQueue``` in RabbitMQ.
* Service will handle user authentication via Patreon or other authentication providers. It receives authentication requests, processes them using the specified provider, and manages session information for authenticated users.By performing authentication at this level, we can ensure that only authenticated requests are passed on to other services.
* The service will implement error handling to respond to the client-side application with appropriate HTTP status codes and messages in case of any failures.

**VideoComments Service (ASP.NET Core Minimal Api)**
* This service is responsible for receiving ```HTTP GET``` requests from the Gateway API Service. When it receives a videoId, it checks whether the videoId exists in the PostgreSQL database. If the videoId exists, it retrieves the comments for that videoId, wraps them into a ```CommentsBatchDTO```, and returns this DTO to the Gateway API Service.
* If the videoId does not exist, it responds with a message that the videoId is not available, allowing the Gateway API Service to send a new videoId to the message bus and notify the UI of the loading state.
* The Data API Service uses Entity Framework Core to interact with the PostgreSQL database and will implement error handling to respond with appropriate HTTP status codes and messages in case of failures.

**VideoExecutionStatus Service (ASP.NET Core Minimal Api)**
* This service is responsible for receiving ```HTTP GET``` requests from the Gateway API Service. When it receives ```GET /api/v1/video-execution/{id}/status```, it checks whether the videoId is currently in execution.
* If videoId is not available it will return ```HTTP 400 notFound```.
* If videoId is available it will return the execution status.

**YouTubeCommentsFetcher Service (ASP.NET Core Worker Service)**
* Worker service is a long-running service that continuously listens to the ```VideoPageToken Queue``` in RabbitMQ.
* For each ```VideoPageTokenDto {videoId, lastPageToken}``` it consumes, it fetches comments from YouTube API starting with the last page token if available.
* Each batch of up to 100 comments is wrapped into a ```CommentsBatchDto``` and sent to a ```CommentsBatch Queue``` in RabbitMQ.
* Execution status is wrapped into a ```CommentsExecutionStatusDto {status: inprogress/processed, progress: 1..100}``` and sent to a ```CommentsExecutionStatus Queue``` in RabbitMQ.
* This service should also log the start, progress, and completion of the fetch operation for each videoId to Seq.
* It implements exception handling to manage potential failures when calling the YouTube API (e.g., rate limits, network failures, etc.).

**YouTubeVideoFetcher Service (ASP.NET Core Minimal Api)**
* This service is responsible for receiving ```GET video-fetcher/api/v1/video/{videoId}``` requests from the Gateway API Service.
* The service will request YouTube API a Video DTO.
* If video exist it will return status code 200(OK) and a ```VideoDto``` with video details.
* If video does not exist it will return status code 404(notFound)

**CommentsStorage Service (ASP.NET Core Worker Service)**
* This is another long-running service that continuously listens to all the ```CommentsQueue``` queues in RabbitMQ. For each ```CommentsBatchDTO``` it receives, it saves the comments to a PostgreSQL database.
* The service will implement a retry policy in case of transient failures when connecting to the database, as well as error handling for non-recoverable failures.
* It will also log successful operations and any errors to ```Seq```.

**MassTransit with RabbitMQ**
* MassTransit is a lightweight message bus framework for building distributed .NET applications. We use it along with RabbitMQ as the transport layer.
* Our services make use of MassTransit's interfaces and methods for sending and receiving messages to and from queues. The capability to create queues, handle potential RabbitMQ connection failures, implement a retry policy for transient failures, and provide appropriate logging of operations and errors are all handled by MassTransit.
* A shared Data Transfer Object (DTO) is used to ensure a consistent data structure when communicating with RabbitMQ. This DTO is part of our shared models, which are used across services.
* MassTransit also automatically handles message retry, scheduling, and a variety of message patterns, so there's no need to write this code ourselves.

**Logging and Monitoring (Seq/Serilog)**
* All microservices and the RabbitMQ Client Library will use ```Serilog``` for logging their operations and any errors, and these logs will be sent to Seq. This includes each time a videoId is received, when the comments for a videoId start and finish being fetched, when the comments for a videoId are saved to the database, and any errors that occur during these processes.
* When logging, logs will be structured in a way that allows to easily query them based on videoId, operation type (receiving videoId, fetching comments, saving comments), status (started, in progress, completed, error), and timestamp. This will help you effectively monitor the status of processing each video and troubleshoot any issues.
