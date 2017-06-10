### install dotnet core environment
- ref [https://www.microsoft.com/net/core#linuxubuntu](https://www.microsoft.com/net/core#linuxubuntu)

### how to
- install dotnet core environment
- clone project
- run `dotnet restore`
- run `dotnet publish -c Release -o publish`
- cd publish folder
- run `dotnet MessagePusher.Web.dll`

### set web hook
- Github  
    `http://{your_host}:8001/api/github`
- GitLab  
    `http://{your_host}:8001/api/gitlab`
