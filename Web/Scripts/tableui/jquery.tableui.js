/*
 * tableUI 0.1
 * Date: 2014-10-16
 * 使用tableUI可以方便地将表格提示使用体验。先提供的功能有奇偶行颜色交替，鼠标移上高亮显示
 */
(function ($) {
    $.fn.tableUI = function (options) {
        var defaults = {
            evenRowClass: "evenRow",
            oddRowClass: "oddRow",
            activeRowClass: "activeRow",
            clickRowClass: null//"clickRow"// 如果为null 表示没有click事件
        }
        var options = $.extend(defaults, options);
        this.each(function () {
            var thisTable = $(this);
            $(thisTable).find("tr:even").addClass(options.evenRowClass);
            $(thisTable).find("tr:odd").addClass(options.oddRowClass);
            $(thisTable).find("tr").live("mouseover", function () {
                if ($(this).hasClass(options.clickRowClass)) return;// 被选中的不改变颜色
                $(this).removeClass(options.clickRowClass).addClass(options.activeRowClass);
            });
            $(thisTable).find("tr").live("mouseout", function () {
                if ($(this).hasClass(options.clickRowClass)) return;// 被选中的不改变颜色
                $(this).removeClass(options.clickRowClass).removeClass(options.activeRowClass);
            });
            if (options.clickRowClass != null) {
                $(thisTable).find("tr").live("click", function () {
                    $(thisTable).find("." + options.clickRowClass).removeClass(options.clickRowClass).removeClass(options.activeRowClass);// 移除其他选中的行颜色
                    $(this).addClass(options.clickRowClass);
                });
            }
        });
    };
})(jQuery);