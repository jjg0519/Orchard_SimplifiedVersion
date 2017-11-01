//获取Controller描述
function SwaggerCustom() {
    this.setControllerSummary = function () {
        $.ajax({
            type: "get",
            async: true,
            url: $("#input_baseUrl").val(),
            dataType: "json",
            success: function (data) {
                var summaryDict = data.ControllerDesc;
                var id, controllerName, strSummary;
                $("#resources_container .resource").each(function (i, item) {
                    id = $(item).attr("id");
                    if (id) {
                        controllerName = id.substring(9);
                        strSummary = summaryDict[controllerName];
                        if (strSummary) {
                            $(item).children(".heading").children(".options").prepend('<li style="color:red;" class="controller-summary" title="' + strSummary + '">' + strSummary + '</li>');
                        }
                    }
                });
                swaggerCustom.loadMenu(data.AreaDescription);
                expendtoggle();//注册菜单收缩事件
            }
        });

    };
    //获取当前参数
    this.getQueryString = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    },
    this.loadMenu = function (modules) {
        var url = window.location.href;
        url = url.substring(0, url.lastIndexOf('?'));
        $('body').prepend('<div class="menu-expender" id="menuExpend">←</div><div id="moduleMenu"><div onclick="menuClick(this)" data-url="' + url + '?module=all"  " data-module="all" class="menu-inner" >全部</div></div>');
        var menuInner = '';
        modules.forEach(function (item) {
            menuInner += '<div onclick="menuClick(this)" data-url="' + url + '?module=' + item.toLowerCase() + '" data-module="' + item.toLowerCase() + '" class="menu-inner" >' + item + '</div>';
        });
        $('#moduleMenu').append(menuInner);
        $('#moduleMenu').css("position", "fixed").css("top", "20%");
    }
}
var swaggerCustom = new SwaggerCustom();
var swaggerCustomGlobalData = {
    currentModule: "all"
}
$(function () {
    swaggerCustomGlobalData.currentModule = swaggerCustom.getQueryString('module') == null ? "all" : swaggerCustom.getQueryString('module');
    //alert(swaggerCustomGlobalData.currentModule);
});
var swaggerStyle = {
    showActionLink: function () {
        $("li .toggleEndpointList").css("color", "#2392f7");
    },
    titleStyle: function () {
        $("h2 .toggleEndpointList").css("color", "green");
    },
    showDetailLink: function () {
        $("li .expandResource").css('color', '#996633');
    },
    paramTable: function () {
        $('.fullwidth.parameters thead tr th:nth-child(1)').width('50px');
        $('.fullwidth.parameters thead tr th:nth-child(2)').width('350px');
        $('.fullwidth.parameters thead tr th:nth-child(3)').width('100px');
        $('.fullwidth.parameters thead tr th:nth-child(4)').width('60px');
        $('.fullwidth.parameters thead tr th:nth-child(5)').width('400px');
        $('td textarea').width('380px');
    },
    init: function () {
        this.showActionLink();
        this.titleStyle();
        this.showDetailLink();
        //this.paramTable();
    }
}

function menuClick(ele) {
    window.location.href = (ele.dataset.url);
}
function expendtoggle() {
    $('#menuExpend').toggle(function () {
        $(this).html('→');
        $('#moduleMenu').hide();
    }, function () {
        $(this).html('←');
        $('#moduleMenu').show();
    });
}

