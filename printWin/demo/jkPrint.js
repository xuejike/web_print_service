function jkPrintSend(paperWidth, paperHeight, pages,successFun,errorFun) {
    var query = "http://127.0.0.1:19999/";
    if (paperWidth >0 && paperHeight>0) {
        query = query + "?paperWidth=" + paperWidth + "&paperHeight=" + paperHeight;
    }
    var postBody = "";
    for (var i = 0; i < pages.length; i++) {
        postBody += pages[i] + "#";
    }
    $.post(query,postBody,
        function (res) {
            successFun(res);
            console.log(res);
        });
}
var printDefaultOpt= {
    width: 0, height: 0, page: ".print-page",
    success: function() {
        
    },
    error: function(msg) {
        
    }
}
function jkPrint(opt) {
    var printDefaultOpt = {
        width: 0, height: 0, page: ".print-page",
        success: function () {

        },
        error: function (msg) {

        }
    }
    if (opt == undefined) {
        opt = {};
    }
    $.extend(printDefaultOpt, opt);

    var pages = $(printDefaultOpt.page);
    if (pages.length > 0) {
        var printData = [];

        convertImage(printDefaultOpt, pages, 0, printData);

    } else {
        console.error("打印页面不存在");
        return false;
    }
}
function convertImage(opt, pageDiv, pageNo, pageData) {
    let _this = this;
    if (pageNo < pageDiv.length) {
        html2canvas(pageDiv[pageNo], { background: "#fff" }).then(function (canvas) {
            pageData[pageNo] = canvas.toDataURL("image/jpeg", 1.0);
            pageNo++;
            _this.convertImage(opt, pageDiv, pageNo, pageData);
        });
    } else {
        _this.jkPrintSend(opt.width, opt.height, pageData,
            function () {
                opt.success();
            },
            function (msg) {
                opt.error(msg);
            });
    }
}