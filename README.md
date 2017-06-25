MessagePusher
------------------

a dotnet core application for webhooks


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
    
### usage
- set message senders
    + edit `/publish/config.json`, add:
        ```json
        "ServerJiang": {
            "Token": "{your token}"
        }
        ```
        
        ```
        "Telegram": {
            "Token": "{your bot token}",
            "ChatId": "{chat id}"
        }
        ```
        💡 follow [this link](https://core.telegram.org/bots/api#getting-updates) to get your chat id
- web hook  
    e.g. Github
    + Payload URL
       `http://{your_host}:8001/api/github`
    + content-type
        `application/json`
    + set config
        edit `/publish/config.json`, add:
        ```json
        "GitHub": {
            "Token": "{your webhook secret token}",
            "SendTo": [ "Telegram", "ServerJiang" ]
        },
        ```
- simple site monitor service
    + config
        ```json
        "SiteMonitor": {
            "Sites": [ "{your site, e.g. https://i.caoyue.me/}" ],
            "SendTo": [ "Telegram" ]
        },
        ```
    + cronjob
        ```
        */5 * * * * curl http://{your_host}:8001/api/douyu >/dev/null 2>&1
        ```
- twitch stream notification
    + config
        ```json
        "Twitch": {
            "Channels": [ "{channel id}" ],
            "ClientId": "{your client id}",
            "SendTo": [ "Telegram" ]
        }
        ```
        💡 To get a client ID, register a developer application on the [connections page](https://www.twitch.tv/settings/connections) of your Twitch account.
    + cronjob
        ```
        */5 * * * * curl http://{your_host}:8001/api/douyu >/dev/null 2>&1
        ```   
- 斗鱼开播提醒  
    + config
        ```json
        "DouYu": {
            "Rooms": [ "{room id}" ], 
            "SendTo": [ "Telegram" ]
        }
        ```
        需要提醒的房间 id 填入 `Rooms`
    + 设置 cronjob
        ```
        */5 * * * * curl http://{your_host}:8001/api/douyu >/dev/null 2>&1
        ```
    
