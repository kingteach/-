/* NUGET: BEGIN  
*
* ����ʹ�õ�һЩ���ܺ��� edit by wyh@2014.12.26
*
* NUGET: END  */
/**
   * �Զ���ajax
 **/
$(function () {
    $.pAjax = function (options, aimDiv) {
        // ajax ���� Ԫ�� begin
        var img = $('<img id="processImg" src="/Content/images/ajaxloader.gif" />');
        var mask = $('<div id="maskOfprocessImg"></div>').addClass("mask");
        var positionStyle = "fixed";
        //�Ƿ�Loading�̶���aimDiv�в���,����Ĭ��Ϊȫ��Loading����
        if (aimDiv != null && aimDiv != "" && aimDiv != undefined) {
            $(aimDiv).css("position", "relative").append(img).append(mask);
            positionStyle = "absolute";
        } else {
            $("body").append(img).append(mask);
        }
        img.css({
            "z-index": "2000",
            "display": "none"
        });
        mask.css({
            "position": positionStyle,
            "top": "0",
            "right": "0",
            "bottom": "0",
            "left": "0",
            "z-index": "1000",
            "background-color": "#000000",
            "display": "none"
        });
        // ajax ���� Ԫ�� end

        var complete = options.complete;
        options.complete = function (httpRequest, status) {
            img.remove();
            mask.remove();
            if (complete) {
                complete(httpRequest, status);
            }
        }
        options.async = true;

        // ��ʾ���� 
        img.show().css({
            "position": positionStyle,
            "top": "50%",
            "left": "50%",
            "margin-top": function () { return -1 * img.height() / 2; },
            "margin-left": function () { return -1 * img.width() / 2; }
        });
        mask.show().css("opacity", "0.1");
        $.ajax(options);
    }
})

/**
  * Urlƴ��
**/
function appendQueryString(url, parameters) {
    url += "?"
    for (var key in parameters) {
        if (parameters[key] === null || parameters[key] === undefined || parameters[key] === "") {
            continue;
        }
        url += key + "=" + encodeURIComponent(parameters[key]) + "&";
    }
    return url;
}

// ���ڸ�ʽ������Ҫʹ�õ�jquery.formatDate js��
function formatedate(begin) {
    var date1 = eval('new ' + (begin.replace(/\//g, '')))
    return $.formatDate("yyyy-MM-dd HH:ss:mm", date1);
}