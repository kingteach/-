viewModel = function (options) {
    var self = this;

    // CRUD均通过ajax调用实现，这里提供用于获取ajax请求地址的方法
    self.urls = options.urls;

    // 作为显示数据的表格的头部：显示文字和对应的字段名（辅助排序）
    self.tbHeaders = ko.observableArray(options.tbHeaders);

    // 查询条件：查询字段名
    self.form = ko.mapping.fromJS(options.form);
    delete self.form.__ko_mapping__;

    // 排序
    //orderBy，defaultOrderBy & isAsc: 当前排序字段名，默认排序字段名和方向（升序/降序）
    self.orderBy = ko.observable();
    self.isAsc = ko.observable(false);
    self.defaultOrderBy = options.defaultOrderBy;

    // 分页
    //totalPages， pageNumbers & pageIndex：总页数，页码列表和当前页
    self.totalPages = ko.observable();
    self.totalCount = ko.observable();
    self.pageNumbers = ko.observableArray();
    self.pageIndex = ko.observable(1);

    self.dataSource = ko.observableArray();

    self.search = function (callback) {
        var params = getParams();
        params.orderBy(self.defaultOrderBy);
        self.pageIndex(1);// 查询时从第一页开始
        $.pAjax({
            url: self.urls.query,
            data: params,
            type: "GET",
            success: function (result) {
                self.dataSource(result.Data);
                self.totalPages(result.TotalPages)
                self.totalCount(result.TotalCount);
                 
                if (result.PageIndex > 1) {// 代码指定页面时
                    self.pageIndex(result.PageIndex);
                }
                // 重新加载分页
                self.loadPager(result.TotalPages, self.pageIndex(), false);

                if (typeof (callback) != 'undefined')
                    callback();
            }
        });
    }

    // 删除(点击Delete按钮删除当前记录)
    self.del = function (data) {
        //var b = pConfirm("您确定要删除吗？");
        if (confirm("您确定要删除吗？")) {
            $.ajax({
                url: self.urls.del(data, self),
                type: "GET",
                success: function (r) {
                    if (r && r.Result == 1) {
                        self.dataSource.remove(function (item) {
                            return (item.Pk == r.Data.PK);
                        })
                        //var nowCount= self.totalCount() - 1;
                        //var pages = (nowCount / 10) + (nowCount % 10 > 0 ? 1 : 0);
                        //alert(pages);
                        //self.loadPager(pages, false);
                    } else {
                        alert(r.Msg);
                    }
                }
            });
        }
    }

    //点击表格头部进行排序
    self.sort = function (tbHeader) {
        if (self.orderBy() == tbHeader.value) {
            self.isAsc(!self.isAsc());
        }
        self.orderBy(tbHeader.value);
        self.pageIndex(1);

        $.ajax({
            url: self.urls.query,
            data: getParams(),
            type: "GET",
            success: function (result) {
                self.dataSource(result.Data);
                self.loadPager(result.TotalPages, self.pageIndex(), false);
            }
        })
    }

    //点击页码获取当前页数据
    self.turnPage = function (pageIndex) {
      
        var params = getParams();
        params.pageIndex(pageIndex);
        if (options.beforeTurnPage != null && typeof (options.beforeTurnPage) != 'undefined') {
            options.beforeTurnPage();
        }
        $.pAjax({
            url: self.urls.query,
            data: params,
            type: "GET",
            success: function (result) {
                self.dataSource(result.Data);
                if (options.afterTurnPage != null && typeof (options.afterTurnPage) != 'undefined') {
                    options.afterTurnPage();
                }
            }
        });
    }

    self.clear = function () {
        $.each(self.form, function () { this(''); });
        this.search();
    }

    function getParams() {
        var param = ko.toJS(self.form);
        var pager = { "pageIndex": self.pageIndex, "orderBy": self.orderBy, "isAsc": self.isAsc };
        return $.extend({}, param, pager);
    }


    self.loadPager = function (totalPage, pageIndex, isclick) {
        var num_entries = totalPage;
        // 创建分页
        if ($("#pagination").length == 0) return;
        $("#pagination").pagination(num_entries, {
            num_edge_entries: 1, //边缘页数
            num_display_entries: 5, //主体页数
            current_page: pageIndex-1,// 当前选中的页面
            callback: function (page_index, jq) {
                if (isclick) {
                    self.turnPage(page_index + 1);
                    return false;
                } else {
                    isclick = true;
                }
            },
            items_per_page: 1, //每页显示1项
            prev_text: "前一页",
            next_text: "后一页"
        });
    };
}