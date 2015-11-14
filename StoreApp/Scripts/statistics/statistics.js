'use strict';

var productsApp = angular.module('productsApp', []);

productsApp.controller('ProductsCtrl', function ($scope) {
    $scope.model = {
        selectedProduct: null,
        isLoading: false,
        products: []
    };

    $scope.selectProduct = function (product) {
        $scope.model.selectedProduct = product;
    };

    $scope.updateProductsList = function () {
        var model = $scope.model;
        model.selectedProduct = null;
        model.isLoading = true;

        $.ajax({
            url: 'Statistics/GetProductsSum',
            method: 'POST',
            dataType: 'json',
            success: function (res) {
                model.isLoading = false;
                model.products = res;

                $scope.$apply();
            },
            error: function (res) {
                model.isLoading = false;
                alert('error - ' + res);
                console.log(res);
            }
        });

        return true;
    };

    $scope.makeProduct = function () {
        var model = $scope.model;

        if (!model.selectedProduct)
            return;

        model.isLoading = true;

        $.ajax({
            url: 'Statistics/MakeProduct',
            method: 'POST',
            data: { id: model.selectedProduct.productId },
            dataType: 'json',
            success: function (res) {
                model.isLoading = false;

                if (res.result && res.result === 'error')
                {
                    alert(res.message);
                    $scope.$apply();
                    return;
                }

                alert('Продукт создан');//алерт, но может быть Bootstrap UI диалог

                $scope.updateProductsList();
            },
            error: function (res) {
                model.isLoading = false;
                alert('error');
                console.log(res);
            }
        });
    };

    $scope.isEnableMakeButton = function () {
        var model = $scope.model;

        return model.selectedProduct != null && model.selectedProduct.count > 0;
    };

    $scope.updateProductsList();
});