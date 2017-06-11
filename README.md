### install dotnet core environment
- ref  
    [https://www.microsoft.com/net/core#linuxubuntu](https://www.microsoft.com/net/core#linuxubuntu)

### how to
- install dotnet core environment
- clone project
- run `dotnet restore`
- run `dotnet publish -c Release -o publish`
- cd publish folder
- run `dotnet MessagePusher.Web.dll`

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
    docker run --name message_push_server -p 8001:8001 -v /MessagePusher/publish:/app -d message_pusher
    ```
    
### set web hook
- Github  
    `http://{your_host}:8001/api/github`
- GitLab  
    `http://{your_host}:8001/api/gitlab`
