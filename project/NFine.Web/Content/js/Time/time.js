/**
 * 毫秒转换友好的显示格式
 * 输出格式：21小时28分钟15秒
 * @param  {[type]} time [description]
 * @return {[type]}      [description]
 */
function timeToDate(time) {
    // 获取当前时间戳
    var currentTime = parseInt(new Date().getTime() / 1000);
    var diffTime = currentTime - time;
    var second = 0;
    var minute = 0;
    var hour = 0;
    if (null != diffTime && "" != diffTime) {
        if (diffTime > 60 && diffTime < 60 * 60) {
            diffTime = "<span class='layui-red'>刚刚</span>";
        }
        else if (diffTime >= 60 * 60 && diffTime < 60 * 60 * 24) {
            diffTime = "<span class='layui-red'>今日</span>";
        }
        else {
            //超过1天
            var date = new Date(parseInt(time) * 1000);
            var day = date.getDate();
            if (day <= 9) {
                day = "0" + day ;
            }
            diffTime = day + "日";
        }
    }
    return diffTime;
}