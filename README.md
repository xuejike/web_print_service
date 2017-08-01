# 网页打印控件
## 1.打印协议说明
### 1.1 基本打印
发送Http请求到本地 http://127.0.0.1:9999/?paperWidth=纸张宽度&paperHeight=纸张高度
post域中写需要打印的图片的Base64格式数据。
### 1.2 多页打印
多页打印只需在Post域中的打印图片数据使用  #  隔开就行。
### 1.3 安装
只需下载打开运行 printWin.exe 

## 2.打印JS插件
JS插件使用html2canvas 工具将当前页面需要打印的内容生成图片并生成Base64数据，然后使用jquery 发送给本地地址。
js插件文件存在demo中
```javascript

    jkPrint({
        width:150,//纸张宽度
        height:60,//纸张高度
        page:".print-page", //自动获取class 为printPage的Html元素进行打印，多个printPage 会自动进行分页
        success:function(){
            //打印成功的回调
        },
        error:function(error){
            //打印失败的回调
        }
    })


```
