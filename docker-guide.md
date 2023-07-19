
# Services are configured to run using Docker Compose.

## Steps to run containers
1. Install docker on local machine
2. Navigate to Hadzy/src/
3. Build container images and start containers:<br>
```Docker-compose -f docker-compose-develop.yml up --build --force-recreate```
4. Create/update Hadzy/src/.env file to support YouTube API Key:<br>
```YOUTUBE_APIKEY=<API_KEY_HERE>```
