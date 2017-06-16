MessagePusher
------------------

an dotnet core application for webhooks


### install dotnet core environment
- ref  
    [https://www.microsoft.com/net/core#linuxubuntu](https://www.microsoft.com/net/core#linuxubuntu)

### how to
- install dotnet core environment
- clone project
- cd project and run `dotnet restore`
- run `dotnet publish -c Release -o /home/MessagePusher/publish`
- cd publish folder and run `dotnet MessagePusher.Web.dll`

### run with supervisor
- ref  
    `doc/supervisor.conf`

### run with docker
- build image
    ```bash
    docker build -t message_pusher .
    ```
- run container
    ```bash
    docker run --name message_push_server -p 8001:8001 -v /home/MessagePusher/publish:/app -d message_pusher
    ```
    
### config
- set message senders
    + edit `/publish/config.json`, add:
        ```json
        "ServerJiang": {
            "Token": "{your token}"
        },
        "Telegram": {
            "Token": "{your token}",
            "ChatId": "{chat id}"
        }
        ```
- set web hook  
    e.g. Github
    + Payload URL
       `http://{your_host}:8001/api/github`
    + content-type
        `application/json`
    + set config
        edit `/publish/config.json`, add:
        ```json
        "GitHub": {
            "Token": "",
            "SendTo": [ "Telegram", "ServerJiang" ]
        },
        ```
    
