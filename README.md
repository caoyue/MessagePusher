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
- reload config file without restart app
    request `http://{your_host}:8001/api/reload`
- set message senders
    + edit `/publish/config.json`, add:
        ```json
        "Sender": {
            "{your sender name}": {
                "Type": "{required, sender type}",
                "{xxx}": "{sender config field}"
            }
        }
        ```
        e.g.
        ```json
        "Sender": {
            "SendToAlice": {
                "Type": "ServerJiang",
                "Token": ""
            },
            "SendToBob": {
                "Type": "Telegram",
                "Token": "",
                "ChatId": ""
            }
        }
        ```
        💡 telegram users: follow [this link](https://core.telegram.org/bots/api#getting-updates) to get your chat id
        
- web hook  
    e.g. Github
    + Payload URL
       `http://{your_host}:8001/api/github`
    + content-type
        `application/json`
    + set config
        edit `/publish/config.json`, add:
        ```json
        "Receiver": {
            "GitHub": {
                "Token": "",
                "SendTo": [ "your sender name" ]
            }
        }
        ```
- simple site monitor service
    + config
        ```json
        "Receiver": {
            "SiteMonitor": {
                "Sites": [ "{your site, e.g. https://i.caoyue.me/}" ],
                "SendTo": [ "{your sender name}" ]
            }
        }
        ```
    + cronjob
        ```
        */5 * * * * curl http://{your_host}:8001/api/douyu >/dev/null 2>&1
        ```
- twitch stream notification
    + config
        ```json
        "Receiver": {
            "Twitch": {
                "Channels": [ "{twitch channel id}" ],
                "ClientId": "{your client id}",
                "SendTo": [ "{your sender name}" ]
            }
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
        "Receiver": {
            "DouYu": {
                "Rooms": [ "{douyu room id}" ], 
                "SendTo": [ "{your sender name}" ]
            }
        }
        ```
        需要提醒的房间 id 填入 `Rooms`
    + 设置 cronjob
        ```
        */5 * * * * curl http://{your_host}:8001/api/douyu >/dev/null 2>&1
        ```
