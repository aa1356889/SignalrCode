
SignalrClient = (function () {
    function client(user) {
        $.connection.hub.url = "http://localhost:4568/signalr";
        $.connection.hub.qs = user;
        var chat = $.connection.MyHub;
        var lmm = null;
        //给指定客户端推送好友与群组信息列表
        chat.client.receive = function (datas) {
            if (datas.sendid == user.Uid) return;
            console.log(datas);
            lmm.getMessage(datas);
        };
        chat.client.receiveOffline = function (datas) {
            datas.forEach(function (c) {
                  lmm.getMessage(c);
            });
        }
        chat.client.receiveGroupOffline = function (datas) {
            datas.forEach(function (c) {
                lmm.getMessage(c);
            });
        }
        //layim配置
        layui.use('layim', function (layim) {

            //基础配置
            layim.config({

                //初始化接口
                init: {
                    url: '/Home/GetUserInfo',
                    data: {}
                }

                //简约模式（不显示主面板）
                //,brief: true

                //查看群员接口
                ,
                members: {
                    url: '/Home/GetMenber',
                    data: {}
                },
                uploadImage: {
                    url: '' //（返回的数据格式见下文）
                    ,
                    type: '' //默认post
                },
                uploadFile: {
                    url: '' //（返回的数据格式见下文）
                    ,
                    type: '' //默认post
                }

                //,skin: ['aaa.jpg'] //新增皮肤
                //,isfriend: false //是否开启好友
                //,isgroup: false //是否开启群组
                //,min: true //是否始终最小化主面板（默认false）
                ,
                chatLog: './demo/chatlog.html' //聊天记录地址
                ,
                find: './demo/find.html'
                ,copyright: true //是否授权
            });
          
            layim.on('sendMessage', function (data) {
                var message = { MessageContent: data.mine.content, SendId: data.mine.id, ReceiveId: data.to.id, MessageType: data.to.type }//type为user表示用户对用户
                SendMessage(message);

            });
            lmm = layim;
            //监听在线状态的切换事件
            layim.on('online', function (data) {
                console.log(data);
            });
           
        });


    }
    function SendMessage(message) {
    
        $.ajax({
            type: "POST",
            url: "/Home/Sendmessage",
            data: message,
            success: function (ajadata) {
                myCommon.Ajax.ResponseReulstProcess(ajadata, function () {

                });
            }
        });
    }
    client.prototype.Connection = function() {
        var th = this;
     
        $.connection.hub.start().done(function () {
         
         
        });

      
    }

 
    return client;
})();