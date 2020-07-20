myApp.controller('indexController', ['dataFactory', '$scope', function (dataFactory, $scope) {
    var index = this;
    index.searching = false;
    index.currentPage = 1;
    index.maxSize = 5;
    index.totalItems = 0;
    index.pageSize = [10, 20, 30];
    index.currentSize = 10;
    index.datas = [];

    var hub = $.connection.dataHub;

    hub.client.update = function (response) {
        index.setDatas(response);
    }

    index.search = function () {
        index.searching = true;
        $.connection.hub.start().done(function () {
            index.currentPage = 1;
            index.totalItems = 0;
            index.currentSize = 10;
            index.datas = [];
            index.getSearch($.connection.hub.id);
        }).fail(function (result) {
            alert("Can't connected to the hub");
        });
    }

    index.getSearch = function (connectionId) {
        dataFactory.search({
            connectionId: connectionId || null,
            searchCriteria: {
                Condition: index.condition
            },
            SearchFilter: {
                PageNo: index.currentPage,
                PageSize: index.currentSize
            }
        }, index.setDatas);
    }

    index.changePageSize = function (size) {
        index.currentSize = size;
        index.currentPage = 1;
        index.getSearch();
    }

    index.changeFilter = function () {
        index.getSearch();
    }

    index.setDatas = function (response) {
        if (typeof response == "object") {
            index.totalItems = response.TotalItems;
            index.datas = response.Datas;
            index.searching = !response.IsCompleted;

            if (response.IsCompleted && $.connection.hub && $.connection.hub.state === $.signalR.connectionState.connected) {
                $.connection.hub.stop();
            }
            $scope.$apply();
        }
    }
}]);